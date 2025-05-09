using Domain.Entities.Leagues;
using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Standings.Mapper;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Teams.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamRepository> _logger;
        private readonly ITeamMapper _mapper;
        private readonly IStandingMapper _standingMapper;    

        public TeamRepository(
            ApplicationDbContext context,
            ILogger<TeamRepository> logger,
            ITeamMapper mapper,
            IStandingMapper standingMapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _standingMapper = standingMapper;
        }

        public async Task<Team> AddAsync(Team team)
        {
            var e = _mapper.ToEntity(team);
            const string sql = @"
                INSERT INTO Teams
                  (ExternalID, Name, Category, Club, Stadium, Logo, CoachPlayerID, CreatedAt)
                VALUES
                  (@ExtID, @Name, @Cat, @Club, @Stad, @Logo, @Coach, @CreatedAt)";
            var p = new[]
            {
                new SqlParameter("@ExtID",     e.ExternalID),
                new SqlParameter("@Name",      e.Name),
                new SqlParameter("@Cat",       e.Category),
                new SqlParameter("@Club",      e.Club),
                new SqlParameter("@Stad",      e.Stadium),
                new SqlParameter("@Logo",      e.Logo),
                new SqlParameter("@Coach",     (object?)e.CoachPlayerID ?? DBNull.Value),
                new SqlParameter("@CreatedAt", e.CreatedAt)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);

            var newId = await _context.Teams
                .FromSqlRaw("SELECT TOP 1 TeamID FROM Teams ORDER BY TeamID DESC")
                .Select(t => t.TeamID)
                .FirstAsync();

            var inserted = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @ID", new SqlParameter("@ID", newId))
                .Include(t => t.Coach)
                .FirstAsync();

            return _mapper.ToDomain(inserted,
                players: Enumerable.Empty<Player>(),
                standings: Enumerable.Empty<Standing>());
        }

        public async Task UpdateAsync(Team team)
        {
            var e = _mapper.ToEntity(team);
            const string sql = @"
                UPDATE Teams SET
                  ExternalID    = @ExtID,
                  Name          = @Name,
                  Category      = @Cat,
                  Club          = @Club,
                  Stadium       = @Stad,
                  Logo          = @Logo,
                  CoachPlayerID = @Coach
                WHERE TeamID = @ID";
            var p = new[]
            {
                new SqlParameter("@ExtID",     e.ExternalID),
                new SqlParameter("@Name",      e.Name),
                new SqlParameter("@Cat",       e.Category),
                new SqlParameter("@Club",      e.Club),
                new SqlParameter("@Stad",      e.Stadium),
                new SqlParameter("@Logo",      e.Logo),
                new SqlParameter("@Coach",     (object?)e.CoachPlayerID ?? DBNull.Value),
                new SqlParameter("@ID",        e.TeamID)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, p);
        }

        public async Task<bool> DeleteAsync(TeamID teamId)
        {
            const string sql = "DELETE FROM Teams WHERE TeamID = @ID";
            var rows = await _context.Database.ExecuteSqlRawAsync(
                sql,
                new SqlParameter("@ID", teamId.Value));
            return rows > 0;
        }

        public async Task<Team?> GetByIdAsync(TeamID teamId)
        {
            var e = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @ID", new SqlParameter("@ID", teamId.Value))
                .Include(t => t.Coach)
                .FirstOrDefaultAsync();

            if (e == null) return null;

            return _mapper.ToDomain(e,
                players: await GetPlayersAsync(teamId),
                standings: await GetStandingsAsync(teamId));
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            var list = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams")
                .Include(t => t.Coach)
                .ToListAsync();

            var result = new List<Team>();
            foreach (var e in list)
            {
                var id = new TeamID(e.TeamID);
                result.Add(_mapper.ToDomain(e,
                    players: await GetPlayersAsync(id),
                    standings: await GetStandingsAsync(id)));
            }
            return result;
        }

        public async Task<Team?> GetByExternalIdAsync(string externalId)
        {
            var e = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE ExternalID = @ExtID", new SqlParameter("@ExtID", externalId))
                .Include(t => t.Coach)
                .FirstOrDefaultAsync();
            if (e == null) return null;

            var id = new TeamID(e.TeamID);
            return _mapper.ToDomain(e,
                players: await GetPlayersAsync(id),
                standings: await GetStandingsAsync(id));
        }

        public async Task<IEnumerable<Team>> GetByCategoryAsync(string category)
            => (await GetAllAsync()).Where(t => t.Category == category);

        public async Task<IEnumerable<Team>> SearchByNameAsync(string partialName)
        {
            var pattern = $"%{partialName}%";
            var list = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE Name LIKE @P", new SqlParameter("@P", pattern))
                .Include(t => t.Coach)
                .ToListAsync();

            var result = new List<Team>();
            foreach (var e in list)
            {
                var id = new TeamID(e.TeamID);
                result.Add(_mapper.ToDomain(e,
                    players: await GetPlayersAsync(id),
                    standings: await GetStandingsAsync(id)));
            }
            return result;
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(TeamID teamId)
        {
            var tps = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @ID", new SqlParameter("@ID", teamId.Value))
                .Include(tp => tp.Player)
                .ToListAsync();

            return tps.Select(tp =>
                new Infrastructure.Persistence.Players.Mapper.PlayerMapper()
                    .ToDomain(tp.Player, tps))
                .ToList();
        }

        public async Task<IEnumerable<Standing>> GetStandingsAsync(TeamID teamId)
        {
            var sList = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE TeamID = @ID",
                    new SqlParameter("@ID", teamId.Value))
                .Include(s => s.League)
                .Include(s => s.Team)
                .ToListAsync();

            return sList.Select(e =>
            {
                var leagueDomain = new League(
                    new LeagueID(e.LeagueID),
                    new LeagueName(e.League.Name));

                var teamDomain = _mapper.ToDomain(                
                    e.Team,
                    players: Enumerable.Empty<Player>(),
                    standings: Enumerable.Empty<Standing>());

                return _standingMapper.MapToDomain(e, leagueDomain, teamDomain);
            })
            .ToList();
        }

        public Task<IEnumerable<Team>> GetByClubAsync(string club)
            => Task.FromResult(GetAllAsync().Result.Where(t => t.Club == club));

        public Task<IEnumerable<Team>> GetByStadiumAsync(string stadium)
            => Task.FromResult(GetAllAsync().Result.Where(t => t.Stadium == stadium));
    }
}