using Domain.Services.Players;
using Domain.Shared;

namespace Application.Playes.UseCases.Delete
{
    public class DeletePlayer
    {
        private readonly PlayerService _playerService;

        public DeletePlayer(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<bool> Execute(PlayerID playerId)
        {
            if (playerId == null)
                throw new ArgumentNullException(nameof(playerId), "El ID del jugador no puede ser nulo.");

            var player = await _playerService.GetPlayerByIdAsync(playerId);
            if (player == null)
                throw new InvalidOperationException("El jugador no existe. No se puede eliminar.");

            return await _playerService.DeletePlayerAsync(playerId);
        }
    }
}
