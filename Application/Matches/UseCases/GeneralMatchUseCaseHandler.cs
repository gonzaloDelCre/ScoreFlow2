//using Application.Matches.DTOs;
//using Domain.Shared;
//using Application.Matches.UseCases;
//using Application.Matches.UseCases.Get;
//using Application.Matches.UseCases.Create;
//using Application.Matches.UseCases.Delete;
//using Application.Matches.UseCases.Update;

//namespace Application.Matches.UseCases
//{
//    public class GeneralMatchUseCaseHandler
//    {
//        private readonly GetAllMatches _getAllMatches;
//        private readonly GetMatchById _getMatchById;
//        private readonly CreateMatch _addMatch;
//        private readonly UpdateMatch _updateMatch;
//        private readonly DeleteMatch _deleteMatch;

//        public GeneralMatchUseCaseHandler(
//            GetAllMatches getAllMatches,
//            GetMatchById getMatchById,
//            CreateMatch addMatch,
//            UpdateMatch updateMatch,
//            DeleteMatch deleteMatch)
//        {
//            _getAllMatches = getAllMatches;
//            _getMatchById = getMatchById;
//            _addMatch = addMatch;
//            _updateMatch = updateMatch;
//            _deleteMatch = deleteMatch;
//        }

//        public async Task<object> Execute(MatchActionDTO actionDTO)
//        {
//            if (string.IsNullOrWhiteSpace(actionDTO.Action))
//                throw new ArgumentException("La acción no puede estar vacía.");

//            switch (actionDTO.Action.ToLower())
//            {
//                case "getall":
//                    return await _getAllMatches.ExecuteAsync();

//                case "getbyid":
//                    if (actionDTO.MatchID == null)
//                        throw new ArgumentException("El ID del partido es necesario para obtenerlo.");
//                    return await _getMatchById.ExecuteAsync(new MatchID(actionDTO.MatchID.Value));

//                case "add":
//                    if (actionDTO.Match == null)
//                        throw new ArgumentException("Los detalles del partido son necesarios para agregarlo.");
//                    return await _addMatch.Execute(actionDTO.Match);

//                case "update":
//                    if (actionDTO.Match == null)
//                        throw new ArgumentException("Los detalles del partido son necesarios para actualizarlo.");
//                    return await _updateMatch.Execute(actionDTO.Match);

//                case "delete":
//                    if (actionDTO.MatchID == null)
//                        throw new ArgumentException("El ID del partido es necesario para eliminarlo.");
//                    return await _deleteMatch.Execute(new MatchID(actionDTO.MatchID.Value));

//                default:
//                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
//            }
//        }
//    }
//}
