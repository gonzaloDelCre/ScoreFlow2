using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Users.Mapper;
using Infrastructure.Persistence.Teams.Mappers;
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

        /// <summary>
        /// Get All Teams
        /// </summary>
        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM Teams";
                var dbTeams = await _context.Teams
                    .FromSqlRaw(sql)
                    .Include(t => t.Coach) // Incluimos el Coach
                    .ToListAsync();

                return dbTeams.Select(dbTeam =>
                {
                    User? coachDomain = dbTeam.Coach != null ? dbTeam.Coach.MapToDomain() : null;
                    return TeamMapper.MapToDomain(dbTeam, coachDomain);
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de equipos");
                return new List<Team>();
            }
        }

        /// <summary>
        /// Get Team By Id
        /// </summary>
        public async Task<Team?> GetByIdAsync(TeamID teamId)
        {
            try
            {
                string sql = "SELECT * FROM Teams WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);

                var dbTeams = await _context.Teams
                    .FromSqlRaw(sql, parameter)
                    .Include(t => t.Coach) // Incluimos el Coach
                    .ToListAsync();

                var dbTeam = dbTeams.FirstOrDefault();
                if (dbTeam == null)
                    return null;

                User? coachDomain = dbTeam.Coach != null ? dbTeam.Coach.MapToDomain() : null;
                return TeamMapper.MapToDomain(dbTeam, coachDomain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el equipo con ID {TeamID}", teamId.Value);
                return null;
            }
        } 

        /// <summary>
        /// Create Team
        /// </summary>
        public async Task<Team> AddAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                var teamEntity = team.MapToEntity();

                string insertSql = @"INSERT INTO Teams (Name, Logo, CoachID, CreatedAt) 
                                     VALUES (@Name, @Logo, @CoachID, @CreatedAt);";

                var parameters = new[]
                {
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@CoachID", teamEntity.CoachID),
                    new SqlParameter("@CreatedAt", teamEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                string selectSql = "SELECT TOP 1 TeamID FROM Teams ORDER BY TeamID DESC";
                var newTeamId = await _context.Teams
                    .FromSqlRaw(selectSql)
                    .Select(t => t.TeamID)
                    .FirstOrDefaultAsync();

                // Se asume que tras la inserción, el coach sigue siendo el mismo que se pasó en el objeto 'team'
                return new Team(
                    new TeamID(newTeamId),
                    team.Name,
                    team.Coach,
                    team.CreatedAt,
                    team.Logo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo equipo");
                throw;
            }
        }

        /// <summary>
        /// Update Team
        /// </summary>
        public async Task UpdateAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                var teamEntity = team.MapToEntity();

                string updateSql = @"UPDATE Teams 
                                     SET Name = @Name, Logo = @Logo, CoachID = @CoachID
                                     WHERE TeamID = @TeamID";

                var parameters = new[]
                {
                    new SqlParameter("@TeamID", teamEntity.TeamID),
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@CoachID", teamEntity.CoachID)
                };

                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el equipo con ID {TeamID}", team.TeamID.Value);
                throw;
            }
        }

        /// <summary>
        /// Delete Team
        /// </summary>
        public async Task<bool> DeleteAsync(TeamID teamId)
        {
            try
            {
                string sql = "DELETE FROM Teams WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el equipo con ID {TeamID}", teamId.Value);
                return false;
            }
        }
    }
}
