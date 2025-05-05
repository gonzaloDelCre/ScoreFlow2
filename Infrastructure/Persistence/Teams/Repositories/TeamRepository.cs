using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Teams.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(ApplicationDbContext context, ILogger<TeamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            var teamEntities = await _context.Teams.ToListAsync();
            var leagueEntities = await _context.Leagues.ToListAsync();
            var playerEntities = await _context.Players.ToListAsync();
            var teamPlayerEntities = await _context.TeamPlayers.ToListAsync();

            var teams = new List<Team>();

            foreach (var te in teamEntities)
            {
                var leagueEntity = leagueEntities.FirstOrDefault(l => l.LeagueID == te.LeagueID)
                    ?? throw new InvalidOperationException($"No league with ID {te.LeagueID}");
                var league = LeagueMapper.MapToDomainSimple(leagueEntity);

                var tpsForTeam = teamPlayerEntities
                    .Where(tp => tp.TeamID == te.TeamID)
                    .ToList();

                var players = tpsForTeam
                    .Join(playerEntities,
                          tp => tp.PlayerID,
                          p => p.PlayerID,
                          (tp, p) => PlayerMapper.MapToDomain(p, tpsForTeam.Where(x => x.PlayerID == p.PlayerID).ToList()))
                    .ToList();

                var team = TeamMapper.MapToDomain(te, league, tpsForTeam, playerEntities);
                teams.Add(team);
            }

            return teams;
        }

        public async Task<Team?> GetByIdAsync(TeamID teamId)
        {
            try
            {
                var parameter = new SqlParameter("@TeamID", teamId.Value);
                var te = await _context.Teams
                    .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @TeamID", parameter)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (te == null) return null;

                var tps = await _context.TeamPlayers
                    .Where(tp => tp.TeamID == teamId.Value)
                    .ToListAsync();

                var playerIds = tps.Select(tp => tp.PlayerID).Distinct().ToList();
                var players = await _context.Players
                    .Where(p => playerIds.Contains(p.PlayerID))
                    .ToListAsync();

                var leagueEntity = await _context.Leagues.FindAsync(te.LeagueID)
                    ?? throw new InvalidOperationException($"No league {te.LeagueID}");
                var league = LeagueMapper.MapToDomainSimple(leagueEntity);

                return TeamMapper.MapToDomain(te, league, tps, players);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting team {TeamID}", teamId.Value);
                return null;
            }
        }

        public async Task<Team?> GetByExternalIdAsync(string externalId)
        {
            var entity = await _context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ExternalID == externalId);
            if (entity == null) return null;

            var leagueEntity = await _context.Leagues.FindAsync(entity.LeagueID)
                ?? throw new InvalidOperationException($"No league {entity.LeagueID}");
            var league = LeagueMapper.MapToDomainSimple(leagueEntity);

            return new Team(
                new TeamID(entity.TeamID),
                new TeamName(entity.Name),
                league,
                entity.Logo ?? string.Empty,
                entity.CreatedAt,
                entity.ExternalID
            );
        }

        public async Task<Team> AddAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            try
            {
                var teamEntity = TeamMapper.MapToEntity(team);
                var insertSql = @"INSERT INTO Teams (Name, Logo, CreatedAt, ExternalID, Category, Club, Stadium, LeagueID) 
                     VALUES (@Name, @Logo, @CreatedAt, @ExternalID, @Category, @Club, @Stadium, @LeagueID);
                     SELECT SCOPE_IDENTITY();";

                var parameters = new[]
                {
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@CreatedAt", teamEntity.CreatedAt),
                    new SqlParameter("@ExternalID", (object?)teamEntity.ExternalID ?? DBNull.Value),
                    new SqlParameter("@Category", (object?)teamEntity.Category ?? DBNull.Value),
                    new SqlParameter("@Club", (object?)teamEntity.Club ?? DBNull.Value),
                    new SqlParameter("@Stadium", (object?)teamEntity.Stadium ?? DBNull.Value),
                    new SqlParameter("@LeagueID", teamEntity.LeagueID)
                };

                var newId = Convert.ToInt32(await _context.Database.ExecuteSqlRawAsync(insertSql, parameters));
                var created = new Team(
                    new TeamID(newId),
                    team.Name,
                    team.League,
                    team.Logo,
                    team.CreatedAt,
                    team.ExternalID
                );
                created.Update(team.Category, team.Club, team.Stadium);

                if (team.Players.Any())
                {
                    var tpSql = string.Join(", ", team.Players.Select((p, i) => $"(@TeamID, @PlayerID{i})"));
                    var tpParams = new List<SqlParameter> { new SqlParameter("@TeamID", newId) };
                    tpParams.AddRange(team.Players.Select((p, i) => new SqlParameter($"@PlayerID{i}", p.PlayerID.Value)));
                    await _context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO TeamPlayers (TeamID, PlayerID) VALUES " + tpSql,
                        tpParams.ToArray());
                }

                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding team: {Message}", ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            try
            {
                var entity = TeamMapper.MapToEntity(team);
                var updateSql = @"UPDATE Teams SET Name=@Name, Logo=@Logo, ExternalID=@ExternalID,
                                          Category=@Category, Club=@Club, Stadium=@Stadium, LeagueID=@LeagueID
                                   WHERE TeamID=@TeamID";
                var parameters = new[]
                {
                    new SqlParameter("@TeamID", entity.TeamID),
                    new SqlParameter("@Name", entity.Name),
                    new SqlParameter("@Logo", entity.Logo),
                    new SqlParameter("@ExternalID", (object?)entity.ExternalID ?? DBNull.Value),
                    new SqlParameter("@Category", (object?)entity.Category ?? DBNull.Value),
                    new SqlParameter("@Club", (object?)entity.Club ?? DBNull.Value),
                    new SqlParameter("@Stadium", (object?)entity.Stadium ?? DBNull.Value),
                    new SqlParameter("@LeagueID", entity.LeagueID)
                };
                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);

                // Refresh team‐player relations
                var delParam = new SqlParameter("@TeamID", team.TeamID.Value);
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM TeamPlayers WHERE TeamID=@TeamID", delParam);

                if (team.Players.Any())
                {
                    var tpSql = string.Join(", ", team.Players.Select((p, i) => $"(@TeamID, @PlayerID{i})"));
                    var tpParams = new List<SqlParameter> { new SqlParameter("@TeamID", team.TeamID.Value) };
                    tpParams.AddRange(team.Players.Select((p, i) => new SqlParameter($"@PlayerID{i}", p.PlayerID.Value)));
                    await _context.Database.ExecuteSqlRawAsync("INSERT INTO TeamPlayers (TeamID, PlayerID) VALUES " + tpSql, tpParams.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating team {TeamID}: {Message}", team.TeamID.Value, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TeamID teamId)
        {
            try
            {
                var param = new SqlParameter("@TeamID", teamId.Value);
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM TeamPlayers WHERE TeamID=@TeamID", param);
                var rows = await _context.Database.ExecuteSqlRawAsync("DELETE FROM Teams WHERE TeamID=@TeamID", param);
                return rows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting team {TeamID}: {Message}", teamId.Value, ex.Message);
                return false;
            }
        }
    }
}