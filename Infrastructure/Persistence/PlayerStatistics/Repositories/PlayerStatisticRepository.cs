using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.PlayerStatistics.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Domain.Shared;
using System;
using Infrastructure.Persistence.Matches.Mapper;
using Infrastructure.Persistence.Players.Mapper;

namespace Infrastructure.Persistence.PlayerStatistics.Repositories
{
    public class PlayerStatisticRepository : IPlayerStatisticRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerStatisticRepository> _logger;
        private readonly IPlayerStatisticMapper _statMapper;
        private readonly IPlayerMapper _playerMapper;
        private readonly IMatchMapper _matchMapper;

        public PlayerStatisticRepository(
            ApplicationDbContext context,
            ILogger<PlayerStatisticRepository> logger,
            IPlayerStatisticMapper statMapper,
            IPlayerMapper playerMapper,
            IMatchMapper matchMapper)
        {
            _context = context;
            _logger = logger;
            _statMapper = statMapper;
            _playerMapper = playerMapper;
            _matchMapper = matchMapper;
        }

        public async Task<PlayerStatistic> AddAsync(PlayerStatistic stat)
        {
            var e = _statMapper.MapToEntity(stat);
            const string sql = @"
                INSERT INTO PlayerStatistics
                  (ID, PlayerID, MatchID, Goals, Assists, YellowCards, RedCards, MinutesPlayed, CreatedAt)
                VALUES
                  (@ID, @PlayerID, @MatchID, @Goals, @Assists, @YC, @RC, @Min, @CreatedAt)";
            var p = new[]
            {
                new SqlParameter("@ID",         e.ID),
                new SqlParameter("@PlayerID",   e.PlayerID),
                new SqlParameter("@MatchID",    e.MatchID),
                new SqlParameter("@Goals",      e.Goals),
                new SqlParameter("@Assists",    e.Assists),
                new SqlParameter("@YC",         e.YellowCards),
                new SqlParameter("@RC",         e.RedCards),
                new SqlParameter("@Min",        e.MinutesPlayed),
                new SqlParameter("@CreatedAt",  e.CreatedAt)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
            return stat;
        }

        public async Task UpdateAsync(PlayerStatistic stat)
        {
            var e = _statMapper.MapToEntity(stat);
            const string sql = @"
                UPDATE PlayerStatistics
                SET
                  Goals        = @Goals,
                  Assists      = @Assists,
                  YellowCards  = @YC,
                  RedCards     = @RC,
                  MinutesPlayed= @Min
                WHERE ID = @ID";
            var p = new[]
            {
                new SqlParameter("@ID",    e.ID),
                new SqlParameter("@Goals", e.Goals),
                new SqlParameter("@Assists", e.Assists),
                new SqlParameter("@YC",    e.YellowCards),
                new SqlParameter("@RC",    e.RedCards),
                new SqlParameter("@Min",   e.MinutesPlayed)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task<bool> DeleteAsync(PlayerStatisticID statId)
        {
            const string sql = "DELETE FROM PlayerStatistics WHERE ID = @ID";
            var rows = await _context.Database.ExecuteSqlRawAsync(
                sql,
                new SqlParameter("@ID", statId.Value));
            return rows > 0;
        }

        public async Task<PlayerStatistic?> GetByIdAsync(PlayerStatisticID statId)
        {
            var entity = await _context.PlayerStatistics
                .FromSqlRaw("SELECT * FROM PlayerStatistics WHERE ID = @ID", new SqlParameter("@ID", statId.Value))
                .Include(ps => ps.Player)
                .Include(ps => ps.Match)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            var playerDomain = _playerMapper.ToDomain(
                entity.Player,
                await _context.TeamPlayers
                    .Where(tp => tp.PlayerID == entity.PlayerID)
                    .ToListAsync()
            );

            var matchDomain = _matchMapper.ToDomain(entity.Match);

            return _statMapper.MapToDomain(entity, playerDomain, matchDomain);
        }

        public async Task<IEnumerable<PlayerStatistic>> GetAllAsync()
        {
            var list = await _context.PlayerStatistics
                .FromSqlRaw("SELECT * FROM PlayerStatistics")
                .Include(ps => ps.Player)
                .Include(ps => ps.Match)
                .ToListAsync();

            return list.Select(entity =>
            {
                var playerDomain = _playerMapper.ToDomain(
                    entity.Player,
                    _context.TeamPlayers
                        .Where(tp => tp.PlayerID == entity.PlayerID)
                        .ToList()
                );

                var matchDomain = _matchMapper.ToDomain(entity.Match);

                return _statMapper.MapToDomain(entity, playerDomain, matchDomain);
            }).ToList();
        }

        public Task<IEnumerable<PlayerStatistic>> GetByPlayerIdAsync(Domain.Shared.PlayerID playerId)
            => FilterBySqlAsync(
                "SELECT * FROM PlayerStatistics WHERE PlayerID = @PID",
                new SqlParameter("@PID", playerId.Value));

        public Task<IEnumerable<PlayerStatistic>> GetByMatchIdAsync(Domain.Shared.MatchID matchId)
            => FilterBySqlAsync(
                "SELECT * FROM PlayerStatistics WHERE MatchID = @MID",
                new SqlParameter("@MID", matchId.Value));

        public Task<IEnumerable<PlayerStatistic>> GetByGoalsRangeAsync(int minGoals, int maxGoals)
            => FilterBySqlAsync(
                "SELECT * FROM PlayerStatistics WHERE Goals BETWEEN @Min AND @Max",
                new SqlParameter("@Min", minGoals),
                new SqlParameter("@Max", maxGoals));

        public Task<IEnumerable<PlayerStatistic>> GetByAssistsRangeAsync(int minAssists, int maxAssists)
            => FilterBySqlAsync(
                "SELECT * FROM PlayerStatistics WHERE Assists BETWEEN @Min AND @Max",
                new SqlParameter("@Min", minAssists),
                new SqlParameter("@Max", maxAssists));

        private async Task<IEnumerable<PlayerStatistic>> FilterBySqlAsync(string sql, params SqlParameter[] ps)
        {
            var list = await _context.PlayerStatistics
                .FromSqlRaw(sql, ps)
                .Include(ps => ps.Player)
                .Include(ps => ps.Match)
                .ToListAsync();

            var result = new List<PlayerStatistic>();
            foreach (var entity in list)
            {
                var playerDomain = _playerMapper.ToDomain(
                    entity.Player,
                    await _context.TeamPlayers
                        .Where(tp => tp.PlayerID == entity.PlayerID)
                        .ToListAsync()
                );

                var matchDomain = _matchMapper.ToDomain(entity.Match);

                result.Add(_statMapper.MapToDomain(entity, playerDomain, matchDomain));
            }

            return result;
        }
    }
}
