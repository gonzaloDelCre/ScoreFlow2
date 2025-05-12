using System;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Shared;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.TeamPlayers.Mapper;
using Infrastructure.Persistence.Teams.Mapper;

namespace Infrastructure.Persistence.TeamPlayers.Mappers
{
    public class TeamPlayerMapper : ITeamPlayerMapper
    {
        private readonly IPlayerMapper _playerMapper;
        private readonly ITeamMapper _teamMapper;

        public TeamPlayerMapper(IPlayerMapper playerMapper, ITeamMapper teamMapper)
        {
            _playerMapper = playerMapper ?? throw new ArgumentNullException(nameof(playerMapper));
            _teamMapper = teamMapper ?? throw new ArgumentNullException(nameof(teamMapper));
        }

        public TeamPlayerEntity MapToEntity(TeamPlayer domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));

            return new TeamPlayerEntity
            {
                ID = domain.ID.Value,
                TeamID = domain.TeamID.Value,
                PlayerID = domain.PlayerID.Value,
                RoleInTeam = domain.RoleInTeam ?? default,
                JoinedAt = domain.JoinedAt.Value
            };
        }

        public TeamPlayer MapToDomain(TeamPlayerEntity e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var player = _playerMapper.ToDomain(
                e.Player,
                Enumerable.Empty<Infrastructure.Persistence.TeamPlayers.Entities.TeamPlayerEntity>()
            );


            var team = _teamMapper.ToDomain(
                e.Team,
                Enumerable.Empty<Domain.Entities.Players.Player>(),
                Enumerable.Empty<Domain.Entities.Standings.Standing>()
            );

            return new TeamPlayer(
                new TeamPlayerID(e.ID),
                new TeamID(e.TeamID),
                new PlayerID(e.PlayerID),
                new JoinedAt(e.JoinedAt),
                e.RoleInTeam,
                team,
                player
            );
        }

    }
}
