using Domain.Entities.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.PlayerStatistics.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PlayerStatistics.Repositories
{
    public class PlayerStatisticRepository : IPlayerStatisticRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlayerStatisticRepository> _logger;
        private readonly PlayerStatisticMapper _mapper;

        public PlayerStatisticRepository(ApplicationDbContext context, ILogger<PlayerStatisticRepository> logger, PlayerStatisticMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlayerStatistic>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM PlayerStatistics";
                var dbStats = await _context.PlayerStatistics.FromSqlRaw(sql).ToListAsync();
                return dbStats.Select(dbStat => _mapper.MapToDomain(dbStat, null, null)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las estadísticas de los jugadores");
                return new List<PlayerStatistic>();
            }
        }

        public async Task<PlayerStatistic?> GetByIdAsync(int playerStatisticId)
        {
            try
            {
                string sql = "SELECT * FROM PlayerStatistics WHERE StatID = @StatID";
                var parameter = new SqlParameter("@StatID", playerStatisticId);
                var dbStats = await _context.PlayerStatistics.FromSqlRaw(sql, parameter).ToListAsync();
                var dbStat = dbStats.FirstOrDefault();
                return dbStat != null ? _mapper.MapToDomain(dbStat, null, null) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la estadística con ID {StatID}", playerStatisticId);
                return null;
            }
        }

        public async Task<IEnumerable<PlayerStatistic>> GetByPlayerIdAsync(int playerId)
        {
            try
            {
                string sql = "SELECT * FROM PlayerStatistics WHERE PlayerID = @PlayerID";
                var parameter = new SqlParameter("@PlayerID", playerId);
                var dbStats = await _context.PlayerStatistics.FromSqlRaw(sql, parameter).ToListAsync();
                return dbStats.Select(dbStat => _mapper.MapToDomain(dbStat, null, null)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas del jugador con ID {PlayerID}", playerId);
                return new List<PlayerStatistic>();
            }
        }

        public async Task<PlayerStatistic> AddAsync(PlayerStatistic playerStatistic)
        {
            try
            {
                var statEntity = _mapper.MapToEntity(playerStatistic);
                string sql = "INSERT INTO PlayerStatistics (PlayerID, MatchID, Goals, Assists, YellowCards, RedCards, MinutesPlayed, CreatedAt) " +
                             "VALUES (@PlayerID, @MatchID, @Goals, @Assists, @YellowCards, @RedCards, @MinutesPlayed, @CreatedAt);";

                var parameters = new[]
                {
                    new SqlParameter("@PlayerID", statEntity.PlayerID),
                    new SqlParameter("@MatchID", statEntity.MatchID),
                    new SqlParameter("@Goals", statEntity.Goals),
                    new SqlParameter("@Assists", statEntity.Assists),
                    new SqlParameter("@YellowCards", statEntity.YellowCards),
                    new SqlParameter("@RedCards", statEntity.RedCards),
                    new SqlParameter("@MinutesPlayed", statEntity.MinutesPlayed ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedAt", statEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                return playerStatistic;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar una nueva estadística de jugador");
                throw;
            }
        }

        public async Task UpdateAsync(PlayerStatistic playerStatistic)
        {
            try
            {
                var statEntity = _mapper.MapToEntity(playerStatistic);
                string sql = "UPDATE PlayerStatistics SET Goals = @Goals, Assists = @Assists, YellowCards = @YellowCards, " +
                             "RedCards = @RedCards, MinutesPlayed = @MinutesPlayed WHERE StatID = @StatID";

                var parameters = new[]
                {
                    new SqlParameter("@StatID", statEntity.StatID),
                    new SqlParameter("@Goals", statEntity.Goals),
                    new SqlParameter("@Assists", statEntity.Assists),
                    new SqlParameter("@YellowCards", statEntity.YellowCards),
                    new SqlParameter("@RedCards", statEntity.RedCards),
                    new SqlParameter("@MinutesPlayed", statEntity.MinutesPlayed ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la estadística con ID {StatID}", playerStatistic.PlayerStatisticID.Value);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int playerStatisticId)
        {
            try
            {
                string sql = "DELETE FROM PlayerStatistics WHERE StatID = @StatID";
                var parameter = new SqlParameter("@StatID", playerStatisticId);
                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la estadística con ID {StatID}", playerStatisticId);
                return false;
            }
        }
    }
}
