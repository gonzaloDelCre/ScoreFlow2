using Application.Playes.DTOs;
using Domain.Ports.Players;
using Domain.Ports.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Get
{
    public class GetPlayersByTeamUseCase
    {
        private readonly ITeamRepository _teamRepo;
        private readonly IPlayerRepository _playerRepo;

        public GetPlayersByTeamUseCase(ITeamRepository teamRepo, IPlayerRepository playerRepo)
        {
            _teamRepo = teamRepo;
            _playerRepo = playerRepo;
        }

        public async Task<IEnumerable<PlayerResponseDTO>> ExecuteAsync(int teamExternalId)
        {
            // 1) Resolver Team interno
            var team = await _teamRepo.GetByExternalIdAsync(teamExternalId.ToString());
            if (team == null)
                return Enumerable.Empty<PlayerResponseDTO>();

            // 2) Obtener jugadores de ese equipo
            var players = await _playerRepo.GetByTeamIdAsync(team.TeamID);

            // 3) Mapear a DTO
            return players.Select(p => new PlayerResponseDTO
            {
                PlayerID = p.PlayerID,
                Name = p.Name.Value,
                Position = p.Position,
                Age = p.Age.Value,
                Goals = p.Goals,
                Photo = p.Photo,
                CreatedAt = p.CreatedAt
            });
        }
    }
}
