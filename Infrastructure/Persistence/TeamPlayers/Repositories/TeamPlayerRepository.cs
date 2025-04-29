// Infrastructure/Persistence/TeamPlayers/Repositories/TeamPlayerRepository.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Entities.Teams;
using Domain.Enum;
using Domain.Shared;
using Domain.Ports.TeamPlayers;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.TeamPlayers.Mappers;

namespace Infrastructure.Persistence.TeamPlayers.Repositories
{
    public class TeamPlayerRepository : ITeamPlayerRepository
    {
        private readonly ApplicationDbContext _ctx;

        public TeamPlayerRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(TeamID teamId, PlayerID playerId, DateTime? joinedAt = null, RoleInTeam? role = null)
        {
            // Obtener el jugador para poder extraer su posición
            var player = await _ctx.Players.FirstOrDefaultAsync(p => p.PlayerID == playerId.Value);
            if (player == null)
                throw new InvalidOperationException("Jugador no encontrado");

            // Convertir posición de string a RoleInTeam (si coincide)
            RoleInTeam inferredRole;
            var parsed = Enum.TryParse<RoleInTeam>(player.Position.Replace(" ", "").ToUpper(), out inferredRole);

            var entity = new TeamPlayerEntity
            {
                TeamID = teamId.Value,
                PlayerID = playerId.Value,
                JoinedAt = joinedAt ?? DateTime.UtcNow,
                RoleInTeam = role ?? (parsed ? inferredRole : default)
            };

            _ctx.TeamPlayers.Add(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<TeamPlayer> AddAsync(TeamPlayer teamPlayer)
        {
            if (teamPlayer == null)
                throw new ArgumentNullException(nameof(teamPlayer));

            var entity = new TeamPlayerEntity
            {
                TeamID = teamPlayer.TeamID.Value,
                PlayerID = teamPlayer.PlayerID.Value,
                JoinedAt = teamPlayer.JoinedAt,
                RoleInTeam = teamPlayer.RoleInTeam ?? default
            };

            _ctx.TeamPlayers.Add(entity);
            await _ctx.SaveChangesAsync();

            // Devuelve el TeamPlayer mapeado a dominio después de que se haya guardado
            return new TeamPlayer(
                teamPlayer.TeamID,
                teamPlayer.PlayerID,
                entity.JoinedAt,
                entity.RoleInTeam,
                teamPlayer.Team,
                teamPlayer.Player
            );
        }


        public async Task<IEnumerable<TeamPlayer>> GetAllAsync()
        {
            var ents = await _ctx.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .ToListAsync();

            return ents.Select(TeamPlayerMapper.MapEntityToDomain);
        }

        public async Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId)
        {
            var ent = await _ctx.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .FirstOrDefaultAsync(tp => tp.TeamID == teamId.Value && tp.PlayerID == playerId.Value);

            return ent == null ? null : TeamPlayerMapper.MapEntityToDomain(ent);
        }

        public async Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId)
        {
            var ents = await _ctx.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .Where(tp => tp.TeamID == teamId.Value)
                .ToListAsync();

            return ents.Select(TeamPlayerMapper.MapEntityToDomain);
        }

        public async Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId)
        {
            var ents = await _ctx.TeamPlayers
                .Include(tp => tp.Team)
                .Include(tp => tp.Player)
                .Where(tp => tp.PlayerID == playerId.Value)
                .ToListAsync();

            return ents.Select(TeamPlayerMapper.MapEntityToDomain);
        }

        public async Task UpdateAsync(TeamPlayer teamPlayer)
        {
            var ent = await _ctx.TeamPlayers
                .FirstOrDefaultAsync(tp => tp.TeamID == teamPlayer.TeamID.Value
                                        && tp.PlayerID == teamPlayer.PlayerID.Value);
            if (ent == null) throw new InvalidOperationException("Relación no encontrada");

            if (teamPlayer.RoleInTeam.HasValue)
                ent.RoleInTeam = teamPlayer.RoleInTeam.Value;

            ent.JoinedAt = teamPlayer.JoinedAt;

            _ctx.TeamPlayers.Update(ent);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId)
        {
            var ent = await _ctx.TeamPlayers
                .FirstOrDefaultAsync(tp => tp.TeamID == teamId.Value && tp.PlayerID == playerId.Value);
            if (ent == null) return false;

            _ctx.TeamPlayers.Remove(ent);
            await _ctx.SaveChangesAsync();
            return true;
        }

        
    }
}
