using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Domain.Ports.Players;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;

namespace Infrastructure.Persistence.Players.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerRepository> _logger;
        private readonly PlayerMapper _mapper;

        public PlayerRepository(ApplicationDbContext context, ILogger<PlayerRepository> logger, PlayerMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // Obtiene todos los jugadores usando una consulta SQL explícita.
        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM Players";
                var dbPlayers = await _context.Players
                    .FromSqlRaw(sql)
                    .ToListAsync();

                // Obtiene todas las relaciones de TeamPlayers (para todos los jugadores)
                var teamPlayers = await _context.TeamPlayers.ToListAsync();

                // Mapea cada entidad a dominio usando el mapper (filtrando las relaciones correspondientes)
                var result = dbPlayers.Select(dbPlayer =>
                    _mapper.MapToDomain(dbPlayer, teamPlayers.Where(tp => tp.PlayerID == dbPlayer.PlayerID).ToList()))
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de jugadores");
                return new List<Player>();
            }
        }

        // Obtiene un jugador por su PlayerID
        public async Task<Player?> GetByIdAsync(PlayerID playerId)
        {
            try
            {
                string sql = "SELECT * FROM Players WHERE PlayerID = @PlayerID";
                var parameter = new SqlParameter("@PlayerID", playerId.Value);

                var dbPlayers = await _context.Players
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbPlayer = dbPlayers.FirstOrDefault();

                if (dbPlayer == null)
                    return null;

                // Para obtener sus relaciones, obtenemos los registros de TeamPlayers correspondientes
                var teamPlayers = await _context.TeamPlayers
                    .Where(tp => tp.PlayerID == dbPlayer.PlayerID)
                    .ToListAsync();

                return _mapper.MapToDomain(dbPlayer, teamPlayers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con ID {PlayerID}", playerId.Value);
                return null;
            }
        }

        // Obtiene un jugador por nombre (exacto)
        public async Task<Player?> GetByNameAsync(string playerName)
        {
            try
            {
                string sql = "SELECT * FROM Players WHERE Name = @Name";
                var parameter = new SqlParameter("@Name", playerName);

                var dbPlayers = await _context.Players
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbPlayer = dbPlayers.FirstOrDefault();

                if (dbPlayer == null)
                    return null;

                var teamPlayers = await _context.TeamPlayers
                    .Where(tp => tp.PlayerID == dbPlayer.PlayerID)
                    .ToListAsync();

                return _mapper.MapToDomain(dbPlayer, teamPlayers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con Nombre {PlayerName}", playerName);
                return null;
            }
        }

        // Obtiene los jugadores asociados a un equipo mediante la tabla intermedia
        public async Task<IEnumerable<Player>> GetByTeamIdAsync(TeamID teamId)
        {
            try
            {
                // Se obtiene primero la lista de PlayerIDs que están asociados al equipo
                string sql = "SELECT DISTINCT PlayerID FROM TeamPlayers WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);
                var playerIds = await _context.TeamPlayers
                    .FromSqlRaw(sql, parameter)
                    .Select(tp => tp.PlayerID)
                    .ToListAsync();

                // Ahora se obtienen los jugadores cuyos IDs estén en la lista
                string sqlPlayers = "SELECT * FROM Players WHERE PlayerID IN ({0})";
                // Convertimos la lista de IDs en una cadena separada por comas
                string ids = string.Join(",", playerIds);
                sqlPlayers = string.Format(sqlPlayers, ids);

                var dbPlayers = await _context.Players
                    .FromSqlRaw(sqlPlayers)
                    .ToListAsync();

                // Obtenemos las relaciones de TeamPlayers para cada jugador
                var teamPlayers = await _context.TeamPlayers.ToListAsync();

                return dbPlayers.Select(dbPlayer =>
                    _mapper.MapToDomain(dbPlayer, teamPlayers.Where(tp => tp.PlayerID == dbPlayer.PlayerID).ToList()))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los jugadores del equipo con ID {TeamID}", teamId.Value);
                return new List<Player>();
            }
        }

        // Agrega un nuevo jugador a la base de datos mediante consulta SQL
        public async Task<Player> AddAsync(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "El jugador no puede ser null");

            try
            {
                var playerEntity = _mapper.MapToEntity(player);

                string insertSql = @"INSERT INTO Players (Name, Position, Age, Goals, Photo, CreatedAt)
                                     VALUES (@Name, @Position, @Age, @Goals, @Photo, @CreatedAt);";

                var parameters = new[]
                {
                    new SqlParameter("@Name", playerEntity.Name),
                    new SqlParameter("@Position", playerEntity.Position),
                    new SqlParameter("@Age", playerEntity.Age),
                    new SqlParameter("@Goals", playerEntity.Goals),
                    new SqlParameter("@Photo", (object)playerEntity.Photo ?? DBNull.Value),
                    new SqlParameter("@CreatedAt", playerEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                // Obtener el nuevo PlayerID (ejemplo con SQL Server: SELECT SCOPE_IDENTITY())
                string selectSql = "SELECT CAST(SCOPE_IDENTITY() AS INT)";
                var newPlayerId = await _context.Players
                    .FromSqlRaw(selectSql)
                    .Select(p => p.PlayerID)
                    .FirstOrDefaultAsync();

                // Reobtener la entidad agregada
                var dbPlayer = await _context.Players
                    .FirstOrDefaultAsync(p => p.PlayerID == newPlayerId);

                // No existen relaciones inicialmente, por lo tanto se pasa lista vacía.
                return _mapper.MapToDomain(dbPlayer, new List<TeamPlayerEntity>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo jugador");
                throw;
            }
        }

        // Actualiza la información de un jugador mediante consulta SQL
        public async Task UpdateAsync(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "El jugador no puede ser null");

            try
            {
                var playerEntity = _mapper.MapToEntity(player);

                string updateSql = @"UPDATE Players 
                                     SET Name = @Name, Position = @Position, Age = @Age, Goals = @Goals, Photo = @Photo, CreatedAt = @CreatedAt
                                     WHERE PlayerID = @PlayerID";

                var parameters = new[]
                {
                    new SqlParameter("@PlayerID", playerEntity.PlayerID),
                    new SqlParameter("@Name", playerEntity.Name),
                    new SqlParameter("@Position", playerEntity.Position),
                    new SqlParameter("@Age", playerEntity.Age),
                    new SqlParameter("@Goals", playerEntity.Goals),
                    new SqlParameter("@Photo", (object)playerEntity.Photo ?? DBNull.Value),
                    new SqlParameter("@CreatedAt", playerEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el jugador con ID {PlayerID}", player.PlayerID.Value);
                throw;
            }
        }

        // Elimina un jugador mediante consulta SQL
        public async Task<bool> DeleteAsync(PlayerID playerId)
        {
            try
            {
                string sql = "DELETE FROM Players WHERE PlayerID = @PlayerID";
                var parameter = new SqlParameter("@PlayerID", playerId.Value);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el jugador con ID {PlayerID}", playerId.Value);
                return false;
            }
        }
    }
}
