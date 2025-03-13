using Application.Teams.DTOs;
using Domain.Entities.Players;
using Domain.Entities.Standings;
using Domain.Entities.TeamLeagues;
using Domain.Entities.Teams;
using Domain.Entities.Users;
using Domain.Shared;

namespace Application.Teams.Mapper
{
    public static class TeamMapper
    {
        // Transformar TeamDTO a Team (dominio)
        public static Team ToDomain(TeamDTO teamDTO, User? coach = null,
            ICollection<Player>? players = null,
            ICollection<TeamLeague>? teamLeagues = null,
            ICollection<Standing>? standings = null)
        {
            return new Team(
                new TeamID(teamDTO.TeamID),
                new TeamName(teamDTO.Name),
                coach,
                teamDTO.CreatedAt
            )
            {
                Players = players ?? new List<Player>(),
                TeamLeagues = teamLeagues ?? new List<TeamLeague>(),
                Standings = standings ?? new List<Standing>()
            };
        }

        // Transformar Team (dominio) a TeamDTO
        public static TeamDTO ToDTO(Team team)
        {
            return new TeamDTO
            {
                TeamID = team.TeamID.Value,
                Name = team.Name.Value,
                CoachID = team.Coach?.UserID.Value, 
                CreatedAt = team.CreatedAt,
                PlayerIDs = team.Players.Select(p => p.PlayerID).ToList(),
                TeamLeagueIDs = team.TeamLeagues.Select(tl => tl.TeamLeagueID.Value).ToList(),
                StandingIDs = team.Standings.Select(s => s.StandingID.Value).ToList()
            };
        }
    }
}
