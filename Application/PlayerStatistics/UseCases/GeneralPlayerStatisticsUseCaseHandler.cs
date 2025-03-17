using Domain.Shared;
using Application.PlayerStatistics.DTOs;
using Application.PlayerStatistics.UseCases.Get;
using Application.PlayerStatistics.UseCases.Update;
using Application.PlayerStatistics.UseCases.Delete;
using Application.PlayerStatistics.UseCases.Create;

namespace Application.PlayerStatistics.UseCases
{
    public class GeneralPlayerStatisticsUseCaseHandler
    {
        private readonly GetAllPlayerStatistics _getAllPlayerStatistics;
        private readonly GetPlayerStatisticById _getPlayerStatisticById;
        private readonly CreatePlayerStatistic _createPlayerStatistic;
        private readonly UpdatePlayerStatistic _updatePlayerStatistic;
        private readonly DeletePlayerStatistic _deletePlayerStatistic;

        public GeneralPlayerStatisticsUseCaseHandler(
            GetAllPlayerStatistics getAllPlayerStatistics,
            GetPlayerStatisticById getPlayerStatisticById,
            CreatePlayerStatistic createPlayerStatistic,
            UpdatePlayerStatistic updatePlayerStatistic,
            DeletePlayerStatistic deletePlayerStatistic)
        {
            _getAllPlayerStatistics = getAllPlayerStatistics;
            _getPlayerStatisticById = getPlayerStatisticById;
            _createPlayerStatistic = createPlayerStatistic;
            _updatePlayerStatistic = updatePlayerStatistic;
            _deletePlayerStatistic = deletePlayerStatistic;
        }

        public async Task<object> Execute(PlayerStatisticActionDTO actionDTO)
        {
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllPlayerStatistics.ExecuteAsync();

                case "getbyid":
                    if (actionDTO.PlayerStatisticID == null)
                        throw new ArgumentException("El ID de las estadísticas del jugador es necesario para obtenerlas.");
                    return await _getPlayerStatisticById.ExecuteAsync(actionDTO.PlayerStatisticID);

                case "add":
                    if (actionDTO.PlayerStatistic == null)
                        throw new ArgumentException("Los detalles de las estadísticas del jugador son necesarios para agregar.");
                    return await _createPlayerStatistic.ExecuteAsync(actionDTO.PlayerStatistic);

                case "update":
                    if (actionDTO.PlayerStatistic == null)
                        throw new ArgumentException("Los detalles de las estadísticas del jugador son necesarios para actualizar.");
                    if (actionDTO.PlayerStatisticID == null)
                        throw new ArgumentException("El ID de las estadísticas del jugador es necesario para actualizar.");
                    return await _updatePlayerStatistic.ExecuteAsync(actionDTO.PlayerStatistic, actionDTO.PlayerStatisticID);

                case "delete":
                    if (actionDTO.PlayerStatisticID == null)
                        throw new ArgumentException("El ID de las estadísticas del jugador es necesario para eliminar.");
                    return await _deletePlayerStatistic.ExecuteAsync(actionDTO.PlayerStatisticID);

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }
    }
}
