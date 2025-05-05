using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Standings;
using Domain.Ports.Standings;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.Standings.Entities;
using Infrastructure.Persistence.Standings.Mapper;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Standings.Repositories
{
    public class StandingRepository : IStandingRepository
    {
        private readonly ApplicationDbContext _context;

        public StandingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Standing>> GetAllAsync()
        {
            var sql = "SELECT * FROM Standings";
            var dbStandings = await _context.Standings
                .FromSqlRaw(sql)
                .AsNoTracking()  
                .ToListAsync();

            var leagues = await _context.Leagues.ToListAsync();
            var teams = await _context.Teams.ToListAsync();
            var teamPlayers = await _context.TeamPlayers.ToListAsync();
            var players = await _context.Players.ToListAsync();

            return dbStandings.Select(e =>
            {
                var leagueEntity = leagues.First(l => l.LeagueID == e.LeagueID);
                var teamEntity = teams.First(t => t.TeamID == e.TeamID);

                var league = LeagueMapper.MapToDomainSimple(leagueEntity);
                var team = TeamMapper.MapToDomain(
                    teamEntity,
                    league,
                    teamPlayers.Where(tp => tp.TeamID == e.TeamID).ToList(),
                    players
                );

                return e.ToDomain(
                    league,
                    teamPlayers.Where(tp => tp.TeamID == e.TeamID).ToList(),
                    players
                );
            }).ToList();
        }

        public async Task<Standing?> GetByIdAsync(StandingID standingId)
        {
            var param = new SqlParameter("@id", standingId.Value);
            var entity = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE StandingID = @id", param)
                .FirstOrDefaultAsync();

            if (entity == null) return null;

            var leagueEntity = await _context.Leagues.FindAsync(entity.LeagueID)
                ?? throw new InvalidOperationException($"League {entity.LeagueID} not found.");
            var league = LeagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers
                .Where(tp => tp.TeamID == entity.TeamID)
                .ToListAsync();
            var players = await _context.Players.ToListAsync();

            return entity.ToDomain(league, teamPlayers, players);
        }

        public async Task<IEnumerable<Standing>> GetByLeagueIdAsync(LeagueID leagueId)
        {
            var param = new SqlParameter("@lid", leagueId.Value);
            var dbStandings = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE LeagueID = @lid", param)
                .ToListAsync();

            var leagueEntity = await _context.Leagues.FindAsync(leagueId.Value)
                ?? throw new InvalidOperationException($"League {leagueId.Value} not found.");
            var league = LeagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers.ToListAsync();
            var players = await _context.Players.ToListAsync();

            return dbStandings.Select(e =>
                e.ToDomain(
                    league,
                    teamPlayers.Where(tp => tp.TeamID == e.TeamID).ToList(),
                    players
                )
            ).ToList();
        }

        public async Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(LeagueID leagueId)
        {
            var param = new SqlParameter("@lid", leagueId.Value);
            var dbStandings = await _context.Standings
                .FromSqlRaw(@"
                    SELECT *
                    FROM Standings
                    WHERE LeagueID = @lid
                    ORDER BY Points DESC, (GoalsFor - GoalsAgainst) DESC", param)
                .ToListAsync();

            var leagueEntity = await _context.Leagues.FindAsync(leagueId.Value)
                ?? throw new InvalidOperationException($"League {leagueId.Value} not found.");
            var league = LeagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers.ToListAsync();
            var players = await _context.Players.ToListAsync();

            return dbStandings.Select(e =>
                e.ToDomain(
                    league,
                    teamPlayers.Where(tp => tp.TeamID == e.TeamID).ToList(),
                    players
                )
            ).ToList();
        }

        public async Task<Standing?> GetByTeamIdAndLeagueIdAsync(TeamID teamId, LeagueID leagueId)
        {
            var parameters = new[]
            {
                new SqlParameter("@tid", teamId.Value),
                new SqlParameter("@lid", leagueId.Value)
            };

            var entity = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE TeamID = @tid AND LeagueID = @lid", parameters)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            var leagueEntity = await _context.Leagues.FindAsync(leagueId.Value)
                ?? throw new InvalidOperationException($"League {leagueId.Value} not found.");
            var league = LeagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers
                .Where(tp => tp.TeamID == teamId.Value)
                .ToListAsync();
            var players = await _context.Players.ToListAsync();

            return entity.ToDomain(league, teamPlayers, players);
        }

        public async Task<Standing> AddAsync(Standing standing)
        {
            var insertSql = @"
                INSERT INTO Standings
                    (LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt)
                VALUES
                    (@lid, @tid, @w, @d, @l, @gf, @ga, @p, @ca);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            var parameters = new[]
            {
                new SqlParameter("@lid", standing.LeagueID.Value),
                new SqlParameter("@tid", standing.TeamID.Value),
                new SqlParameter("@w",   standing.Wins.Value),
                new SqlParameter("@d",   standing.Draws.Value),
                new SqlParameter("@l",   standing.Losses.Value),
                new SqlParameter("@gf",  standing.GoalsFor.Value),
                new SqlParameter("@ga",  standing.GoalsAgainst.Value),
                new SqlParameter("@p",   standing.Points.Value),
                new SqlParameter("@ca",  standing.CreatedAt)
            };

            var newId = Convert.ToInt32(await _context.Database.ExecuteSqlRawAsync(insertSql, parameters));
            var entity = await _context.Standings.FindAsync(newId)
                         ?? throw new InvalidOperationException("Inserted standing not found.");

            var leagueEntity = await _context.Leagues.FindAsync(entity.LeagueID)
                ?? throw new InvalidOperationException($"League {entity.LeagueID} not found.");
            var league = LeagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers
                .Where(tp => tp.TeamID == entity.TeamID)
                .ToListAsync();
            var players = await _context.Players.ToListAsync();

            return entity.ToDomain(league, teamPlayers, players);
        }

        public async Task UpdateAsync(Standing standing)
        {
            var sql = @"
                UPDATE Standings
                SET 
                    LeagueID     = @lid,
                    TeamID       = @tid,
                    Wins         = @w,
                    Draws        = @d,
                    Losses       = @l,
                    GoalsFor     = @gf,
                    GoalsAgainst = @ga,
                    Points       = @p
                WHERE StandingID = @id;
            ";

            var parameters = new[]
            {
                new SqlParameter("@id",  standing.StandingID.Value),
                new SqlParameter("@lid", standing.LeagueID.Value),
                new SqlParameter("@tid", standing.TeamID.Value),
                new SqlParameter("@w",   standing.Wins.Value),
                new SqlParameter("@d",   standing.Draws.Value),
                new SqlParameter("@l",   standing.Losses.Value),
                new SqlParameter("@gf",  standing.GoalsFor.Value),
                new SqlParameter("@ga",  standing.GoalsAgainst.Value),
                new SqlParameter("@p",   standing.Points.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<bool> DeleteAsync(StandingID standingId)
        {
            var param = new SqlParameter("@id", standingId.Value);
            var affected = await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Standings WHERE StandingID = @id", param);
            return affected > 0;
        }
    }
}
