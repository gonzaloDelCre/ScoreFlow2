using Application.TeamPlayers.DTOs;
using System.Threading.Tasks;
using Application.TeamPlayers.Mappers;
using Domain.Entities.TeamPlayers;
using Domain.Ports.TeamPlayers;
using Domain.Shared;

namespace Application.TeamPlayers.UseCases.Update
{
    public class UpdateTeamPlayerUseCase
    {
        private readonly ITeamPlayerRepository _repo;
        public UpdateTeamPlayerUseCase(ITeamPlayerRepository repo) => _repo = repo;

        public async Task<TeamPlayerResponseDTO?> ExecuteAsync(TeamPlayerRequestDTO dto)
        {
            if (!dto.ID.HasValue)
                throw new ArgumentException("El ID es obligatorio para actualizar TeamPlayer");

            var existing = await _repo.GetByIdsAsync(
                new TeamID(dto.TeamID),
                new PlayerID(dto.PlayerID)
            );
            if (existing == null) return null;

            existing.UpdateJoinedAt(new JoinedAt(dto.JoinedAt));
            existing.UpdateRole(dto.RoleInTeam);

            await _repo.UpdateAsync(existing);
            return existing.ToDTO();
        }
    }
}
