using Application.Standings.DTOs;
using Domain.Entities.Standings;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Standings.Mapper
{
    public class StandingMapper
    {
        public StandingResponseDTO MapToDTO(Standing s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            return new StandingResponseDTO
            {
                StandingID = s.StandingID.Value,
                LeagueID = s.LeagueID.Value,
                TeamID = s.TeamID.Value,
                Wins = s.Wins.Value,
                Draws = s.Draws.Value,
                Losses = s.Losses.Value,
                GoalsFor = s.GoalsFor.Value,
                GoalsAgainst = s.GoalsAgainst.Value,
                Points = s.Points.Value,
                CreatedAt = s.CreatedAt
            };
        }

        public Standing MapToDomain(StandingRequestDTO dto, Standing? existing = null)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // Si existe, actualizamos
            if (existing != null)
            {
                existing.Update(
                    new Wins(dto.Wins),
                    new Draws(dto.Draws),
                    new Losses(dto.Losses),
                    new GoalsFor(dto.GoalsFor),
                    new GoalsAgainst(dto.GoalsAgainst),
                    new Points(dto.Wins * 2 + dto.Draws) // o regla de puntos que uses
                );
                return existing;
            }

            // Nuevo
            return new Standing(
                standingID: new StandingID(dto.StandingID),
                leagueID: new LeagueID(dto.LeagueID),
                teamID: new TeamID(dto.TeamID),
                wins: new Wins(dto.Wins),
                draws: new Draws(dto.Draws),
                losses: new Losses(dto.Losses),
                goalsFor: new GoalsFor(dto.GoalsFor),
                goalsAgainst: new GoalsAgainst(dto.GoalsAgainst),
                points: new Points(dto.Wins * 2 + dto.Draws),
                league: null,
                team: null,
                createdAt: DateTime.UtcNow
            );
        }
    }
}
