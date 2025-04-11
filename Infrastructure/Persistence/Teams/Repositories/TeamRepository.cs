using Domain.Entities.Players;
using Domain.Entities.Teams;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Teams.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(ApplicationDbContext context, ILogger<TeamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            try
            {
                string sql = @"
                    SELECT t.*, tp.*, p.* 
                    FROM Teams t
                    LEFT JOIN TeamPlayers tp ON t.TeamID = tp.TeamID
                    LEFT JOIN Players p ON tp.PlayerID = p.PlayerID";

                var dbTeams = await _context.Teams
                    .FromSqlRaw(sql)
                    .ToListAsync();

                var teamPlayers = await _context.TeamPlayers.ToListAsync();
                var players = await _context.Players.ToListAsync();

                // Mapeo de equipos con jugadores
                return dbTeams.Select(teamEntity => TeamMapper.MapToDomain(
                    teamEntity,
                    teamPlayers.Where(tp => tp.TeamID == teamEntity.TeamID).ToList(),
                    players.Where(p => teamPlayers.Any(tp => tp.PlayerID == p.PlayerID && tp.TeamID == teamEntity.TeamID)).ToList()
                )).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de equipos");
                return new List<Team>();
            }
        }

        public async Task<Team?> GetByIdAsync(TeamID teamId)
        {
            try
            {
                string sql = @"
                    SELECT t.*, tp.*, p.* 
                    FROM Teams t
                    LEFT JOIN TeamPlayers tp ON t.TeamID = tp.TeamID
                    LEFT JOIN Players p ON tp.PlayerID = p.PlayerID
                    WHERE t.TeamID = @TeamID";

                var parameter = new SqlParameter("@TeamID", teamId.Value);

                var dbTeams = await _context.Teams
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                if (!dbTeams.Any())
                {
                    return null;
                }

                var teamEntity = dbTeams.First();
                var teamPlayers = await _context.TeamPlayers.Where(tp => tp.TeamID == teamEntity.TeamID).ToListAsync();
                var players = await _context.Players.ToListAsync();

                return TeamMapper.MapToDomain(
                    teamEntity,
                    teamPlayers,
                    players.Where(p => teamPlayers.Any(tp => tp.PlayerID == p.PlayerID && tp.TeamID == teamEntity.TeamID)).ToList()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el equipo con ID {TeamID}", teamId.Value);
                return null;
            }
        }

        public async Task<Team> AddAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                // Use the static TeamMapper method instead of an instance method
                var teamEntity = TeamMapper.MapToEntity(team);

                // Insertar equipo
                string insertSql = @"INSERT INTO Teams (Name, Logo, CreatedAt, Category, Club, Stadium) 
                                     VALUES (@Name, @Logo, @CreatedAt, @Category, @Club, @Stadium);";
                var parameters = new[]
                {
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@CreatedAt", teamEntity.CreatedAt),
                    new SqlParameter("@Category", teamEntity.Category ?? (object)DBNull.Value),
                    new SqlParameter("@Club", teamEntity.Club ?? (object)DBNull.Value),
                    new SqlParameter("@Stadium", teamEntity.Stadium ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                // Obtener el nuevo TeamID
                string selectSql = "SELECT TOP 1 TeamID FROM Teams ORDER BY TeamID DESC";
                var newTeamId = await _context.Teams
                    .FromSqlRaw(selectSql)
                    .Select(t => t.TeamID)
                    .FirstOrDefaultAsync();

                // Insertar jugadores si existen
                if (team.Players.Any())
                {
                    foreach (var player in team.Players)
                    {
                        var playerTeamRelation = new TeamPlayerEntity
                        {
                            TeamID = newTeamId,
                            PlayerID = player.PlayerID.Value
                        };
                        await _context.TeamPlayers.AddAsync(playerTeamRelation);
                    }
                    await _context.SaveChangesAsync();
                }

                return new Team(
                    new TeamID(newTeamId),
                    team.Name,
                    team.CreatedAt,
                    team.Logo
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo equipo");
                throw;
            }
        }

        public async Task UpdateAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                // Use the static TeamMapper method instead of an instance method
                var teamEntity = TeamMapper.MapToEntity(team);

                // Actualizar equipo
                string updateSql = @"UPDATE Teams 
                                     SET Name = @Name, Logo = @Logo, Category = @Category, 
                                         Club = @Club, Stadium = @Stadium
                                     WHERE TeamID = @TeamID";
                var parameters = new[]
                {
                    new SqlParameter("@TeamID", teamEntity.TeamID),
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@Category", teamEntity.Category ?? (object)DBNull.Value),
                    new SqlParameter("@Club", teamEntity.Club ?? (object)DBNull.Value),
                    new SqlParameter("@Stadium", teamEntity.Stadium ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);

                // Actualizar jugadores
                if (team.Players.Any())
                {
                    // Eliminar relaciones anteriores de jugadores
                    var existingRelations = await _context.TeamPlayers
                        .Where(tp => tp.TeamID == team.TeamID.Value)
                        .ToListAsync();
                    _context.TeamPlayers.RemoveRange(existingRelations);

                    // Insertar nuevas relaciones
                    foreach (var player in team.Players)
                    {
                        var playerTeamRelation = new TeamPlayerEntity
                        {
                            TeamID = team.TeamID.Value,
                            PlayerID = player.PlayerID.Value
                        };
                        await _context.TeamPlayers.AddAsync(playerTeamRelation);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el equipo con ID {TeamID}", team.TeamID.Value);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TeamID teamId)
        {
            try
            {
                // Eliminar las relaciones de jugadores
                var teamPlayers = await _context.TeamPlayers
                    .Where(tp => tp.TeamID == teamId.Value)
                    .ToListAsync();
                _context.TeamPlayers.RemoveRange(teamPlayers);

                // Eliminar el equipo
                string sql = "DELETE FROM Teams WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el equipo con ID {TeamID}", teamId.Value);
                return false;
            }
        }
    }
}