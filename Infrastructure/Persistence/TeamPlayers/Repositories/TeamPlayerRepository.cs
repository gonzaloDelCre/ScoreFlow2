using Domain.Entities.TeamPlayers;
using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Ports.TeamPlayers;
using Domain.Enum;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.TeamPlayers.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.TeamPlayers.Repositories
{
    public class TeamPlayerRepository : ITeamPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamPlayerRepository> _logger;

        public TeamPlayerRepository(ApplicationDbContext context, ILogger<TeamPlayerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TeamPlayer>> GetAllAsync()
        {
            try
            {
                var query = @"
                    SELECT tp.Id, tp.TeamID, tp.PlayerID, tp.JoinedAt, tp.RoleInTeam,
                           t.Name AS TeamName, p.Name AS PlayerName
                    FROM TeamPlayers tp
                    INNER JOIN Teams t ON tp.TeamID = t.TeamID
                    INNER JOIN Players p ON tp.PlayerID = p.PlayerID";

                var teamPlayers = new List<TeamPlayer>();

                await using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    if (command.Connection.State != System.Data.ConnectionState.Open)
                        await command.Connection.OpenAsync();

                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Creamos instancias mínimas de Team y Player con valores por defecto para el mapeo
                            var team = new Team(
                                new TeamID(reader.GetInt32(reader.GetOrdinal("TeamID"))),
                                new TeamName(reader.GetString(reader.GetOrdinal("TeamName"))),
                                DateTime.UtcNow,
                                string.Empty
                            );
                            var player = new Player(
                                new PlayerID(reader.GetInt32(reader.GetOrdinal("PlayerID"))),
                                new PlayerName(reader.GetString(reader.GetOrdinal("PlayerName"))),
                                PlayerPosition.LD,
                                new PlayerAge(0),
                                0,
                                null,
                                DateTime.UtcNow,
                                new List<TeamPlayer>()
                            );

                            RoleInTeam? role = null;
                            int roleOrdinal = reader.GetOrdinal("RoleInTeam");
                            if (!reader.IsDBNull(roleOrdinal))
                            {
                                int roleValue = reader.GetInt32(roleOrdinal);
                                role = (RoleInTeam)roleValue;
                            }

                            var teamPlayer = new TeamPlayer(
                                new TeamID(reader.GetInt32(reader.GetOrdinal("TeamID"))),
                                new PlayerID(reader.GetInt32(reader.GetOrdinal("PlayerID"))),
                                reader.GetDateTime(reader.GetOrdinal("JoinedAt")),
                                role,
                                team,
                                player
                            );
                            teamPlayers.Add(teamPlayer);
                        }
                    }
                }

                return teamPlayers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de TeamPlayers");
                return new List<TeamPlayer>();
            }
        }

        public async Task<TeamPlayer?> GetByIdsAsync(TeamID teamId, PlayerID playerId)
        {
            try
            {
                var query = @"
                    SELECT * 
                    FROM TeamPlayers
                    WHERE TeamID = @teamId AND PlayerID = @playerId";

                var parameters = new[]
                {
                    new SqlParameter("@teamId", teamId.Value),
                    new SqlParameter("@playerId", playerId.Value)
                };

                var entity = await _context.TeamPlayers.FromSqlRaw(query, parameters).FirstOrDefaultAsync();

                if (entity == null)
                    return null;

                var teamEntity = await _context.Teams.FindAsync(entity.TeamID);
                var playerEntity = await _context.Players.FindAsync(entity.PlayerID);

                if (teamEntity == null || playerEntity == null)
                    return null;

                // Get all related team players for proper mapping
                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                // Convert entities to domain objects using your mappers
                var team = Infrastructure.Persistence.Teams.Mapper.TeamMapper.MapToDomain(teamEntity, allTeamPlayers, allPlayers);

                var playerMapper = new Infrastructure.Persistence.Players.Mapper.PlayerMapper();
                var player = playerMapper.MapToDomain(playerEntity, allTeamPlayers);

                return TeamPlayerMapper.MapToDomain(entity, team, player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la relación de TeamPlayer");
                return null;
            }
        }

        public async Task<IEnumerable<TeamPlayer>> GetByTeamIdAsync(TeamID teamId)
        {
            try
            {
                var query = @"
                    SELECT * 
                    FROM TeamPlayers
                    WHERE TeamID = @teamId";

                var parameter = new SqlParameter("@teamId", teamId.Value);
                var entities = await _context.TeamPlayers.FromSqlRaw(query, parameter).ToListAsync();

                if (!entities.Any())
                    return new List<TeamPlayer>();

                // Load all related data for mapping
                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();
                var teamEntity = await _context.Teams.FindAsync(teamId.Value);

                if (teamEntity == null)
                    return new List<TeamPlayer>();

                // Map the team entity to domain
                var team = Infrastructure.Persistence.Teams.Mapper.TeamMapper.MapToDomain(teamEntity, allTeamPlayers, allPlayers);

                var result = new List<TeamPlayer>();
                var playerMapper = new Infrastructure.Persistence.Players.Mapper.PlayerMapper();

                foreach (var entity in entities)
                {
                    var playerEntity = await _context.Players.FindAsync(entity.PlayerID);
                    if (playerEntity != null)
                    {
                        var player = playerMapper.MapToDomain(playerEntity, allTeamPlayers);
                        result.Add(TeamPlayerMapper.MapToDomain(entity, team, player));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener TeamPlayers por TeamID");
                return new List<TeamPlayer>();
            }
        }

        public async Task<IEnumerable<TeamPlayer>> GetByPlayerIdAsync(PlayerID playerId)
        {
            try
            {
                var query = @"
                    SELECT * 
                    FROM TeamPlayers
                    WHERE PlayerID = @playerId";

                var parameter = new SqlParameter("@playerId", playerId.Value);
                var entities = await _context.TeamPlayers.FromSqlRaw(query, parameter).ToListAsync();

                if (!entities.Any())
                    return new List<TeamPlayer>();

                // Load all related data for mapping
                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();
                var playerEntity = await _context.Players.FindAsync(playerId.Value);

                if (playerEntity == null)
                    return new List<TeamPlayer>();

                // Map the player entity to domain
                var playerMapper = new Infrastructure.Persistence.Players.Mapper.PlayerMapper();
                var player = playerMapper.MapToDomain(playerEntity, allTeamPlayers);

                var result = new List<TeamPlayer>();

                foreach (var entity in entities)
                {
                    var teamEntity = await _context.Teams.FindAsync(entity.TeamID);
                    if (teamEntity != null)
                    {
                        var team = Infrastructure.Persistence.Teams.Mapper.TeamMapper.MapToDomain(teamEntity, allTeamPlayers, allPlayers);
                        result.Add(TeamPlayerMapper.MapToDomain(entity, team, player));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener TeamPlayers por PlayerID");
                return new List<TeamPlayer>();
            }
        }

        public async Task<TeamPlayer> AddAsync(TeamPlayer teamPlayer)
        {
            try
            {
                var query = @"
            INSERT INTO TeamPlayers (TeamID, PlayerID, JoinedAt, RoleInTeam)
            VALUES (@teamId, @playerId, @joinedAt, @roleInTeam);";

                var parameters = new[]
                {
            new SqlParameter("@teamId", teamPlayer.TeamID.Value),
            new SqlParameter("@playerId", teamPlayer.PlayerID.Value),
            new SqlParameter("@joinedAt", teamPlayer.JoinedAt),
            new SqlParameter("@roleInTeam", (object?)teamPlayer.RoleInTeam ?? DBNull.Value)
        };

                await _context.Database.ExecuteSqlRawAsync(query, parameters);
                return teamPlayer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar TeamPlayer");
                throw;
            }
        }


        public async Task UpdateAsync(TeamPlayer teamPlayer)
        {
            try
            {
                var query = @"
                    UPDATE TeamPlayers
                    SET RoleInTeam = @roleInTeam
                    WHERE TeamID = @teamId AND PlayerID = @playerId";

                var parameters = new[]
                {
                    new SqlParameter("@roleInTeam", teamPlayer.RoleInTeam.HasValue ? (object)teamPlayer.RoleInTeam.Value : DBNull.Value),
                    new SqlParameter("@teamId", teamPlayer.TeamID.Value),
                    new SqlParameter("@playerId", teamPlayer.PlayerID.Value)
                };

                int result = await _context.Database.ExecuteSqlRawAsync(query, parameters);
                if (result == 0)
                {
                    _logger.LogWarning("No se actualizó la relación de TeamPlayer");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación de TeamPlayer");
            }
        }

        public async Task<bool> DeleteAsync(TeamID teamId, PlayerID playerId)
        {
            try
            {
                var query = @"
                    DELETE FROM TeamPlayers
                    WHERE TeamID = @teamId AND PlayerID = @playerId";

                var parameters = new[]
                {
                    new SqlParameter("@teamId", teamId.Value),
                    new SqlParameter("@playerId", playerId.Value)
                };

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación de TeamPlayer");
                return false;
            }
        }
    }
}