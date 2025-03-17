using Domain.Shared;
using Application.Leagues.DTOs;
using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Update;
using Application.Leagues.UseCases.Get;

namespace Application.Leagues.UseCases
{
    public class GeneralLeagueUseCaseHandler
    {
        private readonly GetAllLeagues _getAllLeagues;
        private readonly GetLeagueById _getLeagueById;
        private readonly CreateLeague _addLeague;
        private readonly UpdateLeague _updateLeague;
        private readonly DeleteLeague _deleteLeague;

        public GeneralLeagueUseCaseHandler(
            GetAllLeagues getAllLeagues,
            GetLeagueById getLeagueById,
            CreateLeague addLeague,
            UpdateLeague updateLeague,
            DeleteLeague deleteLeague)
        {
            _getAllLeagues = getAllLeagues;
            _getLeagueById = getLeagueById;
            _addLeague = addLeague;
            _updateLeague = updateLeague;
            _deleteLeague = deleteLeague;
        }

        public async Task<object> Execute(LeagueActionDTO actionDTO)
        {
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllLeagues.ExecuteAsync();

                case "getbyid":
                    if (actionDTO.LeagueID == null)
                        throw new ArgumentException("El ID de la liga es necesario para obtenerla.");
                    return await _getLeagueById.ExecuteAsync(new LeagueID(actionDTO.LeagueID.Value));

                case "add":
                    if (actionDTO.League == null)
                        throw new ArgumentException("Los detalles de la liga son necesarios para agregarla.");
                    return await _addLeague.Execute(actionDTO.League);

                case "update":
                    if (actionDTO.League == null)
                        throw new ArgumentException("Los detalles de la liga son necesarios para actualizarla.");
                    return await _updateLeague.Execute(actionDTO.League, actionDTO.LeagueID.Value);

                case "delete":
                    if (actionDTO.LeagueID == null)
                        throw new ArgumentException("El ID de la liga es necesario para eliminarla.");
                    return await _deleteLeague.Execute(new LeagueID(actionDTO.LeagueID.Value));

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}
