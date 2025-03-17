using Domain.Entities.Players;
using Domain.Ports.Players;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Players.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Get all players
        /// </summary>
        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM Players";
                var dbPlayers = await _context.Players
                    .FromSqlRaw(sql)
                    .ToListAsync();

                return dbPlayers.Select(dbPlayer => _mapper.MapToDomain(dbPlayer, null)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de jugadores");
                return new List<Player>();
            }
        }

        /// <summary>
        /// Get player by ID
        /// </summary>
        public async Task<Player?> GetByIdAsync(int playerId)
        {
            try
            {
                string sql = "SELECT * FROM Players WHERE PlayerID = @PlayerID";
                var parameter = new SqlParameter("@PlayerID", playerId);

                var dbPlayers = await _context.Players
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbPlayer = dbPlayers.FirstOrDefault();
                return dbPlayer != null ? _mapper.MapToDomain(dbPlayer, null) : null; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con ID {PlayerID}", playerId);
                return null;
            }
        }

        /// <summary>
        /// Get player by Name
        /// </summary>
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
                return dbPlayer != null ? _mapper.MapToDomain(dbPlayer, null) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con Nombre {PlayerName}", playerName);
                return null;
            }
        }

        /// <summary>
        /// Get players by Team ID
        /// </summary>
        public async Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId)
        {
            try
            {
                string sql = "SELECT * FROM Players WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId);

                var dbPlayers = await _context.Players
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                return dbPlayers.Select(dbPlayer => _mapper.MapToDomain(dbPlayer, null)).ToList(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los jugadores del equipo con ID {TeamID}", teamId);
                return new List<Player>();
            }
        }

        /// <summary>
        /// Add a new player
        /// </summary>
        public async Task<Player> AddAsync(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "El jugador no puede ser null");

            try
            {
                var playerEntity = _mapper.MapToEntity(player);

                string insertSql = @"INSERT INTO Players (Name, Position, TeamID, CreatedAt) 
                                     VALUES (@Name, @Position, @TeamID, @CreatedAt);";

                var parameters = new[]
                {
                    new SqlParameter("@Name", playerEntity.Name),
                    new SqlParameter("@Position", playerEntity.Position.ToString()),
                    new SqlParameter("@TeamID", playerEntity.TeamID),
                    new SqlParameter("@CreatedAt", playerEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                string selectSql = "SELECT TOP 1 PlayerID FROM Players ORDER BY PlayerID DESC";
                var newPlayerId = await _context.Players
                    .FromSqlRaw(selectSql)
                    .Select(p => p.PlayerID)
                    .FirstOrDefaultAsync();

                return new Player(
                    new PlayerID(newPlayerId),
                    player.Name,
                    new TeamID(playerEntity.TeamID), 
                    player.Position,
                    null, 
                    player.CreatedAt
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo jugador");
                throw;
            }
        }

        /// <summary>
        /// Update an existing player
        /// </summary>
        public async Task UpdateAsync(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "El jugador no puede ser null");

            try
            {
                var playerEntity = _mapper.MapToEntity(player);

                string updateSql = @"UPDATE Players 
                                     SET Name = @Name, Position = @Position, TeamID = @TeamID, CreatedAt = @CreatedAt
                                     WHERE PlayerID = @PlayerID";

                var parameters = new[]
                {
                    new SqlParameter("@PlayerID", playerEntity.PlayerID),
                    new SqlParameter("@Name", playerEntity.Name),
                    new SqlParameter("@Position", playerEntity.Position),
                    new SqlParameter("@TeamID", playerEntity.TeamID),
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

        /// <summary>
        /// Delete a player by ID
        /// </summary>
        public async Task<bool> DeleteAsync(int playerId)
        {
            try
            {
                string sql = "DELETE FROM Players WHERE PlayerID = @PlayerID";
                var parameter = new SqlParameter("@PlayerID", playerId);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el jugador con ID {PlayerID}", playerId);
                return false;
            }
        }
    }
}
