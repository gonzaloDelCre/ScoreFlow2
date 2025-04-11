
using Application.Playes.DTOs;
using Application.TeamPlayers.Mappers;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Shared;

namespace Application.Playes.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerResponseDTO MapToResponseDTO(Player player)
        {
            return new PlayerResponseDTO
            {
                PlayerID = player.PlayerID,
                Name = player.Name.Value,
                Age = player.Age.Value,
                Position = player.Position,
                Goals = player.Goals,
                Photo = player.Photo,
                CreatedAt = player.CreatedAt,
                TeamPlayers = player.TeamPlayers.Select(tp => TeamPlayerMapper.ToResponseDTO(tp)).ToList(),
                //MatchEvents = player.MatchEvents.Select(me => MatchEventMapper.MapToResponseDTO(me)).ToList(),
                //PlayerStatistics = player.PlayerStatistics.Select(ps => PlayerStatisticMapper.MapToResponseDTO(ps)).ToList()
            };
        }

        public static Player MapToDomain(PlayerRequestDTO dto, List<TeamPlayer> teamPlayers)
        {
            return new Player(
                new PlayerID(0), 
                new PlayerName(dto.Name),
                dto.Position,
                new PlayerAge(dto.Age),
                dto.Goals,
                dto.Photo,
                dto.CreatedAt,
                teamPlayers
            );
        }
    }
}
