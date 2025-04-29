using Application.TeamPlayers.DTOs;
using Domain.Entities.TeamPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TeamPlayers.Mappers
{
    public static class TeamRosterMapper
    {
        public static TeamRosterDto ToRosterDto(this IEnumerable<TeamPlayer> rels)
        {
            var first = rels.FirstOrDefault();
            if (first == null)
            {
                return new TeamRosterDto
                {
                    TeamId = 0,
                    TeamName = string.Empty,
                    Logo = null,
                    Players = new List<TeamPlayerSimpleDto>()
                };
            }

            return new TeamRosterDto
            {
                TeamId = first.TeamID.Value,
                TeamName = first.Team!.Name.Value,
                Logo = first.Team.Logo,
                Players = rels.Select(tp => new TeamPlayerSimpleDto
                {
                    PlayerId = tp.PlayerID.Value,
                    PlayerName = tp.Player!.Name.Value,
                    Position = tp.Player.Position.ToString(),
                    Age = tp.Player.Age.Value,
                    Goals = tp.Player.Goals,
                    Photo = tp.Player.Photo
                }).ToList()
            };
        }
    }
}
