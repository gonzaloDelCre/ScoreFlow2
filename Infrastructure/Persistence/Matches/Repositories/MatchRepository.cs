using Domain.Entities.Matches;
using Domain.Entities.MatchEvents;
using Domain.Entities.PlayerStatistics;
using Domain.Entities.Teams;
using Domain.Ports.Matches;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Matches.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Persistence.Matches.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MatchRepository> _logger;
        private readonly IMatchMapper _mapper;

        public MatchRepository(
            ApplicationDbContext context,
            ILogger<MatchRepository> logger,
            IMatchMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un partido por su ID
        /// </summary>
        public async Task<Match?> GetByIdAsync(MatchID matchId)
        {
            var entity = await _context.Matches
                .FromSqlRaw("SELECT * FROM Matches WHERE ID = @ID", new SqlParameter("@ID", matchId.Value))
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.League)                         
                .Include(m => m.MatchEvents).ThenInclude(e => e.Player)
                .Include(m => m.PlayerStatistics).ThenInclude(s => s.Player)
                .FirstOrDefaultAsync();

            return entity == null
                ? null
                : _mapper.ToDomain(entity);
        }

        /// <summary>
        /// Obtiene todos los partidos
        /// </summary>
        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            var entities = await _context.Matches
                .FromSqlRaw("SELECT * FROM Matches")
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.League)                        
                .Include(m => m.MatchEvents).ThenInclude(e => e.Player)
                .Include(m => m.PlayerStatistics).ThenInclude(s => s.Player)
                .ToListAsync();

            return entities.Select(e => _mapper.ToDomain(e));
        }

        /// <summary>
        /// Agrega un nuevo partido, incluyendo marcador y estado
        /// </summary>
        public async Task<Match> AddAsync(Match match)
        {
            const string sql = @"
                INSERT INTO Matches 
                    (Team1ID, Team2ID, LeagueID, Jornada, DateTime, ScoreTeam1, ScoreTeam2, Status, Location, CreatedAt)
                VALUES 
                    (@Team1ID, @Team2ID, @LeagueID, @Jornada, @DateTime, @ScoreTeam1, @ScoreTeam2, @Status, @Location, @CreatedAt)";

            var parameters = new[]
            {
                new SqlParameter("@Team1ID", match.Team1.TeamID.Value),
                new SqlParameter("@Team2ID", match.Team2.TeamID.Value),
                new SqlParameter("@LeagueID", match.League.LeagueID.Value),   
                new SqlParameter("@Jornada", match.Jornada),                  
                new SqlParameter("@DateTime", match.MatchDate),
                new SqlParameter("@ScoreTeam1", match.ScoreTeam1),
                new SqlParameter("@ScoreTeam2", match.ScoreTeam2),
                new SqlParameter("@Status", (int)match.Status),
                new SqlParameter("@Location", match.Location ?? (object)DBNull.Value),
                new SqlParameter("@CreatedAt", match.CreatedAt)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            // Obtener el ID recién insertado
            var newId = await _context.Matches
                .FromSqlRaw("SELECT TOP 1 * FROM Matches ORDER BY ID DESC")
                .Select(m => m.ID)
                .FirstAsync();

            // Recuperar entidad completa
            var entity = await _context.Matches
                .FromSqlRaw("SELECT * FROM Matches WHERE ID = @ID", new SqlParameter("@ID", newId))
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.League)                        
                .FirstAsync();

            return _mapper.ToDomain(entity);
        }

        /// <summary>
        /// Actualiza un partido existente
        /// </summary>
        public async Task UpdateAsync(Match match)
        {
            const string sql = @"
                UPDATE Matches
                SET Team1ID   = @Team1ID,
                    Team2ID   = @Team2ID,
                    LeagueID  = @LeagueID,   -- liga
                    Jornada   = @Jornada,    -- jornada
                    DateTime  = @DateTime,
                    ScoreTeam1 = @ScoreTeam1,
                    ScoreTeam2 = @ScoreTeam2,
                    Status    = @Status,
                    Location  = @Location
                WHERE ID = @ID";

            var parameters = new[]
            {
                new SqlParameter("@ID", match.MatchID.Value),
                new SqlParameter("@Team1ID", match.Team1.TeamID.Value),
                new SqlParameter("@Team2ID", match.Team2.TeamID.Value),
                new SqlParameter("@LeagueID", match.League.LeagueID.Value),   
                new SqlParameter("@Jornada", match.Jornada),                  
                new SqlParameter("@DateTime", match.MatchDate),
                new SqlParameter("@ScoreTeam1", match.ScoreTeam1),
                new SqlParameter("@ScoreTeam2", match.ScoreTeam2),
                new SqlParameter("@Status", (int)match.Status),
                new SqlParameter("@Location", match.Location ?? (object)DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <summary>
        /// Elimina un partido por su ID
        /// </summary>
        public async Task<bool> DeleteAsync(MatchID matchId)
        {
            const string sql = "DELETE FROM Matches WHERE ID = @ID";
            var rows = await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@ID", matchId.Value));
            return rows > 0;
        }

        /// <summary>
        /// Obtiene los partidos de un equipo
        /// </summary>
        public async Task<IEnumerable<Match>> GetByTeamIdAsync(int teamId)
        {
            var entities = await _context.Matches
                .FromSqlRaw("SELECT * FROM Matches WHERE Team1ID = @TeamID OR Team2ID = @TeamID", new SqlParameter("@TeamID", teamId))
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.League)                         
                .Include(m => m.MatchEvents).ThenInclude(e => e.Player)
                .Include(m => m.PlayerStatistics).ThenInclude(s => s.Player)
                .ToListAsync();

            return entities.Select(e => _mapper.ToDomain(e));
        }

        /// <summary>
        /// Obtiene los partidos de una liga
        /// </summary>
        public async Task<IEnumerable<Match>> GetByLeagueIdAsync(int leagueId)
        {
            var entities = await _context.Matches
                .FromSqlRaw("SELECT * FROM Matches WHERE LeagueID = @LeagueID", new SqlParameter("@LeagueID", leagueId))  
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.League)
                .Include(m => m.MatchEvents).ThenInclude(e => e.Player)
                .Include(m => m.PlayerStatistics).ThenInclude(s => s.Player)
                .ToListAsync();

            return entities.Select(e => _mapper.ToDomain(e));
        }

        /// <summary>
        /// Obtiene los eventos de un partido
        /// </summary>
        public async Task<IEnumerable<MatchEvent>> GetEventsAsync(MatchID matchId)
            => (await GetByIdAsync(matchId))?.MatchEvents ?? Enumerable.Empty<MatchEvent>();

        /// <summary>
        /// Actualiza los eventos de un partido
        /// </summary>
        public async Task UpdateEventsAsync(MatchID matchId, IEnumerable<MatchEvent> events)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM MatchEvents WHERE MatchID = @MatchID",
                new SqlParameter("@MatchID", matchId.Value));

            foreach (var evt in events)
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO MatchEvents 
                        (MatchID, PlayerID, EventType, Minute, CreatedAt)
                    VALUES 
                        (@MatchID, @PlayerID, @EventType, @Minute, @CreatedAt)",
                    new SqlParameter("@MatchID", matchId.Value),
                    new SqlParameter("@PlayerID", evt.PlayerID?.Value ?? (object)DBNull.Value),
                    new SqlParameter("@EventType", (int)evt.EventType),
                    new SqlParameter("@Minute", evt.Minute),
                    new SqlParameter("@CreatedAt", evt.CreatedAt));
            }
        }

        /// <summary>
        /// Obtiene las estadísticas de jugadores de un partido
        /// </summary>
        public async Task<IEnumerable<PlayerStatistic>> GetStatisticsAsync(MatchID matchId)
            => (await GetByIdAsync(matchId))?.PlayerStatistics ?? Enumerable.Empty<PlayerStatistic>();

        /// <summary>
        /// Actualiza las estadísticas de jugadores de un partido
        /// </summary>
        public async Task UpdateStatisticsAsync(MatchID matchId, IEnumerable<PlayerStatistic> stats)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM PlayerStatistics WHERE MatchID = @MatchID",
                new SqlParameter("@MatchID", matchId.Value));

            foreach (var stat in stats)
            {
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO PlayerStatistics 
                        (MatchID, PlayerID, Goals, Assists, YellowCards, RedCards, MinutesPlayed, CreatedAt)
                    VALUES 
                        (@MatchID, @PlayerID, @Goals, @Assists, @YellowCards, @RedCards, @MinutesPlayed, @CreatedAt)",
                    new SqlParameter("@MatchID", matchId.Value),
                    new SqlParameter("@PlayerID", stat.Player.PlayerID.Value),
                    new SqlParameter("@Goals", stat.Goals),
                    new SqlParameter("@Assists", stat.Assists),
                    new SqlParameter("@YellowCards", stat.YellowCards),
                    new SqlParameter("@RedCards", stat.RedCards),
                    new SqlParameter("@MinutesPlayed", stat.MinutesPlayed),
                    new SqlParameter("@CreatedAt", stat.CreatedAt));
            }
        }
    }
}