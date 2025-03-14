using Application.Teams.DTOs;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Teams.UseCases
{
    public class GeneralTeamUseCaseHandler
    {
        private readonly GetTeamById _getTeamById;
        private readonly CreateTeam _createTeam;
        private readonly UpdateTeam _updateTeam;
        private readonly DeleteTeam _deleteTeam;
        private readonly GetAllTeams _getAllTeams; // Agregado el servicio para obtener todos los equipos

        public GeneralTeamUseCaseHandler(
            GetTeamById getTeamById,
            CreateTeam createTeam,
            UpdateTeam updateTeam,
            DeleteTeam deleteTeam,
            GetAllTeams getAllTeams) // Inyectamos el caso de uso para obtener todos los equipos
        {
            _getTeamById = getTeamById;
            _createTeam = createTeam;
            _updateTeam = updateTeam;
            _deleteTeam = deleteTeam;
            _getAllTeams = getAllTeams;
        }

        public async Task<object> Execute(TeamActionDTO actionDTO)
        {
            // Validación inicial de la acción
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllTeams.Execute(); // Llamamos a GetAllTeams

                case "getbyid":
                    if (actionDTO.TeamID == null)
                        throw new ArgumentException("El ID del equipo es necesario para obtenerlo.");

                    var teamId = new TeamID(actionDTO.TeamID.Value);
                    var team = await _getTeamById.Execute(teamId);

                    if (team == null)
                        throw new ArgumentException("El equipo no existe.");

                    return team;

                case "add":
                    if (actionDTO.Team == null)
                        throw new ArgumentException("Los detalles del equipo son necesarios para agregarlo.");

                    var teamRequestDTO = actionDTO.Team;

                    return await _createTeam.Execute(teamRequestDTO); // Usamos CreateTeam para agregar

                case "update":
                    if (actionDTO.Team == null)
                        throw new ArgumentException("Los detalles del equipo son necesarios para actualizarlo.");

                    var teamUpdateDTO = actionDTO.Team;

                    return await _updateTeam.Execute(teamUpdateDTO);

                case "delete":
                    if (actionDTO.TeamID == null)
                        throw new ArgumentException("El ID del equipo es necesario para eliminarlo.");

                    var deleteTeamId = new TeamID(actionDTO.TeamID.Value);
                    var existingTeamForDelete = await _getTeamById.Execute(deleteTeamId);

                    if (existingTeamForDelete == null)
                        throw new ArgumentException("El equipo a eliminar no existe.");

                    return await _deleteTeam.Execute(deleteTeamId);

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}
