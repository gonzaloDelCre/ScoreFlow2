using Application.Matches.DTOs;
using Domain.Entities.Leagues;
using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Ports.Teams;
using Domain.Shared;
using System;

namespace Application.Matches.Mapper
{
    public static class MatchMapper
    {
        public static MatchResponseDTO ToDTO(this Match m)
            => new MatchResponseDTO
            {
                ID = m.MatchID.Value,
                Team1ID = m.Team1.TeamID.Value,
                Team1Name = m.Team1.Name.Value,
                Team2ID = m.Team2.TeamID.Value,
                Team2Name = m.Team2.Name.Value,
                LeagueID = m.League.LeagueID.Value,    
                LeagueName = m.League.Name.Value,        
                Jornada = m.Jornada,                  
                MatchDate = m.MatchDate,
                Status = m.Status,
                Location = m.Location,
                ScoreTeam1 = m.ScoreTeam1,
                ScoreTeam2 = m.ScoreTeam2,
                CreatedAt = m.CreatedAt
            };

        public static Match ToDomain(
            this MatchRequestDTO dto,
            Team team1,
            Team team2,
            League league)                        
        {
            var match = new Match(
                matchID: dto.ID.HasValue ? new MatchID(dto.ID.Value) : new MatchID(1),
                team1: team1,
                team2: team2,
                matchDate: dto.MatchDate,
                status: dto.Status,
                location: dto.Location,
                league: league,                 
                jornada: dto.Jornada               
            );

            match.UpdateScore(dto.ScoreTeam1, dto.ScoreTeam2);
            return match;
        }
    }
}
