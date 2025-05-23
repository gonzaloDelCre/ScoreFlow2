﻿using Application.Teams.DTOs;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Shared;

namespace Application.Teams.Mapper
{
    public class TeamMapper
    {
        public TeamResponseDTO MapToDTO(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "La entidad de dominio Team no puede ser nula.");

            return new TeamResponseDTO
            {
                TeamID = team.TeamID.Value,
                TeamName = team.Name.Value,
                LogoUrl = team.Logo,
                PlayerIds = team.Players?.Select(p => p.PlayerID.Value).ToList() ?? new List<int>(),
                Category = team.Category,
                Club = team.Club,
                Stadium = team.Stadium
            };
        }

        public Team MapToDomain(TeamRequestDTO teamDTO, Team? existingTeam = null)
        {
            if (teamDTO == null)
                throw new ArgumentNullException(nameof(teamDTO), "El DTO TeamRequestDTO no puede ser nulo.");

            var players = teamDTO.PlayerIds?.Select(id => new Player(new PlayerID(id))).ToList() ?? new List<Player>();

            if (existingTeam != null)
            {
                existingTeam.Update(
                    new TeamName(teamDTO.Name),
                    teamDTO.Logo,
                    teamDTO.Category,
                    teamDTO.Club,
                    teamDTO.Stadium
                );
                return existingTeam;
            }

            var team = new Team(
                new TeamID(0),
                new TeamName(teamDTO.Name),
                DateTime.UtcNow,
                teamDTO.Logo
            );

            team.SetCategory(teamDTO.Category);
            team.SetClub(teamDTO.Club);
            team.SetStadium(teamDTO.Stadium);

            return team;

        }
    }
}
