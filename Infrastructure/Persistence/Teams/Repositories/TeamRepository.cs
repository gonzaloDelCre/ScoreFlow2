using Domain.Entities.Teams;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Domain.Ports.Teams;
using Domain.Shared;
using Microsoft.Data.SqlClient;

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
            try
            {
                string sql = "SELECT * FROM Teams";
                var dbTeams = await _context.Teams
                    .FromSqlRaw(sql)
                    .ToListAsync();

                var coachIds = dbTeams.Select(t => t.CoachID).Distinct().ToList();
                var dbCoaches = await _context.Users
                    .Where(u => coachIds.Contains(u.UserID))
                    .ToListAsync();

                return dbTeams.Select(dbTeam =>
                {
                    var coachEntity = dbCoaches.FirstOrDefault(c => c.UserID == dbTeam.CoachID);
                    return TeamMapper.ToDomain(dbTeam, coachEntity); // Se usa directamente el método estático
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de equipos");
                return new List<Team>();
            }
        }

        public async Task<Team?> GetByIdAsync(TeamID teamId)
        {
            try
            {
                string sql = "SELECT * FROM Teams WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);

                var dbTeams = await _context.Teams
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbTeam = dbTeams.FirstOrDefault();
                if (dbTeam == null)
                {
                    return null;
                }

                var coachEntity = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserID == dbTeam.CoachID);

                return TeamMapper.ToDomain(dbTeam, coachEntity); // Se usa directamente el método estático
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el equipo con ID {TeamID}", teamId.Value);
                return null;
            }
        }

        public async Task<Team> AddAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                var teamEntity = TeamMapper.ToEntity(team); // Se usa directamente el método estático

                string insertSql = @"INSERT INTO Teams (Name, CoachID, CreatedAt, Logo) 
                                     VALUES (@Name, @CoachID, @CreatedAt, @Logo);";

                var parameters = new[]
                {
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@CoachID", teamEntity.CoachID),
                    new SqlParameter("@CreatedAt", teamEntity.CreatedAt),
                    new SqlParameter("@Logo", teamEntity.Logo ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                string selectSql = "SELECT TOP 1 TeamID FROM Teams ORDER BY TeamID DESC";
                var newTeamId = await _context.Teams
                    .FromSqlRaw(selectSql)
                    .Select(t => t.TeamID)
                    .FirstOrDefaultAsync();

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

        public async Task UpdateAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                var teamEntity = TeamMapper.ToEntity(team); // Se usa directamente el método estático

                string updateSql = @"UPDATE Teams 
                                     SET Name = @Name, CoachID = @CoachID, CreatedAt = @CreatedAt, Logo = @Logo
                                     WHERE TeamID = @TeamID";

                var parameters = new[]
                {
                    new SqlParameter("@TeamID", teamEntity.TeamID),
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@CoachID", teamEntity.CoachID),
                    new SqlParameter("@CreatedAt", teamEntity.CreatedAt),
                    new SqlParameter("@Logo", teamEntity.Logo ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el equipo con ID {TeamID}", team.TeamID.Value);
                throw;
            }
        }

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