using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Domain.Ports.Players;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Players.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerRepository> _logger;

        public PlayerRepository(ApplicationDbContext context, ILogger<PlayerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            try
            {
                var dbPlayers = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();
                var teamPlayers = await _context.TeamPlayers.ToListAsync();

                return dbPlayers
                    .Select(p => PlayerMapper.MapToDomain(p,
                        teamPlayers.Where(tp => tp.PlayerID == p.PlayerID).ToList()))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de jugadores");
                return new List<Player>();
            }
        }

        public async Task<Player?> GetByIdAsync(PlayerID playerId)
        {
            try
            {
                var param = new SqlParameter("@PlayerID", playerId.Value);
                var dbPlayer = await _context.Players
                    .FromSqlRaw("SELECT * FROM Players WHERE PlayerID = @PlayerID", param)
                    .FirstOrDefaultAsync();
                if (dbPlayer == null) return null;

                var teamPlayers = await _context.TeamPlayers
                    .Where(tp => tp.PlayerID == dbPlayer.PlayerID)
                    .ToListAsync();

                return PlayerMapper.MapToDomain(dbPlayer, teamPlayers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con ID {PlayerID}", playerId.Value);
                return null;
            }
        }

        public async Task<Player?> GetByNameAsync(string playerName)
        {
            try
            {
                var param = new SqlParameter("@Name", playerName);
                var dbPlayer = await _context.Players
                    .FromSqlRaw("SELECT * FROM Players WHERE Name = @Name", param)
                    .FirstOrDefaultAsync();
                if (dbPlayer == null) return null;

                var teamPlayers = await _context.TeamPlayers
                    .Where(tp => tp.PlayerID == dbPlayer.PlayerID)
                    .ToListAsync();

                return PlayerMapper.MapToDomain(dbPlayer, teamPlayers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el jugador con Nombre {PlayerName}", playerName);
                return null;
            }
        }

        public async Task<IEnumerable<Player>> GetByTeamIdAsync(TeamID teamId)
        {
            try
            {
                // Primero obtenemos los PlayerID asociados
                var sql = "SELECT PlayerID FROM TeamPlayers WHERE TeamID = @TeamID";
                var param = new SqlParameter("@TeamID", teamId.Value);
                var playerIds = await _context.TeamPlayers
                    .FromSqlRaw(sql, param)
                    .Select(tp => tp.PlayerID)
                    .ToListAsync();

                // Luego cargamos esos jugadores
                var idsCsv = string.Join(",", playerIds);
                var dbPlayers = await _context.Players
                    .FromSqlRaw($"SELECT * FROM Players WHERE PlayerID IN ({idsCsv})")
                    .ToListAsync();

                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();

                return dbPlayers
                    .Select(p => PlayerMapper.MapToDomain(p,
                        allTeamPlayers.Where(tp => tp.PlayerID == p.PlayerID).ToList()))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los jugadores del equipo con ID {TeamID}", teamId.Value);
                return new List<Player>();
            }
        }

        public async Task<Player> AddAsync(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            var pe = PlayerMapper.MapToEntity(player);
            var sql = @"
                INSERT INTO Players (Name, Position, Age, Goals, Photo, CreatedAt)
                VALUES (@Name, @Position, @Age, @Goals, @Photo, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();
            int newId;
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@Name", pe.Name));
                cmd.Parameters.Add(new SqlParameter("@Position", pe.Position));
                cmd.Parameters.Add(new SqlParameter("@Age", pe.Age));
                cmd.Parameters.Add(new SqlParameter("@Goals", pe.Goals));
                cmd.Parameters.Add(new SqlParameter("@Photo", (object?)pe.Photo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@CreatedAt", pe.CreatedAt));
                newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            finally
            {
                await conn.CloseAsync();
            }

            var dbPlayer = await _context.Players
                .FromSqlRaw("SELECT * FROM Players WHERE PlayerID = {0}", newId)
                .AsNoTracking()
                .FirstAsync();

            // No hay relaciones aún
            return PlayerMapper.MapToDomain(dbPlayer, new List<TeamPlayerEntity>());
        }

        public async Task UpdateAsync(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            var pe = PlayerMapper.MapToEntity(player);
            var sql = @"
                UPDATE Players
                SET Name = @Name, Position = @Position, Age = @Age, Goals = @Goals, Photo = @Photo, CreatedAt = @CreatedAt
                WHERE PlayerID = @PlayerID
            ";

            var parameters = new[]
            {
                new SqlParameter("@PlayerID",  pe.PlayerID),
                new SqlParameter("@Name",      pe.Name),
                new SqlParameter("@Position",  pe.Position),
                new SqlParameter("@Age",       pe.Age),
                new SqlParameter("@Goals",     pe.Goals),
                new SqlParameter("@Photo",     (object?)pe.Photo ?? DBNull.Value),
                new SqlParameter("@CreatedAt", pe.CreatedAt)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<bool> DeleteAsync(PlayerID playerId)
        {
            try
            {
                var param = new SqlParameter("@PlayerID", playerId.Value);
                var rows = await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Players WHERE PlayerID = @PlayerID", param);
                return rows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el jugador con ID {PlayerID}", playerId.Value);
                return false;
            }
        }
    }
}
