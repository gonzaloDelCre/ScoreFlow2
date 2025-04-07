//using Domain.Entities.Matches;
//using Domain.Entities.Teams;
//using Domain.Ports.Matches;
//using Domain.Shared;
//using Infrastructure.Persistence.Conection;
//using Infrastructure.Persistence.Matches.Mapper;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace Infrastructure.Persistence.Matches.Repositories
//{
//    public class MatchRepository : IMatchRepository
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<MatchRepository> _logger;

//        public MatchRepository(ApplicationDbContext context, ILogger<MatchRepository> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task<IEnumerable<Match>> GetAllAsync()
//        {
//            try
//            {
//                string sql = "SELECT * FROM Matches";
//                var dbMatches = await _context.Matches
//                    .FromSqlRaw(sql)
//                    .ToListAsync();

//                return dbMatches.Select(MatchMapper.ToDomain).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener la lista de partidos");
//                return new List<Match>();
//            }
//        }

//        public async Task<Match?> GetByIdAsync(MatchID matchId)
//        {
//            try
//            {
//                string sql = "SELECT * FROM Matches WHERE MatchID = @MatchID";
//                var parameter = new SqlParameter("@MatchID", matchId.Value);

//                var dbMatches = await _context.Matches
//                    .FromSqlRaw(sql, parameter)
//                    .ToListAsync();

//                var dbMatch = dbMatches.FirstOrDefault();
//                return dbMatch != null ? MatchMapper.ToDomain(dbMatch) : null;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener el partido con ID {MatchID}", matchId.Value);
//                return null;
//            }
//        }

//        public async Task<IEnumerable<Match>> GetByTeamIdAsync(MatchID teamId)
//        {
//            try
//            {
//                string sql = "SELECT * FROM Matches WHERE Team1ID = @TeamID OR Team2ID = @TeamID";
//                var parameter = new SqlParameter("@TeamID", teamId.Value);

//                var dbMatches = await _context.Matches
//                    .FromSqlRaw(sql, parameter)
//                    .ToListAsync();

//                return dbMatches.Select(MatchMapper.ToDomain).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener los partidos para el equipo con ID {TeamID}", teamId.Value);
//                return new List<Match>();
//            }
//        }

//        public async Task<IEnumerable<Match>> GetByLeagueIdAsync(MatchID leagueId)
//        {
//            try
//            {
//                string sql = "SELECT * FROM Matches WHERE LeagueID = @LeagueID";
//                var parameter = new SqlParameter("@LeagueID", leagueId.Value);

//                var dbMatches = await _context.Matches
//                    .FromSqlRaw(sql, parameter)
//                    .ToListAsync();

//                return dbMatches.Select(MatchMapper.ToDomain).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al obtener los partidos para la liga con ID {LeagueID}", leagueId.Value);
//                return new List<Match>();
//            }
//        }

//        public async Task<Match> AddAsync(Match match)
//        {
//            if (match == null)
//                throw new ArgumentNullException(nameof(match), "El partido no puede ser null");

//            try
//            {
//                var matchEntity = MatchMapper.ToEntity(match);

//                string insertSql = @"INSERT INTO Matches (Team1ID, Team2ID, DateTime, ScoreTeam1, ScoreTeam2, Status, Location, CreatedAt) 
//                                     VALUES (@Team1ID, @Team2ID, @DateTime, @ScoreTeam1, @ScoreTeam2, @Status, @Location, @CreatedAt);";

//                var parameters = new[]
//                {
//                    new SqlParameter("@Team1ID", matchEntity.Team1ID),
//                    new SqlParameter("@Team2ID", matchEntity.Team2ID),
//                    new SqlParameter("@DateTime", matchEntity.DateTime),
//                    new SqlParameter("@ScoreTeam1", matchEntity.ScoreTeam1),
//                    new SqlParameter("@ScoreTeam2", matchEntity.ScoreTeam2),
//                    new SqlParameter("@Status", matchEntity.Status),
//                    new SqlParameter("@Location", matchEntity.Location ?? (object)DBNull.Value),
//                    new SqlParameter("@CreatedAt", matchEntity.CreatedAt)
//                };

//                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

//                string selectSql = "SELECT TOP 1 MatchID FROM Matches ORDER BY MatchID DESC";
//                var newMatchId = await _context.Matches
//                    .FromSqlRaw(selectSql)
//                    .Select(m => m.MatchID)
//                    .FirstOrDefaultAsync();

//                return new Match(
//                    new MatchID(newMatchId),
//                    match.Team1,
//                    match.Team2,
//                    match.MatchDate,
//                    match.Status,
//                    match.Location ?? string.Empty // Asegura que no sea null
//                );
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al agregar un nuevo partido");
//                throw;
//            }
//        }

//        public async Task UpdateAsync(Match match)
//        {
//            if (match == null)
//                throw new ArgumentNullException(nameof(match), "El partido no puede ser null");

//            try
//            {
//                var matchEntity = MatchMapper.ToEntity(match);

//                string updateSql = @"UPDATE Matches 
//                                     SET Team1ID = @Team1ID, Team2ID = @Team2ID, DateTime = @DateTime, ScoreTeam1 = @ScoreTeam1, ScoreTeam2 = @ScoreTeam2, 
//                                         Status = @Status, Location = @Location, CreatedAt = @CreatedAt
//                                     WHERE MatchID = @MatchID";

//                var parameters = new[]
//                {
//                    new SqlParameter("@MatchID", matchEntity.MatchID),
//                    new SqlParameter("@Team1ID", matchEntity.Team1ID),
//                    new SqlParameter("@Team2ID", matchEntity.Team2ID),
//                    new SqlParameter("@DateTime", matchEntity.DateTime),
//                    new SqlParameter("@ScoreTeam1", matchEntity.ScoreTeam1),
//                    new SqlParameter("@ScoreTeam2", matchEntity.ScoreTeam2),
//                    new SqlParameter("@Status", matchEntity.Status),
//                    new SqlParameter("@Location", matchEntity.Location ?? (object)DBNull.Value),
//                    new SqlParameter("@CreatedAt", matchEntity.CreatedAt)
//                };

//                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al actualizar el partido con ID {MatchID}", match.MatchID.Value);
//                throw;
//            }
//        }

//        public async Task<bool> DeleteAsync(MatchID matchId)
//        {
//            try
//            {
//                string sql = "DELETE FROM Matches WHERE MatchID = @MatchID";
//                var parameter = new SqlParameter("@MatchID", matchId.Value);

//                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
//                return rowsAffected > 0;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al eliminar el partido con ID {MatchID}", matchId.Value);
//                return false;
//            }
//        }
//    }
//}