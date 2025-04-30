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
        private readonly StandingMapper _standingMapper;
        private readonly LeagueMapper _leagueMapper;
        private readonly TeamMapper _teamMapper;

        public StandingRepository(
            ApplicationDbContext context,
            StandingMapper standingMapper,
            LeagueMapper leagueMapper,
            TeamMapper teamMapper)
        {
            _context = context;
            _standingMapper = standingMapper;
            _leagueMapper = leagueMapper;
            _teamMapper = teamMapper;
        }

        public async Task<IEnumerable<Standing>> GetAllAsync()
        {
            var sql = "SELECT * FROM Standings";
            var dbStandings = await _context.Standings
                                           .FromSqlRaw(sql)
                                           .ToListAsync();

            // Cargamos todas ligas y equipos de una vez
            var leagueList = await _context.Leagues.FromSqlRaw("SELECT * FROM Leagues").ToListAsync();
            var teamList = await _context.Teams.FromSqlRaw("SELECT * FROM Teams").ToListAsync();
            var teamPlayers = await _context.TeamPlayers.FromSqlRaw("SELECT * FROM TeamPlayers").ToListAsync();
            var players = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();

            return dbStandings.Select(e =>
            {
                var leagueEntity = leagueList.First(l => l.LeagueID == e.LeagueID);
                var teamEntity = teamList.First(t => t.TeamID == e.TeamID);

                var league = _leagueMapper.MapToDomainSimple(leagueEntity);
                var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);

                return _standingMapper.MapToDomain(e, league, teamPlayers, players);
            }).ToList();
        }

        public async Task<Standing?> GetByIdAsync(StandingID standingId)
        {
            var sql = "SELECT * FROM Standings WHERE StandingID = @id";
            var param = new SqlParameter("@id", standingId.Value);

            var entity = await _context.Standings
                                      .FromSqlRaw(sql, param)
                                      .FirstOrDefaultAsync();
            if (entity == null) return null;

            // Traemos liga, equipo, jugadores y teamPlayers con SQL crudo
            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", new SqlParameter("@lid", entity.LeagueID))
                .FirstAsync();

            var teamEntity = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .FirstAsync();

            var teamPlayers = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .ToListAsync();

            var players = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);
            var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);

            return _standingMapper.MapToDomain(entity, league, teamPlayers, players);
        }

        public async Task<IEnumerable<Standing>> GetByLeagueIdAsync(LeagueID leagueId)
        {
            var sql = "SELECT * FROM Standings WHERE LeagueID = @lid";
            var param = new SqlParameter("@lid", leagueId.Value);

            var dbStandings = await _context.Standings
                                           .FromSqlRaw(sql, param)
                                           .ToListAsync();

            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", param)
                .FirstAsync();
            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            var teamList = await _context.Teams.FromSqlRaw("SELECT * FROM Teams").ToListAsync();
            var teamPlayers = await _context.TeamPlayers.FromSqlRaw("SELECT * FROM TeamPlayers").ToListAsync();
            var players = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();

            return dbStandings.Select(e =>
            {
                var teamEntity = teamList.First(t => t.TeamID == e.TeamID);
                var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);
                return _standingMapper.MapToDomain(e, league, teamPlayers, players);
            }).ToList();
        }

        public async Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(LeagueID leagueId)
        {
            var sql = "SELECT * FROM Standings WHERE LeagueID = @lid ORDER BY Points DESC, (GoalsFor - GoalsAgainst) DESC";
            var param = new SqlParameter("@lid", leagueId.Value);

            var dbStandings = await _context.Standings
                                           .FromSqlRaw(sql, param)
                                           .ToListAsync();

            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", param)
                .FirstAsync();
            var league = _leagueMapper.MapToDomainSimple(leagueEntity);

            var teamList = await _context.Teams.FromSqlRaw("SELECT * FROM Teams").ToListAsync();
            var teamPlayers = await _context.TeamPlayers.FromSqlRaw("SELECT * FROM TeamPlayers").ToListAsync();
            var players = await _context.Players.FromSqlRaw("SELECT * FROM Players").ToListAsync();

            return dbStandings.Select(e =>
            {
                var teamEntity = teamList.First(t => t.TeamID == e.TeamID);
                var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);
                return _standingMapper.MapToDomain(e, league, teamPlayers, players);
            }).ToList();
        }

        public async Task<Standing?> GetByTeamIdAndLeagueIdAsync(TeamID teamId, LeagueID leagueId)
        {
            var sql = "SELECT * FROM Standings WHERE TeamID = @tid AND LeagueID = @lid";
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
                .FirstAsync();
            var teamEntity = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @tid", parameters[0])
                .FirstAsync();

            var teamPlayers = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @tid", parameters[0])
                .ToListAsync();

            var players = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);
            var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);

            return _standingMapper.MapToDomain(entity, league, teamPlayers, players);
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

            // Ejecuto el INSERT y recupero el nuevo ID
            var newId = await _context.Database
                .ExecuteSqlRawAsync(insertSql, parameters);

            // Vuelvo a leer el registro insertado
            var entity = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE StandingID = @id", new SqlParameter("@id", newId))
                .FirstAsync();

            var leagueEntity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE LeagueID = @lid", new SqlParameter("@lid", entity.LeagueID))
                .FirstAsync();
            var teamEntity = await _context.Teams
                .FromSqlRaw("SELECT * FROM Teams WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .FirstAsync();

            var teamPlayers = await _context.TeamPlayers
                .FromSqlRaw("SELECT * FROM TeamPlayers WHERE TeamID = @tid", new SqlParameter("@tid", entity.TeamID))
                .ToListAsync();

            var players = await _context.Players
                .FromSqlRaw("SELECT * FROM Players")
                .ToListAsync();

            var league = _leagueMapper.MapToDomainSimple(leagueEntity);
            var team = _teamMapper.MapToDomain(teamEntity, league, teamPlayers, players);

            return _standingMapper.MapToDomain(entity, league, teamPlayers, players);
        }
    }
}
