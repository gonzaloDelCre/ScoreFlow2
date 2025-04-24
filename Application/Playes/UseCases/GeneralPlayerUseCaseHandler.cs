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
        private readonly GetPlayerById _getPlayerById;
        private readonly CreatePlayer _createPlayer;
        private readonly UpdatePlayer _updatePlayer;
        private readonly DeletePlayer _deletePlayer;
        private readonly GetAllPlayer _getAllPlayer;
        private readonly CreatePlayersFromScraperUseCase _scraperUseCase;

        public GeneralPlayerUseCaseHandler(
            GetPlayerById getPlayerById,
            CreatePlayer createPlayer,
            UpdatePlayer updatePlayer,
            DeletePlayer deletePlayer,
            GetAllPlayer getAllPlayer,
            CreatePlayersFromScraperUseCase scraperUseCase)
        {
            _getPlayerById = getPlayerById;
            _createPlayer = createPlayer;
            _updatePlayer = updatePlayer;
            _deletePlayer = deletePlayer;
            _getAllPlayer = getAllPlayer;
            _scraperUseCase = scraperUseCase;
        }

        public async Task<IEnumerable<PlayerResponseDTO>> GetAllPlayersAsync()
        {
            return await _getAllPlayer.ExecuteAsync();
        }

        public async Task<PlayerResponseDTO> GetPlayerByIdAsync(int playerId)
        {
            return await _getPlayerById.ExecuteAsync(new PlayerID(playerId));
        }

        public async Task<PlayerResponseDTO> CreatePlayerAsync(PlayerRequestDTO playerDTO)
        {
            return await _createPlayer.Execute(playerDTO);
        }

        public async Task<PlayerResponseDTO> UpdatePlayerAsync(int playerId, PlayerRequestDTO playerDTO)
        {
            return await _updatePlayer.Execute(playerDTO, playerId);
        }

        public async Task DeletePlayerAsync(int playerId)
        {
            await _deletePlayer.Execute(new PlayerID(playerId));
        }
        public async Task ScrapeAsync(int teamId)
        {
            await _scraperUseCase.ExecuteAsync(teamId);
        }
    }
}
