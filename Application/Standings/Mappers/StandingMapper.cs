using Application.Standings.DTOs;
using Domain.Entities.Standings;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.Mappers
{
    public static class StandingMapper
    {
        public static StandingResponseDTO ToDTO(this Standing s)
            => new StandingResponseDTO
            {
                ID = s.StandingID.Value,
                LeagueID = s.LeagueID.Value,
                LeagueName = s.League.Name.Value,
                TeamID = s.TeamID.Value,
                TeamName = s.Team.Name.Value,
                Points = s.Points.Value,
                MatchesPlayed = s.MatchesPlayed.Value,
                Wins = s.Wins.Value,
                Draws = s.Draws.Value,
                Losses = s.Losses.Value,
                GoalDifference = s.GoalDifference.Value,
                CreatedAt = s.CreatedAt
            };

        public static Standing ToDomain(this StandingRequestDTO dto)
            => new Standing(
                standingID: new StandingID(1),
                leagueID: new LeagueID(dto.LeagueID),
                teamID: new TeamID(dto.TeamID),
                points: new Points(dto.Points),
                matchesPlayed: new MatchesPlayed(dto.MatchesPlayed),
                wins: new Wins(dto.Wins),
                draws: new Draws(dto.Draws),
                losses: new Losses(dto.Losses),
                goalDifference: new GoalDifference(dto.GoalDifference),
                league: null!,
                team: null!,
                createdAt: null
            );
    }
}
