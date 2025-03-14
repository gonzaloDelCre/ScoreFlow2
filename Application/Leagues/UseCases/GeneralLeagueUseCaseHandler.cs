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
        private readonly AddLeague _addLeague;
        private readonly UpdateLeague _updateLeague;
        private readonly DeleteLeague _deleteLeague;

        public GeneralLeagueUseCaseHandler(
            GetAllLeagues getAllLeagues,
            GetLeagueById getLeagueById,
            AddLeague addLeague,
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
            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllLeagues.Execute();

                case "getbyid":
                    if (actionDTO.LeagueID == null)
                        throw new ArgumentException("El ID de la liga es necesario para obtenerla.");
                    var leagueId = new LeagueID(actionDTO.LeagueID.Value);
                    return await _getLeagueById.Execute(leagueId);

                case "add":
                    if (actionDTO.League == null)
                        throw new ArgumentException("Los detalles de la liga son necesarios para agregarla.");
                    var leagueRequestDTO = actionDTO.League;
                    var existingLeagueForAdd = await _getAllLeagues.Execute();
                    if (existingLeagueForAdd.Any(l => l.Name == leagueRequestDTO.Name))
                    {
                        // Si la liga ya existe, se actualiza en lugar de agregarla
                        return await _updateLeague.Execute(leagueRequestDTO);
                    }
                    return await _addLeague.Execute(leagueRequestDTO); // Si no existe, agregamos

                case "update":
                    if (actionDTO.League == null)
                        throw new ArgumentException("Los detalles de la liga son necesarios para actualizarla.");
                    var leagueUpdateDTO = actionDTO.League;
                    var existingLeagueForUpdate = await _getAllLeagues.Execute();
                    if (existingLeagueForUpdate.Any(l => l.Name == leagueUpdateDTO.Name))
                    {
                        return await _updateLeague.Execute(leagueUpdateDTO);
                    }
                    throw new ArgumentException("La liga a actualizar no existe.");

                case "delete":
                    if (actionDTO.LeagueID == null)
                        throw new ArgumentException("El ID de la liga es necesario para eliminarla.");
                    var deleteLeagueId = new LeagueID(actionDTO.LeagueID.Value);
                    return await _deleteLeague.Execute(deleteLeagueId);

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}
