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
using System.Text;
using System;

namespace Infrastructure.Persistence.Players.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerRepository> _logger;
        private readonly IPlayerMapper _mapper;

        public PlayerRepository(
            ApplicationDbContext context,
            ILogger<PlayerRepository> logger,
            IPlayerMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Inserta un nuevo jugador en la tabla Players.
        /// </summary>
        public async Task<Player> AddAsync(Player player)
        {
            var e = _mapper.ToEntity(player);
            const string sql = @"
                INSERT INTO Players
                  (PlayerID, Name, Position, Age, Goals, Photo, CreatedAt)
                VALUES
                  (@ID, @Name, @Position, @Age, @Goals, @Photo, @CreatedAt)";
            var p = new[]
            {
                new SqlParameter("@ID",        e.PlayerID),
                new SqlParameter("@Name",      e.Name),
                new SqlParameter("@Position",  e.Position),
                new SqlParameter("@Age",       e.Age),
                new SqlParameter("@Goals",     e.Goals),
                new SqlParameter("@Photo",     (object?)e.Photo ?? DBNull.Value),
                new SqlParameter("@CreatedAt", e.CreatedAt)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
            return player;
        }

        /// <summary>
        /// Actualiza los datos de un jugador existente.
        /// </summary>
        public async Task UpdateAsync(Player player)
        {
            var e = _mapper.ToEntity(player);
            const string sql = @"
                UPDATE Players
                SET
                  Name     = @Name,
                  Position = @Position,
                  Age      = @Age,
                  Goals    = @Goals,
                  Photo    = @Photo
                WHERE PlayerID = @ID";
            var p = new[]
            {
                new SqlParameter("@ID",       e.PlayerID),
                new SqlParameter("@Name",     e.Name),
                new SqlParameter("@Position", e.Position),
                new SqlParameter("@Age",      e.Age),
                new SqlParameter("@Goals",    e.Goals),
                new SqlParameter("@Photo",    (object?)e.Photo ?? DBNull.Value)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
        }

        /// <summary>
        /// Elimina un jugador por su ID.
        /// </summary>
        public async Task<bool> DeleteAsync(PlayerID playerId)
        {
            const string sql = "DELETE FROM Players WHERE PlayerID = @ID";
            var rows = await _context.Database.ExecuteSqlRawAsync(
                sql,
                new SqlParameter("@ID", playerId.Value));
            return rows > 0;
        }

        /// <summary>
        /// Obtiene un jugador por su ID, incluyendo sus TeamPlayers.
        /// </summary>
        public async Task<Player?> GetByIdAsync(PlayerID playerId)
        {
            var entity = await _context.Players
                .FromSqlRaw("SELECT * FROM Players WHERE PlayerID = @ID",
                    new SqlParameter("@ID", playerId.Value))
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var tps = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE PlayerID = @ID",
                    new SqlParameter("@ID", entity.PlayerID))
                .ToListAsync();

            return _mapper.ToDomain(entity, tps);
        }

        /// <summary>
        /// Devuelve todos los jugadores.
        /// </summary>
        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var entities = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

            var result = new List<Player>();
            foreach (var e in entities)
            {
                var tps = await _context.TeamPlayers
                    .FromSqlRaw("SELECT * FROM TeamPlayers WHERE PlayerID = @ID",
                        new SqlParameter("@ID", e.PlayerID))
                    .ToListAsync();

                result.Add(_mapper.ToDomain(e, tps));
            }

            return result;
        }

        /// <summary>
        /// Busca un jugador por nombre exacto.
        /// </summary>
        public async Task<Player?> GetByNameAsync(string name)
        {
            var entity = await _context.Players
                .FromSqlRaw("SELECT * FROM Players WHERE Name = @Name",
                    new SqlParameter("@Name", name))
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var tps = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE PlayerID = @ID",
                    new SqlParameter("@ID", entity.PlayerID))
                .ToListAsync();

            return _mapper.ToDomain(entity, tps);
        }

        /// <summary>
        /// Obtiene todos los jugadores de un equipo.
        /// </summary>
        public async Task<IEnumerable<Player>> GetByTeamIdAsync(int teamId)
        {
            var tps = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @TID",
                    new SqlParameter("@TID", teamId))
                .Include(tp => tp.Player)
                .ToListAsync();

            return tps
                .Select(tp => _mapper.ToDomain(tp.Player, tps))
                .ToList();
        }

        /// <summary>
        /// Filtra jugadores por posición.
        /// </summary>
        public Task<IEnumerable<Player>> GetByPositionAsync(PlayerPosition position)
            => SearchBySqlAsync(
                "SELECT * FROM Players WHERE Position = @Pos",
                new SqlParameter("@Pos", position.ToString()));

        /// <summary>
        /// Filtra jugadores por rango de edad.
        /// </summary>
        public Task<IEnumerable<Player>> GetByAgeRangeAsync(int minAge, int maxAge)
            => SearchBySqlAsync(
                "SELECT * FROM Players WHERE Age BETWEEN @Min AND @Max",
                new SqlParameter("@Min", minAge),
                new SqlParameter("@Max", maxAge));

        /// <summary>
        /// Devuelve los top N goleadores.
        /// </summary>
        public async Task<IEnumerable<Player>> GetTopScorersAsync(int topN)
        {
            var entities = await _context.Players
                .FromSqlRaw($"SELECT TOP {topN} * FROM Players ORDER BY Goals DESC")
                .ToListAsync();

            var result = new List<Player>();
            foreach (var e in entities)
            {
                var tps = await _context.TeamPlayers
                    .FromSqlRaw("SELECT * FROM TeamPlayers WHERE PlayerID = @ID",
                        new SqlParameter("@ID", e.PlayerID))
                    .ToListAsync();

                result.Add(_mapper.ToDomain(e, tps));
            }

            return result;
        }

        /// <summary>
        /// Busca jugadores cuyo nombre contenga la cadena dada.
        /// </summary>
        public Task<IEnumerable<Player>> SearchByNameAsync(string partialName)
            => SearchBySqlAsync(
                "SELECT * FROM Players WHERE Name LIKE @Patt",
                new SqlParameter("@Patt", $"%{partialName}%"));

        /// <summary>
        /// Helper para ejecutar consultas SQL y mapear resultados.
        /// </summary>
        private async Task<IEnumerable<Player>> SearchBySqlAsync(string sql, params SqlParameter[] parms)
        {
            var entities = await _context.Players
                .FromSqlRaw(sql, parms)
                .ToListAsync();

            var result = new List<Player>();
            foreach (var e in entities)
            {
                var tps = await _context.TeamPlayers
                    .FromSqlRaw("SELECT * FROM TeamPlayers WHERE PlayerID = @ID",
                        new SqlParameter("@ID", e.PlayerID))
                    .ToListAsync();

                result.Add(_mapper.ToDomain(e, tps));
            }

            return result;
        }
    }
}
