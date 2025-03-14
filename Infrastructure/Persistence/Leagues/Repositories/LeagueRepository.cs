using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Leagues.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeagueRepository> _logger;
        private readonly LeagueMapper _mapper;

        public LeagueRepository(ApplicationDbContext context, ILogger<LeagueRepository> logger, LeagueMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all leagues
        /// </summary>
        public async Task<IEnumerable<League>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM Leagues";
                var dbLeagues = await _context.Leagues
                    .FromSqlRaw(sql)
                    .ToListAsync();

                return dbLeagues.Select(dbLeague => _mapper.MapToDomain(dbLeague)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de ligas");
                return new List<League>();
            }
        }

        /// <summary>
        /// Get league by ID
        /// </summary>
        public async Task<League?> GetByIdAsync(LeagueID leagueId)
        {
            try
            {
                string sql = "SELECT * FROM Leagues WHERE LeagueID = @LeagueID";
                var parameter = new SqlParameter("@LeagueID", leagueId.Value);

                var dbLeagues = await _context.Leagues
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbLeague = dbLeagues.FirstOrDefault();
                return dbLeague != null ? _mapper.MapToDomain(dbLeague) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la liga con ID {LeagueID}", leagueId.Value);
                return null;
            }
        }

        /// <summary>
        /// Get league by Name
        /// </summary>
        public async Task<League?> GetByNameAsync(string name)
        {
            try
            {
                // Realiza la consulta para obtener una liga por su nombre
                string sql = "SELECT * FROM Leagues WHERE Name = @Name";
                var parameter = new SqlParameter("@Name", name);

                var dbLeagues = await _context.Leagues
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbLeague = dbLeagues.FirstOrDefault();
                return dbLeague != null ? _mapper.MapToDomain(dbLeague) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la liga con nombre {Name}", name);
                return null;
            }
        }

        /// <summary>
        /// Add a new league
        /// </summary>
        public async Task<League> AddAsync(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league), "La liga no puede ser null");

            try
            {
                var leagueEntity = _mapper.MapToEntity(league);

                string insertSql = @"INSERT INTO Leagues (Name, Description, CreatedAt) 
                                     VALUES (@Name, @Description, @CreatedAt);";

                var parameters = new[]
                {
                    new SqlParameter("@Name", leagueEntity.Name),
                    new SqlParameter("@Description", leagueEntity.Description ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedAt", leagueEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                string selectSql = "SELECT TOP 1 LeagueID FROM Leagues ORDER BY LeagueID DESC";
                var newLeagueId = await _context.Leagues
                    .FromSqlRaw(selectSql)
                    .Select(l => l.LeagueID)
                    .FirstOrDefaultAsync();

                return new League(
                    new LeagueID(newLeagueId),
                    league.Name,
                    league.Description,
                    league.CreatedAt
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar una nueva liga");
                throw;
            }
        }

        /// <summary>
        /// Update an existing league
        /// </summary>
        public async Task UpdateAsync(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league), "La liga no puede ser null");

            try
            {
                var leagueEntity = _mapper.MapToEntity(league);

                string updateSql = @"UPDATE Leagues 
                                     SET Name = @Name, Description = @Description, CreatedAt = @CreatedAt
                                     WHERE LeagueID = @LeagueID";

                var parameters = new[]
                {
                    new SqlParameter("@LeagueID", leagueEntity.LeagueID),
                    new SqlParameter("@Name", leagueEntity.Name),
                    new SqlParameter("@Description", leagueEntity.Description ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedAt", leagueEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la liga con ID {LeagueID}", league.LeagueID.Value);
                throw;
            }
        }

        /// <summary>
        /// Delete a league by ID
        /// </summary>
        public async Task<bool> DeleteAsync(LeagueID leagueId)
        {
            try
            {
                string sql = "DELETE FROM Leagues WHERE LeagueID = @LeagueID";
                var parameter = new SqlParameter("@LeagueID", leagueId.Value);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la liga con ID {LeagueID}", leagueId.Value);
                return false;
            }
        }
    }
}
