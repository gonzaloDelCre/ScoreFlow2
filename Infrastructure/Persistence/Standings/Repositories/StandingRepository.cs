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
        private readonly LeagueMapper _leagueMapper;
        private readonly TeamMapper _teamMapper;

        public StandingRepository(
            ApplicationDbContext context,
            LeagueMapper leagueMapper,
            TeamMapper teamMapper)
        {
            _context = context;
            _leagueMapper = leagueMapper;
            _teamMapper = teamMapper;
        }

        public async Task<IEnumerable<Standing>> GetAllAsync()
        {
            var sql = "SELECT StandingID, LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt FROM Standings";
            var dbStandings = await _context.Standings
                .FromSqlRaw(sql)
                .ToListAsync();

            // Cargamos ligas, equipos, teamPlayers y players
            var leagues = await _context.Leagues.FromSqlRaw("SELECT * FROM Leagues").ToListAsync();
            var teams = await _context.Teams.FromSqlRaw("SELECT * FROM Teams").ToListAsync();
            var teamPlayers = await _context.TeamPlayers.FromSqlRaw("SELECT * FROM TeamPlayers").ToListAsync();
            var players = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();

            return dbStandings.Select(e =>
            {
                var leagueEntity = leagues.FirstOrDefault(l => l.LeagueID == e.LeagueID)
                    ?? throw new InvalidOperationException($"League {e.LeagueID} not found.");
                var teamEntity = teams.FirstOrDefault(t => t.TeamID == e.TeamID)
                    ?? throw new InvalidOperationException($"Team {e.TeamID} not found.");

                var league = _leagueMapper.MapToDomainSimple(leagueEntity);
                var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);

                return e.ToDomain(league, teamPlayers, players);
            }).ToList();
        }

        public async Task<Standing?> GetByIdAsync(StandingID standingId)
        {
            var sql = "SELECT StandingID, LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt FROM Standings WHERE StandingID = @id";
            var param = new SqlParameter("@id", standingId.Value);

            var entity = await _context.Standings
                .FromSqlRaw(sql, param)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            // Cargar liga y equipo
            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", new SqlParameter("@lid", entity.LeagueID))
                .FirstOrDefaultAsync()
                ?? throw new InvalidOperationException($"League {entity.LeagueID} not found.");

            var teamEntity = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .FirstOrDefaultAsync()
                ?? throw new InvalidOperationException($"Team {entity.TeamID} not found.");

            var teamPlayers = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .ToListAsync();

            var players = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            return entity.ToDomain(league, teamPlayers, players);
        }

        public async Task<IEnumerable<Standing>> GetByLeagueIdAsync(LeagueID leagueId)
        {
            var sql = "SELECT StandingID, LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt FROM Standings WHERE LeagueID = @lid";
            var param = new SqlParameter("@lid", leagueId.Value);

            var dbStandings = await _context.Standings
                .FromSqlRaw(sql, param)
                .ToListAsync();

            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", param)
                .FirstOrDefaultAsync()
                ?? throw new InvalidOperationException($"League {leagueId.Value} not found.");

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            var teams = await _context.Teams.FromSqlRaw("SELECT * FROM Teams").ToListAsync();
            var teamPlayers = await _context.TeamPlayers.FromSqlRaw("SELECT * FROM TeamPlayers").ToListAsync();
            var players = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();

            return dbStandings.Select(e =>
            {
                var teamEntity = teams.FirstOrDefault(t => t.TeamID == e.TeamID)
                    ?? throw new InvalidOperationException($"Team {e.TeamID} not found.");
                return e.ToDomain(league, teamPlayers, players);
            }).ToList();
        }

        public async Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(LeagueID leagueId)
        {
            var sql = @"
                SELECT StandingID, LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt
                FROM Standings
                WHERE LeagueID = @lid
                ORDER BY Points DESC, (GoalsFor - GoalsAgainst) DESC";
            var param = new SqlParameter("@lid", leagueId.Value);

            var dbStandings = await _context.Standings
                .FromSqlRaw(sql, param)
                .ToListAsync();

            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", param)
                .FirstOrDefaultAsync()
                ?? throw new InvalidOperationException($"League {leagueId.Value} not found.");

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            var teams = await _context.Teams.FromSqlRaw("SELECT * FROM Teams").ToListAsync();
            var teamPlayers = await _context.TeamPlayers.FromSqlRaw("SELECT * FROM TeamPlayers").ToListAsync();
            var players = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();

            return dbStandings.Select(e =>
            {
                var teamEntity = teams.FirstOrDefault(t => t.TeamID == e.TeamID)
                    ?? throw new InvalidOperationException($"Team {e.TeamID} not found.");
                return e.ToDomain(league, teamPlayers, players);
            }).ToList();
        }

        public async Task<Standing?> GetByTeamIdAndLeagueIdAsync(TeamID teamId, LeagueID leagueId)
        {
            var sql = "SELECT StandingID, LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt FROM Standings WHERE TeamID = @tid AND LeagueID = @lid";
            var parameters = new[]
            {
                new SqlParameter("@tid", teamId.Value),
                new SqlParameter("@lid", leagueId.Value)
            };

            var entity = await _context.Standings
                .FromSqlRaw(sql, parameters)
                .FirstOrDefaultAsync();
            if (entity == null) return null;

            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", parameters[1])
                .FirstOrDefaultAsync()
                ?? throw new InvalidOperationException($"League {leagueId.Value} not found.");

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @tid", parameters[0])
                .ToListAsync();

            var players = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

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
                new SqlParameter("@w",  standing.Wins.Value),
                new SqlParameter("@d",  standing.Draws.Value),
                new SqlParameter("@l",  standing.Losses.Value),
                new SqlParameter("@gf",  standing.GoalsFor.Value),
                new SqlParameter("@ga",  standing.GoalsAgainst.Value),
                new SqlParameter("@p",  standing.Points.Value),
                new SqlParameter("@ca",  standing.CreatedAt)
            };

            var newId = await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

            // Volver a leer el registro insertado
            var entity = await _context.Standings
                .FromSqlRaw("SELECT StandingID, LeagueID, TeamID, Wins, Draws, Losses, GoalsFor, GoalsAgainst, Points, CreatedAt FROM Standings WHERE StandingID = @id", new SqlParameter("@id", newId))
                .FirstAsync();

            // Cargar liga y datos relacionados
            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", new SqlParameter("@lid", entity.LeagueID))
                .FirstAsync();

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            var teamPlayers = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .ToListAsync();

            var players = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

            return entity.ToDomain(league, teamPlayers, players);
        }

        public async Task UpdateAsync(Standing standing)
        {
            var sql = @"
                UPDATE Standings
                SET 
                    LeagueID       = @lid,
                    TeamID         = @tid,
                    Wins           = @w,
                    Draws          = @d,
                    Losses         = @l,
                    GoalsFor       = @gf,
                    GoalsAgainst   = @ga,
                    Points         = @p
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
            var sql = "DELETE FROM Standings WHERE StandingID = @id";
            var param = new SqlParameter("@id", standingId.Value);

            var affected = await _context.Database.ExecuteSqlRawAsync(sql, param);
            return affected > 0;
        }
    }
}
