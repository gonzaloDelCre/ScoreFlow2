using Application.Playes.DTOs;
using Application.Playes.Mappers;
using Application.TeamPlayers.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Shared;

namespace Application.Playes.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerResponseDTO ToDTO(this Player p)
            => new PlayerResponseDTO
            {
                ID = p.PlayerID.Value,
                Name = p.Name.Value,
                Position = p.Position,
                Age = p.Age.Value,
                Goals = p.Goals,
                Photo = p.Photo,
                CreatedAt = p.CreatedAt
            };

        public static Player ToDomain(this PlayerRequestDTO dto)
            => new Player(
                playerID: new PlayerID(dto.ID ?? 0),
                name: new PlayerName(dto.Name),
                position: dto.Position,
                age: new PlayerAge(dto.Age),
                goals: dto.Goals,
                photo: dto.Photo,
                createdAt: null,
                teamPlayers: null
            );
    }
}
