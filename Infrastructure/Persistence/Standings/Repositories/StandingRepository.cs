//using Domain.Entities.Standings;
//using Domain.Ports.Standings;
//using Domain.Shared;
//using Infrastructure.Persistence.Conection;
//using Infrastructure.Persistence.Standings.Mapper;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace Infrastructure.Persistence.Standings.Repositories
//{
//    public class StandingRepository : IStandingRepository
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<StandingRepository> _logger;
//        private readonly StandingMapper _mapper;

//        public StandingRepository(ApplicationDbContext context, ILogger<StandingRepository> logger, StandingMapper mapper)
//        {
//            _context = context;
//            _logger = logger;
//            _mapper = mapper;
//        }

//        public async Task<IEnumerable<Standing>> GetAllAsync()
//        {
//            try
//            {
//                string sql = "SELECT * FROM Standings";
//                var dbStandings = await _context.Standings
//                    .FromSqlRaw(sql)
//                    .ToListAsync();

//                var leagues = await _context.Leagues.ToListAsync();
//                var teams = await _context.Teams.ToListAsync();

//                return dbStandings.Select(dbStanding =>
//                {
//                    var league = leagues.FirstOrDefault(l => l.LeagueID == dbStanding.LeagueID);
//                    var team = teams.FirstOrDefault(t => t.TeamID == dbStanding.TeamID);
//                    return _mapper.MapToDomain(dbStanding, league, team);
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener la lista de standings");
//                return new List<Standing>();
//            }
//        }

//        public async Task<Standing?> GetByIdAsync(int standingId)
//        {
//            try
//            {
//                var dbStanding = await _context.Standings
//                    .FromSqlRaw("SELECT * FROM Standings WHERE StandingID = @StandingID", new SqlParameter("@StandingID", standingId))
//                    .FirstOrDefaultAsync();

//                if (dbStanding == null)
//                    return null;

//                var league = await _context.Leagues.FirstOrDefaultAsync(l => l.LeagueID == dbStanding.LeagueID);
//                var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamID == dbStanding.TeamID);

//                return _mapper.MapToDomain(dbStanding, league, team);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener el standing con ID {StandingID}", standingId);
//                return null;
//            }
//        }

//        public async Task<IEnumerable<Standing>> GetByLeagueIdAsync(int leagueId)
//        {
//            try
//            {
//                var dbStandings = await _context.Standings
//                    .FromSqlRaw("SELECT * FROM Standings WHERE LeagueID = @LeagueID", new SqlParameter("@LeagueID", leagueId))
//                    .ToListAsync();

//                var league = await _context.Leagues.FirstOrDefaultAsync(l => l.LeagueID == leagueId);
//                var teams = await _context.Teams.ToListAsync();

//                return dbStandings.Select(dbStanding =>
//                {
//                    var team = teams.FirstOrDefault(t => t.TeamID == dbStanding.TeamID);
//                    return _mapper.MapToDomain(dbStanding, league, team);
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener standings por ID de liga");
//                return new List<Standing>();
//            }
//        }

//        // Método que retorna la clasificación ordenada para una liga.
//        // Aquí se ordena por puntos (y se pueden añadir criterios adicionales según la lógica de negocio).
//        public async Task<IEnumerable<Standing>> GetClassificationByLeagueIdAsync(int leagueId)
//        {
//            try
//            {
//                var dbStandings = await _context.Standings
//                    .FromSqlRaw("SELECT * FROM Standings WHERE LeagueID = @LeagueID", new SqlParameter("@LeagueID", leagueId))
//                    .ToListAsync();

//                var league = await _context.Leagues.FirstOrDefaultAsync(l => l.LeagueID == leagueId);
//                var teams = await _context.Teams.ToListAsync();

//                var orderedStandings = dbStandings
//                    .OrderByDescending(s => s.Points)
//                    .ThenByDescending(s => s.GoalDifference)
//                    .ToList();

//                return orderedStandings.Select(dbStanding =>
//                {
//                    var team = teams.FirstOrDefault(t => t.TeamID == dbStanding.TeamID);
//                    return _mapper.MapToDomain(dbStanding, league, team);
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener la clasificación para la liga con ID {LeagueID}", leagueId);
//                return new List<Standing>();
//            }
//        }

//        // Retorna el standing específico para un equipo en una liga.
//        public async Task<Standing?> GetByTeamIdAndLeagueIdAsync(int teamId, int leagueId)
//        {
//            try
//            {
//                var dbStanding = await _context.Standings
//                    .FromSqlRaw("SELECT * FROM Standings WHERE TeamID = @TeamID AND LeagueID = @LeagueID",
//                        new SqlParameter("@TeamID", teamId),
//                        new SqlParameter("@LeagueID", leagueId))
//                    .FirstOrDefaultAsync();

//                if (dbStanding == null)
//                    return null;

//                var league = await _context.Leagues.FirstOrDefaultAsync(l => l.LeagueID == dbStanding.LeagueID);
//                var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamID == dbStanding.TeamID);

//                return _mapper.MapToDomain(dbStanding, league, team);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener el standing para el equipo {TeamID} en la liga {LeagueID}", teamId, leagueId);
//                return null;
//            }
//        }

//        public async Task<Standing> AddAsync(Standing standing)
//        {
//            if (standing == null)
//                throw new ArgumentNullException(nameof(standing), "El standing no puede ser null");

//            try
//            {
//                var standingEntity = _mapper.MapToEntity(standing);

//                string insertSql = @"INSERT INTO Standings (LeagueID, TeamID, Points, Wins, Losses, Draws, GoalDifference, CreatedAt) 
//                                     VALUES (@LeagueID, @TeamID, @Points, @Wins, @Losses, @Draws, @GoalDifference, @CreatedAt);";

//                var parameters = new[]
//                {
//                    new SqlParameter("@LeagueID", standingEntity.LeagueID),
//                    new SqlParameter("@TeamID", standingEntity.TeamID),
//                    new SqlParameter("@Points", standingEntity.Points),
//                    new SqlParameter("@Wins", standingEntity.Wins),
//                    new SqlParameter("@Losses", standingEntity.Losses),
//                    new SqlParameter("@Draws", standingEntity.Draws),
//                    new SqlParameter("@GoalDifference", standingEntity.GoalDifference),
//                    new SqlParameter("@CreatedAt", standingEntity.CreatedAt)
//                };

//                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

//                var newStandingId = await _context.Standings
//                    .FromSqlRaw("SELECT TOP 1 StandingID FROM Standings ORDER BY StandingID DESC")
//                    .Select(s => s.StandingID)
//                    .FirstOrDefaultAsync();

//                // Se retorna un objeto Standing construido con el nuevo ID.
//                return new Standing(
//                    new StandingID(newStandingId),
//                    standing.LeagueID,
//                    standing.TeamID,
//                    standing.Points,
//                    new MatchesPlayed(standing.Wins.Value + standing.Losses.Value + standing.Draws.Value),
//                    standing.Wins,
//                    standing.Draws,
//                    standing.Losses,
//                    standing.GoalDifference,
//                    standing.League,
//                    standing.Team,
//                    standing.CreatedAt
//                );
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al agregar un nuevo standing");
//                throw;
//            }
//        }

//        public async Task UpdateAsync(Standing standing)
//        {
//            if (standing == null)
//                throw new ArgumentNullException(nameof(standing), "El standing no puede ser null");

//            try
//            {
//                var standingEntity = _mapper.MapToEntity(standing);

//                string updateSql = @"UPDATE Standings 
//                                     SET LeagueID = @LeagueID, TeamID = @TeamID, Points = @Points, Wins = @Wins, Losses = @Losses, Draws = @Draws, GoalDifference = @GoalDifference
//                                     WHERE StandingID = @StandingID";

//                var parameters = new[]
//                {
//                    new SqlParameter("@StandingID", standingEntity.StandingID),
//                    new SqlParameter("@LeagueID", standingEntity.LeagueID),
//                    new SqlParameter("@TeamID", standingEntity.TeamID),
//                    new SqlParameter("@Points", standingEntity.Points),
//                    new SqlParameter("@Wins", standingEntity.Wins),
//                    new SqlParameter("@Losses", standingEntity.Losses),
//                    new SqlParameter("@Draws", standingEntity.Draws),
//                    new SqlParameter("@GoalDifference", standingEntity.GoalDifference)
//                };

//                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al actualizar el standing con ID {StandingID}", standing.StandingID.Value);
//                throw;
//            }
//        }

//        public async Task<bool> DeleteAsync(int standingId)
//        {
//            try
//            {
//                var dbStanding = await _context.Standings
//                    .FromSqlRaw("SELECT * FROM Standings WHERE StandingID = @StandingID", new SqlParameter("@StandingID", standingId))
//                    .FirstOrDefaultAsync();

//                if (dbStanding == null)
//                    return false;

//                _context.Standings.Remove(dbStanding);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al eliminar el standing con ID {StandingID}", standingId);
//                return false;
//            }
//        }
//    }
//}
