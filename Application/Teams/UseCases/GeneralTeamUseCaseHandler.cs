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
        private readonly GetAllTeams _getAllTeams;

        public GeneralTeamUseCaseHandler(
            GetTeamById getTeamById,
            CreateTeam createTeam,
            UpdateTeam updateTeam,
            DeleteTeam deleteTeam,
            GetAllTeams getAllTeams)
        {
            _getTeamById = getTeamById;
            _createTeam = createTeam;
            _updateTeam = updateTeam;
            _deleteTeam = deleteTeam;
            _getAllTeams = getAllTeams;
        }

        public async Task<object> Execute(TeamActionDTO actionDTO)
        {
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllTeams.ExecuteAsync();

                case "getbyid":
                    if (actionDTO.TeamID == null)
                        throw new ArgumentException("El ID del equipo es necesario para obtenerlo.");
                    return await _getTeamById.ExecuteAsync(new TeamID(actionDTO.TeamID.Value));

                case "add":
                    if (actionDTO.Team == null)
                        throw new ArgumentException("Los detalles del equipo son necesarios para agregarlo.");
                    return await _createTeam.Execute(actionDTO.Team);

                case "update":
                    if (actionDTO.Team == null)
                        throw new ArgumentException("Los detalles del equipo son necesarios para actualizarlo.");
                    return await _updateTeam.Execute(actionDTO.Team,actionDTO.TeamID.Value);

                case "delete":
                    if (actionDTO.TeamID == null)
                        throw new ArgumentException("El ID del equipo es necesario para eliminarlo.");
                    return await _deleteTeam.Execute(new TeamID(actionDTO.TeamID.Value));

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}
