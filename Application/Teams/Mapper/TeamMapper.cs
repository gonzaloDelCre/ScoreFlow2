using Application.Teams.DTOs;
using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Enum;
using Domain.Shared;

namespace Application.Teams.Mapper
{
    public class TeamMapper
    {
        // Mapea una entidad Team a un DTO de respuesta (para enviar datos)
        public TeamResponseDTO MapToDTO(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "La entidad de dominio Team no puede ser nula.");

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,  // Accede a Value
                TeamName = team.Name.Value,  // Accede a Value
                LogoUrl = team.Logo,
                CoachID = team.Coach?.UserID.Value ?? 0, // Accede a Value si existe el Coach
                CoachName = team.Coach?.FullName.Value ?? string.Empty,
                CreatedAt = team.CreatedAt
            };
        }

        // Mapea un DTO de solicitud (para crear o actualizar un equipo) a la entidad de dominio
        public Team MapToDomain(TeamRequestDTO teamDTO, User? coach = null, Team? existingTeam = null)
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "El DTO TeamRequestDTO no puede ser nulo.");

            // Si tenemos un equipo existente, actualizamos los valores
            if (existingTeam != null)
            {
                return new Team(
                    existingTeam.TeamID, // Mantener el ID existente
                    new TeamName(teamDTO.Name), // Nuevo nombre
                    coach, // Coach puede ser nulo si no se pasa
                    existingTeam.CreatedAt, // Mantener la fecha de creación
                    teamDTO.Logo // Logo del equipo
                );
            }

            // Si no hay equipo existente, creamos un nuevo equipo
            return new Team(
                new TeamID(0), // El ID será asignado después en el repositorio
                new TeamName(teamDTO.Name), // Nombre
                coach, // Coach puede ser nulo si no se pasa
                DateTime.UtcNow, // Fecha de creación actual
                teamDTO.Logo // Logo del equipo
            );
        }
    }
}
