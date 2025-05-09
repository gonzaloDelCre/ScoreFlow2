using Domain.Entities.Leagues;
using Domain.Entities.Standings;
using Domain.Ports.Leagues;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Standings.Mapper;
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Persistence.Leagues.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeagueRepository> _logger;
        private readonly ILeagueMapper _leagueMapper;
        private readonly IStandingMapper _standingMapper;
        private readonly ITeamMapper _teamMapper;

        public LeagueRepository(
            ApplicationDbContext context,
            ILogger<LeagueRepository> logger,
            ILeagueMapper leagueMapper,
            IStandingMapper standingMapper,
            ITeamMapper teamMapper)
        {
            _context = context;
            _logger = logger;
            _leagueMapper = leagueMapper;
            _standingMapper = standingMapper;
            _teamMapper = teamMapper;
        }

        /// <summary>
        /// Obtiene una liga por su ID
        /// </summary>
        /// <param name="leagueId">ID de la liga</param>
        /// <returns>Liga o null si no existe</returns>
        public async Task<League?> GetByIdAsync(LeagueID leagueId)
        {
            var entity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE ID = @ID", new SqlParameter("@ID", leagueId.Value))
                .FirstOrDefaultAsync();

            return entity == null
                ? null
                : _leagueMapper.MapToDomain(entity);
        }

        /// <summary>
        /// Obtiene todas las ligas
        /// </summary>
        /// <returns>Lista de ligas</returns>
        public async Task<IEnumerable<League>> GetAllAsync()
        {
            var entities = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues")
                .ToListAsync();

            return entities.Select(e => _leagueMapper.MapToDomain(e));
        }

        /// <summary>
        /// Obtiene una liga por su nombre
        /// </summary>
        /// <param name="name">Nombre de la liga</param>
        /// <returns>Liga o null si no existe</returns>
        public async Task<League?> GetByNameAsync(string name)
        {
            var entity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE Name = @Name", new SqlParameter("@Name", name))
                .FirstOrDefaultAsync();

            return entity == null
                ? null
                : _leagueMapper.MapToDomain(entity);
        }

        /// <summary>
        /// Añade una nueva liga
        /// </summary>
        /// <param name="league">Liga a añadir</param>
        /// <returns>Liga añadida</returns>
        public async Task<League> AddAsync(League league)
        {
            const string sql = @"
                INSERT INTO Leagues (Name, Description, CreatedAt)
                VALUES (@Name, @Description, @CreatedAt)";

            var parameters = new[]
            {
                new SqlParameter("@Name", league.Name.Value),
                new SqlParameter("@Description", league.Description ?? (object)DBNull.Value),
                new SqlParameter("@CreatedAt", league.CreatedAt)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            var newId = await _context.Leagues
                .FromSqlRaw("SELECT TOP 1 * FROM Leagues ORDER BY ID DESC")
                .Select(l => l.ID)
                .FirstAsync();

            var entity = await _context.Leagues
                .FromSqlRaw("SELECT * FROM Leagues WHERE ID = @ID", new SqlParameter("@ID", newId))
                .FirstAsync();

            return _leagueMapper.MapToDomain(entity);
        }

        /// <summary>
        /// Actualiza una liga existente
        /// </summary>
        /// <param name="league">Liga con datos actualizados</param>
        public async Task UpdateAsync(League league)
        {
            const string sql = @"
                UPDATE Leagues
                SET Name = @Name,
                    Description = @Description
                WHERE ID = @ID";

            var parameters = new[]
            {
                new SqlParameter("@ID", league.LeagueID.Value),
                new SqlParameter("@Name", league.Name.Value),
                new SqlParameter("@Description", league.Description ?? (object)DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <summary>
        /// Elimina una liga por su ID
        /// </summary>
        /// <param name="leagueId">ID de la liga</param>
        /// <returns>true si la liga fue eliminada</returns>
        public async Task<bool> DeleteAsync(LeagueID leagueId)
        {
            const string sql = "DELETE FROM Leagues WHERE ID = @ID";

            var rows = await _context.Database
                .ExecuteSqlRawAsync(sql, new SqlParameter("@ID", leagueId.Value));

            return rows > 0;
        }

        /// <summary>
        /// Obtiene la clasificación de una liga
        /// </summary>
        /// <param name="leagueId">ID de la liga</param>
        /// <returns>Lista de clasificaciones</returns>
        public async Task<IEnumerable<Standing>> GetStandingsAsync(LeagueID leagueId)
        {
            var standings = await _context.Standings
                .FromSqlRaw("SELECT * FROM Standings WHERE LeagueID = @LeagueID",
                    new SqlParameter("@LeagueID", leagueId.Value))
                .Include(s => s.Team)
                .ToListAsync();

            var league = await GetByIdAsync(leagueId);
            if (league == null)
                return Enumerable.Empty<Standing>();

            return standings.Select(s => _standingMapper.MapToDomain(
                s,
                league,
                _teamMapper.ToDomain(s.Team, Enumerable.Empty<Domain.Entities.Players.Player>(), Enumerable.Empty<Standing>())
            ));
        }

        /// <summary>
        /// Actualiza la clasificación de una liga
        /// </summary>
        /// <param name="leagueId">ID de la liga</param>
        /// <param name="standings">Clasificaciones actualizadas</param>
        public async Task UpdateStandingsAsync(LeagueID leagueId, IEnumerable<Standing> standings)
        {
            foreach (var standing in standings)
            {
                const string sql = @"
                    UPDATE Standings
                    SET Points = @Points,
                        Wins = @Wins,
                        Draws = @Draws,
                        Losses = @Losses,
                        GoalDifference = @GoalDifference
                    WHERE ID = @ID AND LeagueID = @LeagueID AND TeamID = @TeamID";

                var parameters = new[]
                {
                    new SqlParameter("@ID", standing.StandingID.Value),
                    new SqlParameter("@LeagueID", leagueId.Value),
                    new SqlParameter("@TeamID", standing.TeamID.Value),
                    new SqlParameter("@Points", standing.Points.Value),
                    new SqlParameter("@Wins", standing.Wins.Value),
                    new SqlParameter("@Draws", standing.Draws.Value),
                    new SqlParameter("@Losses", standing.Losses.Value),
                    new SqlParameter("@GoalDifference", standing.GoalDifference.Value)
                };

                var rows = await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                // Si no existe, lo insertamos
                if (rows == 0)
                {
                    const string insertSql = @"
                        INSERT INTO Standings (LeagueID, TeamID, Points, Wins, Draws, Losses, GoalDifference, CreatedAt)
                        VALUES (@LeagueID, @TeamID, @Points, @Wins, @Draws, @Losses, @GoalDifference, @CreatedAt)";

                    var insertParameters = new[]
                    {
                        new SqlParameter("@LeagueID", leagueId.Value),
                        new SqlParameter("@TeamID", standing.TeamID.Value),
                        new SqlParameter("@Points", standing.Points.Value),
                        new SqlParameter("@Wins", standing.Wins.Value),
                        new SqlParameter("@Draws", standing.Draws.Value),
                        new SqlParameter("@Losses", standing.Losses.Value),
                        new SqlParameter("@GoalDifference", standing.GoalDifference.Value),
                        new SqlParameter("@CreatedAt", DateTime.UtcNow)
                    };

                    await _context.Database.ExecuteSqlRawAsync(insertSql, insertParameters);
                }
            }
        }
    }
}
