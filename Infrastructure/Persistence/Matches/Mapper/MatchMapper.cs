using Domain.Entities.Leagues;
using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Matches.Entities;
using Infrastructure.Persistence.MatchEvents.Mapper;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.PlayerStatistics.Mapper;
using Infrastructure.Persistence.Teams.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Matches.Mapper
{
    public class MatchMapper : IMatchMapper
    {
        private readonly ITeamMapper _teamMapper;
        private readonly IMatchEventMapper _eventMapper;
        private readonly IPlayerStatisticMapper _statMapper;
        private readonly ILeagueMapper _leagueMapper;    // <--- nuevo

        public MatchMapper(
            ITeamMapper teamMapper,
            IMatchEventMapper eventMapper,
            IPlayerStatisticMapper statMapper,
            ILeagueMapper leagueMapper)           // <--- nuevo
        {
            _teamMapper = teamMapper ?? throw new ArgumentNullException(nameof(teamMapper));
            _eventMapper = eventMapper ?? throw new ArgumentNullException(nameof(eventMapper));
            _statMapper = statMapper ?? throw new ArgumentNullException(nameof(statMapper));
            _leagueMapper = leagueMapper ?? throw new ArgumentNullException(nameof(leagueMapper));
        }

        public MatchEntity ToEntity(Match domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new MatchEntity
            {
                ID = domain.MatchID.Value,
                Team1ID = domain.Team1.TeamID.Value,
                Team2ID = domain.Team2.TeamID.Value,
                LeagueID = domain.League.LeagueID.Value,   // <--- liga
                Jornada = domain.Jornada,                 // <--- jornada
                DateTime = domain.MatchDate,
                ScoreTeam1 = domain.ScoreTeam1,
                ScoreTeam2 = domain.ScoreTeam2,
                Status = domain.Status,
                Location = domain.Location,
                CreatedAt = domain.CreatedAt
            };
        }

        public Match ToDomain(MatchEntity e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            // 1) Mapear equipos
            var team1 = _teamMapper.ToDomain(
                e.Team1,
                new List<Domain.Entities.Players.Player>(),
                new List<Domain.Entities.Standings.Standing>()
            );
            var team2 = _teamMapper.ToDomain(
                e.Team2,
                new List<Domain.Entities.Players.Player>(),
                new List<Domain.Entities.Standings.Standing>()
            );

            // 2) Mapear liga
            var league = _leagueMapper.MapToDomain(e.League);

            // 3) Construir dominio (asumiendo que has añadido este constructor
            //    en Domain.Entities.Matches.Match para recibir league y jornada)
            var match = new Match(
                new MatchID(e.ID),
                team1,
                team2,
                e.DateTime,
                e.Status,
                e.Location,
                league,        
                e.Jornada       
            );

            // 4) Asignar marcador
            match.UpdateScore(e.ScoreTeam1, e.ScoreTeam2);

            // 5) Mapear eventos
            foreach (var evEntity in e.MatchEvents)
            {
                var player = evEntity.Player != null
                    ? new PlayerMapper()
                        .ToDomain(evEntity.Player, Enumerable.Empty<Infrastructure.Persistence.TeamPlayers.Entities.TeamPlayerEntity>())
                    : null;

                var ev = _eventMapper.MapToDomain(evEntity, match, player);
                match.AddMatchEvent(ev);
            }

            // 6) Mapear estadísticas
            foreach (var statEntity in e.PlayerStatistics)
            {
                var player = new PlayerMapper()
                    .ToDomain(statEntity.Player, Enumerable.Empty<Infrastructure.Persistence.TeamPlayers.Entities.TeamPlayerEntity>());

                var stat = _statMapper.MapToDomain(statEntity, player, match);
                match.AddPlayerStatistic(stat);
            }

            return match;
        }
    }
}


