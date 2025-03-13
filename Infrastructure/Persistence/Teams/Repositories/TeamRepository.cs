using Domain.Entities.Teams;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Domain.Ports.Teams;
using Domain.Shared;

namespace Infrastructure.Persistence.Teams.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamRepository> _logger;
        private readonly TeamMapper _mapper;

        public TeamRepository(ApplicationDbContext context, ILogger<TeamRepository> logger, TeamMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            try
            {
                // Obtener todos los equipos de la base de datos
                var dbTeams = await _context.Teams
                    .ToListAsync();

                // Obtener los entrenadores relacionados a los equipos
                var coachIds = dbTeams.Select(t => t.CoachID).Distinct().ToList();
                var dbCoaches = await _context.Users
                    .Where(u => coachIds.Contains(u.UserID))
                    .ToListAsync();

                // Mapear los equipos y sus entrenadores
                var teams = dbTeams.Select(dbTeam =>
                {
                    // Obtener el coach correspondiente al equipo
                    var coachEntity = dbCoaches.FirstOrDefault(c => c.UserID == dbTeam.CoachID);

                    // Mapear el equipo y el coach a dominio
                    return _mapper.MapToDomain(dbTeam, coachEntity);
                }).ToList();

                return teams;
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
                var dbTeam = await _context.Teams
                    .Include(t => t.Coach)  // Incluir la relación con el entrenador (Coach)
                    .FirstOrDefaultAsync(t => t.TeamID == teamId.Value);

                if (dbTeam == null)
                {
                    return null;
                }

                // Obtener el entrenador (UserEntity)
                var coachEntity = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserID == dbTeam.CoachID);

                // Convertir la entidad de persistencia a dominio, pasando la entidad del entrenador también
                return _mapper.MapToDomain(dbTeam, coachEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el equipo con ID {TeamID}", teamId.Value);
                return null;
            }
        }

        public async Task<Team> AddAsync(Team team)
        {
            try
            {
                // Convertir la entidad de dominio a la entidad de persistencia
                var teamEntity = _mapper.MapToEntity(team);

                // Si el equipo tiene un Coach asociado, asegúrate de que la relación esté correctamente configurada
                if (team.Coach != null)
                {
                    // Asegúrate de que el CoachID esté correctamente asignado
                    teamEntity.CoachID = team.Coach.UserID.Value;
                }

                // Agregar el equipo a la base de datos
                await _context.Teams.AddAsync(teamEntity);
                await _context.SaveChangesAsync();

                // Obtener la entidad persistente del equipo (incluyendo la relación con el Coach)
                var teamEntityFromDb = await _context.Teams
                    .Include(t => t.Coach) // Incluir el Coach
                    .FirstOrDefaultAsync(t => t.TeamID == teamEntity.TeamID);

                // Regresar la entidad de dominio mapeada
                return _mapper.MapToDomain(teamEntityFromDb, teamEntityFromDb?.Coach);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo equipo");
                throw;
            }
        }


        public async Task UpdateAsync(Team team)
        {
            try
            {
                // Convertimos la entidad de dominio a la entidad de persistencia
                var teamEntity = _mapper.MapToEntity(team);

                _context.Teams.Update(teamEntity);
                await _context.SaveChangesAsync();
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
                var teamEntity = await _context.Teams
                    .FirstOrDefaultAsync(t => t.TeamID == teamId.Value);

                if (teamEntity != null)
                {
                    _context.Teams.Remove(teamEntity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el equipo con ID {TeamID}", teamId.Value);
                return false;
            }
        }
    }
}
