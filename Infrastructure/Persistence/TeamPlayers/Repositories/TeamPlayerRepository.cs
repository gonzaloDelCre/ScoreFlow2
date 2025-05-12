using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Domain.Ports.TeamPlayers;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.TeamPlayers.Mappers;
using Infrastructure.Persistence.TeamPlayers.Mapper;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.TeamPlayers.Repositories
{
    public class TeamPlayerRepository : ITeamPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamPlayerRepository> _logger;
        private readonly ITeamPlayerMapper _mapper;

        public TeamPlayerRepository(
            ApplicationDbContext context,
            ILogger<TeamPlayerRepository> logger,
            ITeamPlayerMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<TeamPlayer> AddAsync(TeamPlayer tp)
        {
            // 1) Mapea dominio → entidad
            var e = _mapper.MapToEntity(tp);

            // 2) Crea el comando
            var conn = _context.Database.GetDbConnection();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        INSERT INTO TeamPlayers (TeamID, PlayerID, RoleInTeam, JoinedAt)
        VALUES (@TeamID, @PlayerID, @Role, @JoinedAt);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";
            cmd.Parameters.Add(new SqlParameter("@TeamID", e.TeamID));
            cmd.Parameters.Add(new SqlParameter("@PlayerID", e.PlayerID));
            cmd.Parameters.Add(new SqlParameter("@Role", (object?)e.RoleInTeam ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@JoinedAt", e.JoinedAt));

            // 3) Abre la conexión si no lo está
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            // 4) Ejecuta y recupera el nuevo ID
            var result = await cmd.ExecuteScalarAsync();
            var newId = Convert.ToInt32(result);

            // 5) Devuelve dominio con el ID real
            return tp.WithId(new TeamPlayerID(newId));
        }


        public async Task UpdateAsync(TeamPlayer tp)
        {
            var e = _mapper.MapToEntity(tp);
            const string sql = @"
                UPDATE TeamPlayers
                SET
                  RoleInTeam = @Role,
                  JoinedAt   = @Joined
                WHERE ID = @ID";
            var p = new[]
            {
                new SqlParameter("@Role",   e.RoleInTeam),
                new SqlParameter("@Joined", e.JoinedAt),
                new SqlParameter("@ID",     e.ID)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId)
        {
            const string sql = @"
                DELETE FROM TeamPlayers
                WHERE TeamID = @TID AND PlayerID = @PID";
            var rows = await _context.Database.ExecuteSqlRawAsync(sql, new[]
            {
                new SqlParameter("@TID", teamId.Value),
                new SqlParameter("@PID", playerId.Value)
            });
            return rows > 0;
        }

        public async Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId)
        {
            var e = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @TID AND PlayerID = @PID",
                    new SqlParameter("@TID", teamId.Value),
                    new SqlParameter("@PID", playerId.Value))
                .Include(tp => tp.Player)
                .Include(tp => tp.Team)
                .FirstOrDefaultAsync();

            return e == null
                ? null
                : _mapper.MapToDomain(e);
        }

        public async Task<IEnumerable<TeamPlayer>> GetAllAsync()
        {
            var list = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers")
                .Include(tp => tp.Player)
                .Include(tp => tp.Team)
                .ToListAsync();

            return list.Select(e => _mapper.MapToDomain(e)).ToList();
        }

        public async Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId)
        {
            var list = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @TID",
                    new SqlParameter("@TID", teamId.Value))
                .Include(tp => tp.Player)
                .Include(tp => tp.Team)
                .ToListAsync();

            return list.Select(e => _mapper.MapToDomain(e)).ToList();
        }

        public async Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId)
        {
            var list = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE PlayerID = @PID",
                    new SqlParameter("@PID", playerId.Value))
                .Include(tp => tp.Player)
                .Include(tp => tp.Team)
                .ToListAsync();

            return list.Select(e => _mapper.MapToDomain(e)).ToList();
        }

        public Task<IEnumerable<TeamPlayer>> GetByRoleAsync(RoleInTeam role)
            => FilterAsync("SELECT * FROM TeamPlayers WHERE RoleInTeam = @Role",
                new SqlParameter("@Role", role));

        public Task<IEnumerable<TeamPlayer>> GetByJoinDateRangeAsync(DateTime from, DateTime to)
            => FilterAsync("SELECT * FROM TeamPlayers WHERE JoinedAt BETWEEN @From AND @To",
                new SqlParameter("@From", from),
                new SqlParameter("@To", to));

        private async Task<IEnumerable<TeamPlayer>> FilterAsync(string sql, params SqlParameter[] ps)
        {
            var list = await _context.TeamPlayers
                .FromSqlRaw(sql, ps)
                .Include(tp => tp.Player)
                .Include(tp => tp.Team)
                .ToListAsync();

            return list.Select(e => _mapper.MapToDomain(e)).ToList();
        }
    }
}
