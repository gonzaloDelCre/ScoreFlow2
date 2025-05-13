using Domain.Entities.Leagues;
using Domain.Entities.Standings;
using Domain.Ports.Standings;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Standings.Mapper;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Persistence.Standings.Repositories
{
    public class StandingRepository : IStandingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StandingRepository> _logger;
        private readonly IStandingMapper _mapper;
        private readonly ITeamMapper _teamMapper;

        public StandingRepository(
            ApplicationDbContext context,
            ILogger<StandingRepository> logger,
            IStandingMapper mapper,
            ITeamMapper teamMapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _teamMapper = teamMapper;
        }

        public async Task<Standing> AddAsync(Standing standing)
        {
            var e = _mapper.MapToEntity(standing);
            const string sql = @"
                INSERT INTO Standings
                  (LeagueID, TeamID, Points, Wins, Draws, Losses, GoalDifference, CreatedAt)
                VALUES
                  (@LeagueID, @TeamID, @Points, @Wins, @Draws, @Losses, @GD, @CreatedAt)";
            var p = new[]
            {
                new SqlParameter("@LeagueID",  e.LeagueID),
                new SqlParameter("@TeamID",    e.TeamID),
                new SqlParameter("@Points",    e.Points),
                new SqlParameter("@Wins",      e.Wins),
                new SqlParameter("@Draws",     e.Draws),
                new SqlParameter("@Losses",    e.Losses),
                new SqlParameter("@GD",        e.GoalDifference),
                new SqlParameter("@CreatedAt", e.CreatedAt)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
            return standing;
        }

        public async Task UpdateAsync(Standing standing)
        {
            var e = _mapper.MapToEntity(standing);
            const string sql = @"
                UPDATE Standings
                SET 
                  Points         = @Points,
                  Wins           = @Wins,
                  Draws          = @Draws,
                  Losses         = @Losses,
                  GoalDifference = @GD
                WHERE ID = @ID";
            var p = new[]
            {
                new SqlParameter("@ID",     e.ID),
                new SqlParameter("@Points", e.Points),
                new SqlParameter("@Wins",   e.Wins),
                new SqlParameter("@Draws",  e.Draws),
                new SqlParameter("@Losses", e.Losses),
                new SqlParameter("@GD",     e.GoalDifference)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task<bool> DeleteAsync(StandingID standingId)
        {
            const string sql = "DELETE FROM Standings WHERE ID = @ID";
            var rows = await _context.Database.ExecuteSqlRawAsync(
                sql,
                new SqlParameter("@ID", standingId.Value));
            return rows > 0;
        }

        public async Task<Standing?> GetByIdAsync(StandingID standingId)
        {
            var entity = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE ID = @ID", new SqlParameter("@ID", standingId.Value))
                .Include(s => s.League)
                .Include(s => s.Team)
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var leagueDomain = new League(
                new LeagueID(entity.LeagueID),
                new LeagueName(entity.League.Name));

            var teamDomain = _teamMapper.ToDomain(
                entity.Team,
                players: Enumerable.Empty<Domain.Entities.Players.Player>(),
                standings: Enumerable.Empty<Standing>());

            return _mapper.MapToDomain(entity, leagueDomain, teamDomain);
        }

        public async Task<IEnumerable<Standing>> GetAllAsync()
        {
            var list = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings")
                .Include(s => s.League)
                .Include(s => s.Team)
                .ToListAsync();

            return list.Select(e =>
            {
                var leagueDomain = new League(
                    new LeagueID(e.LeagueID),
                    new LeagueName(e.League.Name));

                var teamDomain = _teamMapper.ToDomain(
                    e.Team,
                    players: Enumerable.Empty<Domain.Entities.Players.Player>(),
                    standings: Enumerable.Empty<Standing>());

                return _mapper.MapToDomain(e, leagueDomain, teamDomain);
            }).ToList();
        }

        public async Task<IEnumerable<Standing>> GetByLeagueIdAsync(LeagueID leagueId)
        {
            var list = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE LeagueID = @LID", new SqlParameter("@LID", leagueId.Value))
                .Include(s => s.League)
                .Include(s => s.Team)
                .ToListAsync();

            return list.Select(e =>
            {
                var leagueDomain = new League(
                    new LeagueID(e.LeagueID),
                    new LeagueName(e.League.Name));

                var teamDomain = _teamMapper.ToDomain(
                    e.Team,
                    players: Enumerable.Empty<Domain.Entities.Players.Player>(),
                    standings: Enumerable.Empty<Standing>());

                return _mapper.MapToDomain(e, leagueDomain, teamDomain);
            }).ToList();
        }

        public async Task<Standing?> GetByTeamIdAndLeagueIdAsync(TeamID teamId, LeagueID leagueId)
        {
            var all = await GetByLeagueIdAsync(leagueId);
            return all.FirstOrDefault(s => s.TeamID.Equals(teamId));
        }

        public async Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(LeagueID leagueId)
        {
            var list = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE LeagueID = @LID ORDER BY Points DESC", new SqlParameter("@LID", leagueId.Value))
                .Include(s => s.League)
                .Include(s => s.Team)
                .ToListAsync();

            return list.Select(e =>
            {
                var leagueDomain = new League(
                    new LeagueID(e.LeagueID),
                    new LeagueName(e.League.Name));

                var teamDomain = _teamMapper.ToDomain(
                    e.Team,
                    players: Enumerable.Empty<Domain.Entities.Players.Player>(),
                    standings: Enumerable.Empty<Standing>());

                return _mapper.MapToDomain(e, leagueDomain, teamDomain);
            }).ToList();
        }

        public async Task<IEnumerable<Standing>> GetTopByPointsAsync(int topN)
        {
            var list = await _context.Standings
                .FromSqlRaw($"SELECT TOP {topN} * FROM Standings ORDER BY Points DESC")
                .Include(s => s.League)
                .Include(s => s.Team)
                .ToListAsync();

            return list.Select(e =>
            {
                var leagueDomain = new League(
                    new LeagueID(e.LeagueID),
                    new LeagueName(e.League.Name));

                var teamDomain = _teamMapper.ToDomain(
                    e.Team,
                    players: Enumerable.Empty<Domain.Entities.Players.Player>(),
                    standings: Enumerable.Empty<Standing>());

                return _mapper.MapToDomain(e, leagueDomain, teamDomain);
            }).ToList();
        }

        public async Task<IEnumerable<Standing>> GetByGoalDifferenceRangeAsync(int minGD, int maxGD)
        {
            var list = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE GoalDifference BETWEEN @Min AND @Max",
                    new SqlParameter("@Min", minGD),
                    new SqlParameter("@Max", maxGD))
                .Include(s => s.League)
                .Include(s => s.Team)
                .ToListAsync();

            return list.Select(e =>
            {
                var leagueDomain = new League(
                    new LeagueID(e.LeagueID),
                    new LeagueName(e.League.Name));

                var teamDomain = _teamMapper.ToDomain(
                    e.Team,
                    players: Enumerable.Empty<Domain.Entities.Players.Player>(),
                    standings: Enumerable.Empty<Standing>());

                return _mapper.MapToDomain(e, leagueDomain, teamDomain);
            }).ToList();
        }
    }
}
