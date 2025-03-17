using Application.Playes.DTOs;
using Application.Playes.UseCases.Create;
using Application.Playes.UseCases.Delete;
using Application.Playes.UseCases.Get;
using Application.Playes.UseCases.Update;
using Domain.Shared;

namespace Application.Playes.UseCases
{
    public class GeneralPlayerUseCaseHandler
    {
        private readonly GetAllPlayer _getAllPlayers;
        private readonly GetPlayerById _getPlayerById;
        private readonly CreatePlayer _addPlayer;
        private readonly UpdatePlayer _updatePlayer;
        private readonly DeletePlayer _deletePlayer;

        public GeneralPlayerUseCaseHandler(
            GetAllPlayer getAllPlayers,
            GetPlayerById getPlayerById,
            CreatePlayer addPlayer,
            UpdatePlayer updatePlayer,
            DeletePlayer deletePlayer)
        {
            _getAllPlayers = getAllPlayers;
            _getPlayerById = getPlayerById;
            _addPlayer = addPlayer;
            _updatePlayer = updatePlayer;
            _deletePlayer = deletePlayer;
        }

        public async Task<object> Execute(PlayerActionDTO actionDTO)
        {
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllPlayers.ExecuteAsync();

                case "getbyid":
                    if (actionDTO.PlayerID == null)
                        throw new ArgumentException("El ID del jugador es necesario para obtenerlo.");
                    return await _getPlayerById.ExecuteAsync(new PlayerID(actionDTO.PlayerID.Value));

                case "add":
                    if (actionDTO.Player == null)
                        throw new ArgumentException("Los detalles del jugador son necesarios para agregarlo.");
                    return await _addPlayer.Execute(actionDTO.Player);

                case "update":
                    if (actionDTO.Player == null)
                        throw new ArgumentException("Los detalles del jugador son necesarios para actualizarlo.");
                    return await _updatePlayer.Execute(actionDTO.Player, actionDTO.PlayerID.Value);

                case "delete":
                    if (actionDTO.PlayerID == null)
                        throw new ArgumentException("El ID del jugador es necesario para eliminarlo.");
                    return await _deletePlayer.Execute(new PlayerID(actionDTO.PlayerID.Value));

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}
