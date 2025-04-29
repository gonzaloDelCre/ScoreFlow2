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
                // Paso 1: Obtener todos los equipos sin duplicados
                string teamsSql = "SELECT * FROM Teams";
                var teams = await _context.Teams
                    .FromSqlRaw(teamsSql)
                    .AsNoTracking()
                    .ToListAsync();

                // Paso 2: Obtener todas las relaciones equipo-jugador
                string teamPlayersSql = "SELECT * FROM TeamPlayers";
                var teamPlayers = await _context.TeamPlayers
                    .FromSqlRaw(teamPlayersSql)
                    .AsNoTracking()
                    .ToListAsync();

                // Paso 3: Obtener todos los jugadores
                string playersSql = "SELECT * FROM Players";
                var players = await _context.Players
                    .FromSqlRaw(playersSql)
                    .AsNoTracking()
                    .ToListAsync();

                // Paso 4: Mapear cada equipo a su modelo de dominio
                var result = new List<Team>();
                foreach (var teamEntity in teams)
                {
                    // Filtrar relaciones para este equipo
                    var teamPlayerRelations = teamPlayers
                        .Where(tp => tp.TeamID == teamEntity.TeamID)
                        .ToList();

                    // Obtener jugadores para este equipo
                    var teamPlayerEntities = players
                        .Where(p => teamPlayerRelations.Any(tp => tp.PlayerID == p.PlayerID))
                        .ToList();

                    // Mapear al dominio
                    var team = TeamMapper.MapToDomain(
                        teamEntity,
                        teamPlayerRelations,
                        teamPlayerEntities
                    );

                    result.Add(team);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de equipos: {Message}", ex.Message);
                return new List<Team>(); // Retornar lista vacía en caso de error
            }
        }

        public async Task<Team?> GetByIdAsync(TeamID teamId)
        {
            try
            {
                // Paso 1: Obtener el equipo por ID
                string teamSql = "SELECT * FROM Teams WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);

                var teamEntity = await _context.Teams
                    .FromSqlRaw(teamSql, parameter)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (teamEntity == null)
                {
                    return null;
                }

                // Paso 2: Obtener relaciones de jugadores para este equipo
                string teamPlayersSql = "SELECT * FROM TeamPlayers WHERE TeamID = @TeamID";
                var teamPlayers = await _context.TeamPlayers
                    .FromSqlRaw(teamPlayersSql, parameter)
                    .AsNoTracking()
                    .ToListAsync();

                // Paso 3: Obtener los jugadores asociados
                if (!teamPlayers.Any())
                {
                    // Si no hay jugadores, devolver el equipo sin jugadores
                    return TeamMapper.MapToDomain(teamEntity, new List<TeamPlayerEntity>(), new List<PlayerEntity>());
                }

                // Crear lista de parámetros para la consulta IN
                string playerIds = string.Join(",", teamPlayers.Select(tp => tp.PlayerID));
                string playersSql = $"SELECT * FROM Players WHERE PlayerID IN ({playerIds})";

                var players = await _context.Players
                    .FromSqlRaw(playersSql)
                    .AsNoTracking()
                    .ToListAsync();

                return TeamMapper.MapToDomain(teamEntity, teamPlayers, players);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el equipo con ID {TeamID}", teamId.Value);
                return null;
            }
        }
        public async Task<Team?> GetByExternalIdAsync(string externalId)
        {
            // Suponemos que has añadido la columna ExternalID en TeamEntity
            var entity = await _context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ExternalID == externalId);

            if (entity == null) return null;

            // Carga sus TeamPlayers y PlayerEntities si lo necesitas aquí
            // Para simplicidad, devolvemos solo la entidad sin jugadores
            return new Team(
                new TeamID(entity.TeamID),
                new TeamName(entity.Name),
                entity.CreatedAt,
                entity.Logo,
                entity.ExternalID
            );
        }

        public async Task<Team> AddAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                var teamEntity = TeamMapper.MapToEntity(team);

                // Insertar equipo
                string insertSql = @"INSERT INTO Teams (Name, Logo, CreatedAt, ExternalID, Category, Club, Stadium) 
                     VALUES (@Name, @Logo, @CreatedAt, @ExternalID, @Category, @Club, @Stadium);
                     SELECT SCOPE_IDENTITY();";


                var parameters = new[]
                {
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@CreatedAt", teamEntity.CreatedAt),
                    new SqlParameter("@ExternalID", (object?)teamEntity.ExternalID ?? DBNull.Value),
                    new SqlParameter("@Category", (object?)teamEntity.Category ?? DBNull.Value),
                    new SqlParameter("@Club", (object?)teamEntity.Club ?? DBNull.Value),
                    new SqlParameter("@Stadium", (object?)teamEntity.Stadium ?? DBNull.Value)
};


                // Ejecutar la consulta y obtener el ID insertado
                var newTeamId = Convert.ToInt32(await _context.Database.ExecuteSqlRawAsync(insertSql, parameters));

                // Insertar jugadores si existen
                if (team.Players.Any())
                {
                    // Crear consulta batch para insertar múltiples relaciones
                    string teamPlayerSql = "INSERT INTO TeamPlayers (TeamID, PlayerID) VALUES ";
                    var teamPlayerParams = new List<SqlParameter>();

                    int paramIndex = 0;
                    for (int i = 0; i < team.Players.Count; i++)
                    {
                        var player = team.Players.ElementAt(i);
                        if (i > 0)
                            teamPlayerSql += ", ";

                        teamPlayerSql += $"(@TeamID{paramIndex}, @PlayerID{paramIndex})";
                        teamPlayerParams.Add(new SqlParameter($"@TeamID{paramIndex}", newTeamId));
                        teamPlayerParams.Add(new SqlParameter($"@PlayerID{paramIndex}", player.PlayerID.Value));
                        paramIndex++;
                    }

                    await _context.Database.ExecuteSqlRawAsync(teamPlayerSql, teamPlayerParams.ToArray());
                }

                // Devolver el equipo creado con su ID
                return new Team(
                    new TeamID(newTeamId),
                    team.Name.Value,
                    team.CreatedAt,
                    team.Logo,
                    team.Category,
                    team.Club,
                    team.Stadium
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo equipo: {Message}", ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "El equipo no puede ser null");

            try
            {
                var teamEntity = TeamMapper.MapToEntity(team);

                // Actualizar equipo
                string updateSql = @"UPDATE Teams 
                     SET Name = @Name, Logo = @Logo, ExternalID = @ExternalID, Category = @Category, 
                         Club = @Club, Stadium = @Stadium
                     WHERE TeamID = @TeamID";

                var parameters = new[]
                {
                    new SqlParameter("@TeamID", teamEntity.TeamID),
                    new SqlParameter("@Name", teamEntity.Name),
                    new SqlParameter("@Logo", teamEntity.Logo),
                    new SqlParameter("@ExternalID", (object?)teamEntity.ExternalID ?? DBNull.Value),
                    new SqlParameter("@Category", (object?)teamEntity.Category ?? DBNull.Value),
                    new SqlParameter("@Club", (object?)teamEntity.Club ?? DBNull.Value),
                    new SqlParameter("@Stadium", (object?)teamEntity.Stadium ?? DBNull.Value)
                };


                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);

                // Actualizar jugadores
                if (team.Players.Any())
                {
                    // Eliminar relaciones anteriores
                    string deleteRelationsSql = "DELETE FROM TeamPlayers WHERE TeamID = @TeamID";
                    var deleteParam = new SqlParameter("@TeamID", team.TeamID.Value);
                    await _context.Database.ExecuteSqlRawAsync(deleteRelationsSql, deleteParam);

                    // Crear consulta batch para insertar nuevas relaciones
                    string teamPlayerSql = "INSERT INTO TeamPlayers (TeamID, PlayerID) VALUES ";
                    var teamPlayerParams = new List<SqlParameter>();

                    int paramIndex = 0;
                    for (int i = 0; i < team.Players.Count; i++)
                    {
                        var player = team.Players.ElementAt(i);
                        if (i > 0)
                            teamPlayerSql += ", ";

                        teamPlayerSql += $"(@TeamID{paramIndex}, @PlayerID{paramIndex})";
                        teamPlayerParams.Add(new SqlParameter($"@TeamID{paramIndex}", team.TeamID.Value));
                        teamPlayerParams.Add(new SqlParameter($"@PlayerID{paramIndex}", player.PlayerID.Value));
                        paramIndex++;
                    }

                    await _context.Database.ExecuteSqlRawAsync(teamPlayerSql, teamPlayerParams.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el equipo con ID {TeamID}: {Message}",
                    team.TeamID.Value, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(TeamID teamId)
        {
            try
            {
                // Eliminar las relaciones de jugadores primero (debido a restricciones de clave externa)
                string deleteRelationsSql = "DELETE FROM TeamPlayers WHERE TeamID = @TeamID";
                var parameter = new SqlParameter("@TeamID", teamId.Value);
                await _context.Database.ExecuteSqlRawAsync(deleteRelationsSql, parameter);

                // Ahora eliminar el equipo
                string deleteTeamSql = "DELETE FROM Teams WHERE TeamID = @TeamID";
                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(deleteTeamSql, parameter);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el equipo con ID {TeamID}: {Message}",
                    teamId.Value, ex.Message);
                return false;
            }
        }
    }
}