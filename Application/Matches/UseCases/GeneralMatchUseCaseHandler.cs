using Application.Matches.DTOs;
using Domain.Shared;
using Application.Matches.UseCases;
using Application.Matches.UseCases.Get;
using Application.Matches.UseCases.Create;
using Application.Matches.UseCases.Delete;
using Application.Matches.UseCases.Update;

namespace Application.Matches.UseCases
{
    public class GeneralMatchUseCaseHandler
    {
        private readonly GetAllMatches _getAllMatches;
        private readonly GetMatchById _getMatchById;
        private readonly CreateMatch _addMatch;
        private readonly UpdateMatch _updateMatch;
        private readonly DeleteMatch _deleteMatch;

        public GeneralMatchUseCaseHandler(
            GetAllMatches getAllMatches,
            GetMatchById getMatchById,
            CreateMatch addMatch,
            UpdateMatch updateMatch,
            DeleteMatch deleteMatch)
        {
            _getAllMatches = getAllMatches;
            _getMatchById = getMatchById;
            _addMatch = addMatch;
            _updateMatch = updateMatch;
            _deleteMatch = deleteMatch;
        }

        public async Task<object> Execute(MatchActionDTO actionDTO)
        {
            // Validación inicial de la acción
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllMatches.Execute();

                case "getbyid":
                    if (actionDTO.MatchID == null)
                        throw new ArgumentException("El ID del partido es necesario para obtenerlo.");

                    var matchId = new MatchID(actionDTO.MatchID.Value);
                    var match = await _getMatchById.Execute(matchId);

                    if (match == null)
                        throw new ArgumentException("El partido no existe.");

                    return match;

                case "add":
                    if (actionDTO.Match == null)
                        throw new ArgumentException("Los detalles del partido son necesarios para agregarlo.");

                    var matchRequestDTO = actionDTO.Match;
                    var existingMatchesForAdd = await _getAllMatches.Execute();

                    // Verificar si el partido ya existe
                    if (existingMatchesForAdd.Any(m => m.MatchID.Value == matchRequestDTO.MatchID))
                    {
                        // Si el partido ya existe, se actualiza en lugar de agregarlo
                        return await _updateMatch.Execute(matchRequestDTO);
                    }
                    return await _addMatch.Execute(matchRequestDTO); // Si no existe, lo agregamos

                case "update":
                    if (actionDTO.Match == null)
                        throw new ArgumentException("Los detalles del partido son necesarios para actualizarlo.");

                    var matchUpdateDTO = actionDTO.Match;
                    var existingMatchesForUpdate = await _getAllMatches.Execute();

                    // Verificar si el partido existe para actualizar
                    if (existingMatchesForUpdate.Any(m => m.MatchID.Value == matchUpdateDTO.MatchID))
                    {
                        return await _updateMatch.Execute(matchUpdateDTO);
                    }
                    throw new ArgumentException("El partido a actualizar no existe.");

                case "delete":
                    if (actionDTO.MatchID == null)
                        throw new ArgumentException("El ID del partido es necesario para eliminarlo.");

                    var deleteMatchId = new MatchID(actionDTO.MatchID.Value);
                    var existingMatchForDelete = await _getMatchById.Execute(deleteMatchId);

                    if (existingMatchForDelete == null)
                        throw new ArgumentException("El partido a eliminar no existe.");

                    return await _deleteMatch.Execute(deleteMatchId);

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}