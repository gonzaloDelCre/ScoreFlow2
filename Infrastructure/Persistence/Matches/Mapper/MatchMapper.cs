using Domain.Entities.Matches;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
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

        public MatchMapper(
            ITeamMapper teamMapper,
            IMatchEventMapper eventMapper,
            IPlayerStatisticMapper statMapper)
        {
            _teamMapper = teamMapper ?? throw new ArgumentNullException(nameof(teamMapper));
            _eventMapper = eventMapper ?? throw new ArgumentNullException(nameof(eventMapper));
            _statMapper = statMapper ?? throw new ArgumentNullException(nameof(statMapper));
        }

        public MatchEntity ToEntity(Match domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new MatchEntity
            {
                ID = domain.MatchID.Value,
                Team1ID = domain.Team1.TeamID.Value,
                Team2ID = domain.Team2.TeamID.Value,
                DateTime = domain.MatchDate,
                ScoreTeam1 = 0,
                ScoreTeam2 = 0,
                Status = domain.Status,
                Location = domain.Location,
                CreatedAt = domain.CreatedAt
            };
        }

        public Match ToDomain(MatchEntity e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var team1 = _teamMapper.ToDomain(e.Team1, Enumerable.Empty<Domain.Entities.Players.Player>(), Enumerable.Empty<Domain.Entities.Standings.Standing>());
            var team2 = _teamMapper.ToDomain(e.Team2, Enumerable.Empty<Domain.Entities.Players.Player>(), Enumerable.Empty<Domain.Entities.Standings.Standing>());

            var domain = new Match(
                new MatchID(e.ID),
                team1,
                team2,
                e.DateTime,
                e.Status,
                e.Location
            );

            foreach (var evEntity in e.MatchEvents)
            {
                var player = evEntity.Player != null
                    ? new PlayerMapper().ToDomain(evEntity.Player, Enumerable.Empty<Infrastructure.Persistence.TeamPlayers.Entities.TeamPlayerEntity>())
                    : null;

                var ev = _eventMapper.MapToDomain(evEntity, domain, player);
                domain.AddMatchEvent(ev);
            }

            foreach (var statEntity in e.PlayerStatistics)
            {
                var player = new PlayerMapper().ToDomain(statEntity.Player, Enumerable.Empty<Infrastructure.Persistence.TeamPlayers.Entities.TeamPlayerEntity>());
                var stat = _statMapper.MapToDomain(statEntity, player, domain);
                domain.AddPlayerStatistic(stat);
            }

            return domain;
        }
    }
}


