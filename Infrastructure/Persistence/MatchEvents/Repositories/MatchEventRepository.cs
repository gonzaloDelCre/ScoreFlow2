using Domain.Entities.MatchEvents;
using Domain.Entities.Players;
using Domain.Enum;
using Domain.Ports.MatchEvents;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Matches.Mapper;
using Infrastructure.Persistence.MatchEvents.Mapper;
using Infrastructure.Persistence.Players.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.MatchEvents.Repositories
{
    public class MatchEventRepository : IMatchEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MatchEventRepository> _logger;
        private readonly IMatchEventMapper _mapper;
        private readonly IMatchMapper _matchMapper;
        private readonly IPlayerMapper _playerMapper;

        public MatchEventRepository(
            ApplicationDbContext context,
            ILogger<MatchEventRepository> logger,
            IMatchEventMapper mapper,
            IMatchMapper matchMapper,
            IPlayerMapper playerMapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _matchMapper = matchMapper;
            _playerMapper = playerMapper;
        }

        /// <summary>
        /// Obtiene un evento de partido por su ID
        /// </summary>
        /// <param name="matchEventId">ID del evento</param>
        /// <returns>Evento o null si no existe</returns>
        public async Task<MatchEvent?> GetByIdAsync(MatchEventID matchEventId)
        {
            var entity = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents WHERE ID = @ID", new SqlParameter("@ID", matchEventId.Value))
                .Include(e => e.Match)
                .Include(e => e.Player)
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var match = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .FirstOrDefaultAsync(m => m.ID == entity.MatchID);

            if (match == null)
                return null;

            var matchDomain = _matchMapper.ToDomain(match);

            Player? playerDomain = null;
            if (entity.PlayerID.HasValue)
            {
                var player = await _context.Players
                    .FirstOrDefaultAsync(p => p.PlayerID == entity.PlayerID.Value);

                if (player != null)
                {
                    var teamPlayers = await _context.TeamPlayers
                        .Where(tp => tp.PlayerID == player.PlayerID)
                        .ToListAsync();

                    playerDomain = _playerMapper.ToDomain(player, teamPlayers);
                }
            }

            return _mapper.MapToDomain(entity, matchDomain, playerDomain);
        }

        /// <summary>
        /// Obtiene todos los eventos de partidos
        /// </summary>
        /// <returns>Lista de eventos</returns>
        public async Task<IEnumerable<MatchEvent>> GetAllAsync()
        {
            var entities = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents")
                .Include(e => e.Match)
                .Include(e => e.Player)
                .ToListAsync();

            var result = new List<MatchEvent>();

            foreach (var entity in entities)
            {
                var match = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .FirstOrDefaultAsync(m => m.ID == entity.MatchID);

                if (match == null)
                    continue;

                var matchDomain = _matchMapper.ToDomain(match);

                Player? playerDomain = null;
                if (entity.PlayerID.HasValue)
                {
                    var player = await _context.Players
                        .FirstOrDefaultAsync(p => p.PlayerID == entity.PlayerID.Value);

                    if (player != null)
                    {
                        var teamPlayers = await _context.TeamPlayers
                            .Where(tp => tp.PlayerID == player.PlayerID)
                            .ToListAsync();

                        playerDomain = _playerMapper.ToDomain(player, teamPlayers);
                    }
                }

                result.Add(_mapper.MapToDomain(entity, matchDomain, playerDomain));
            }

            return result;
        }

        /// <summary>
        /// Obtiene eventos por ID de partido
        /// </summary>
        /// <param name="matchId">ID del partido</param>
        /// <returns>Lista de eventos del partido</returns>
        public async Task<IEnumerable<MatchEvent>> GetByMatchIdAsync(MatchID matchId)
        {
            var entities = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents WHERE MatchID = @MatchID", new SqlParameter("@MatchID", matchId.Value))
                .Include(e => e.Match)
                .Include(e => e.Player)
                .ToListAsync();

            var match = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .FirstOrDefaultAsync(m => m.ID == matchId.Value);

            if (match == null)
                return Enumerable.Empty<MatchEvent>();

            var matchDomain = _matchMapper.ToDomain(match);
            var result = new List<MatchEvent>();

            foreach (var entity in entities)
            {
                Player? playerDomain = null;
                if (entity.PlayerID.HasValue)
                {
                    var player = await _context.Players
                        .FirstOrDefaultAsync(p => p.PlayerID == entity.PlayerID.Value);

                    if (player != null)
                    {
                        var teamPlayers = await _context.TeamPlayers
                            .Where(tp => tp.PlayerID == player.PlayerID)
                            .ToListAsync();

                        playerDomain = _playerMapper.ToDomain(player, teamPlayers);
                    }
                }

                result.Add(_mapper.MapToDomain(entity, matchDomain, playerDomain));
            }

            return result;
        }

        /// <summary>
        /// Obtiene eventos por ID de jugador
        /// </summary>
        /// <param name="playerId">ID del jugador</param>
        /// <returns>Lista de eventos del jugador</returns>
        public async Task<IEnumerable<MatchEvent>> GetByPlayerIdAsync(PlayerID playerId)
        {
            var entities = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents WHERE PlayerID = @PlayerID", new SqlParameter("@PlayerID", playerId.Value))
                .Include(e => e.Match)
                .Include(e => e.Player)
                .ToListAsync();

            var result = new List<MatchEvent>();

            foreach (var entity in entities)
            {
                var match = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .FirstOrDefaultAsync(m => m.ID == entity.MatchID);

                if (match == null)
                    continue;

                var matchDomain = _matchMapper.ToDomain(match);

                var player = await _context.Players
                    .FirstOrDefaultAsync(p => p.PlayerID == playerId.Value);

                if (player == null)
                    continue;

                var teamPlayers = await _context.TeamPlayers
                    .Where(tp => tp.PlayerID == player.PlayerID)
                    .ToListAsync();

                var playerDomain = _playerMapper.ToDomain(player, teamPlayers);

                result.Add(_mapper.MapToDomain(entity, matchDomain, playerDomain));
            }

            return result;
        }

        /// <summary>
        /// Obtiene eventos por tipo
        /// </summary>
        /// <param name="eventType">Tipo de evento</param>
        /// <returns>Lista de eventos del tipo especificado</returns>
        public async Task<IEnumerable<MatchEvent>> GetByTypeAsync(EventType eventType)
        {
            var entities = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents WHERE EventType = @EventType", new SqlParameter("@EventType", (int)eventType))
                .Include(e => e.Match)
                .Include(e => e.Player)
                .ToListAsync();

            var result = new List<MatchEvent>();

            foreach (var entity in entities)
            {
                var match = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .FirstOrDefaultAsync(m => m.ID == entity.MatchID);

                if (match == null)
                    continue;

                var matchDomain = _matchMapper.ToDomain(match);

                Player? playerDomain = null;
                if (entity.PlayerID.HasValue)
                {
                    var player = await _context.Players
                        .FirstOrDefaultAsync(p => p.PlayerID == entity.PlayerID.Value);

                    if (player != null)
                    {
                        var teamPlayers = await _context.TeamPlayers
                            .Where(tp => tp.PlayerID == player.PlayerID)
                            .ToListAsync();

                        playerDomain = _playerMapper.ToDomain(player, teamPlayers);
                    }
                }

                result.Add(_mapper.MapToDomain(entity, matchDomain, playerDomain));
            }

            return result;
        }

        /// <summary>
        /// Obtiene eventos en un rango de minutos
        /// </summary>
        /// <param name="fromMinute">Minuto inicial</param>
        /// <param name="toMinute">Minuto final</param>
        /// <returns>Lista de eventos en el rango especificado</returns>
        public async Task<IEnumerable<MatchEvent>> GetByMinuteRangeAsync(int fromMinute, int toMinute)
        {
            var entities = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents WHERE Minute >= @FromMinute AND Minute <= @ToMinute",
                    new SqlParameter("@FromMinute", fromMinute),
                    new SqlParameter("@ToMinute", toMinute))
                .Include(e => e.Match)
                .Include(e => e.Player)
                .ToListAsync();

            var result = new List<MatchEvent>();

            foreach (var entity in entities)
            {
                var match = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .FirstOrDefaultAsync(m => m.ID == entity.MatchID);

                if (match == null)
                    continue;

                var matchDomain = _matchMapper.ToDomain(match);

                Player? playerDomain = null;
                if (entity.PlayerID.HasValue)
                {
                    var player = await _context.Players
                        .FirstOrDefaultAsync(p => p.PlayerID == entity.PlayerID.Value);

                    if (player != null)
                    {
                        var teamPlayers = await _context.TeamPlayers
                            .Where(tp => tp.PlayerID == player.PlayerID)
                            .ToListAsync();

                        playerDomain = _playerMapper.ToDomain(player, teamPlayers);
                    }
                }

                result.Add(_mapper.MapToDomain(entity, matchDomain, playerDomain));
            }

            return result;
        }

        /// <summary>
        /// Añade un nuevo evento de partido
        /// </summary>
        /// <param name="matchEvent">Evento a añadir</param>
        /// <returns>Evento añadido</returns>
        public async Task<MatchEvent> AddAsync(MatchEvent matchEvent)
        {
            const string sql = @"
                INSERT INTO MatchEvents (MatchID, PlayerID, EventType, Minute, CreatedAt)
                VALUES (@MatchID, @PlayerID, @EventType, @Minute, @CreatedAt)";

            var parameters = new[]
            {
                new SqlParameter("@MatchID", matchEvent.MatchID.Value),
                new SqlParameter("@PlayerID", matchEvent.PlayerID?.Value ?? (object)DBNull.Value),
                new SqlParameter("@EventType", (int)matchEvent.EventType),
                new SqlParameter("@Minute", matchEvent.Minute),
                new SqlParameter("@CreatedAt", matchEvent.CreatedAt)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            var newId = await _context.MatchEvents
                .FromSqlRaw("SELECT TOP 1 * FROM MatchEvents ORDER BY ID DESC")
                .Select(e => e.ID)
                .FirstAsync();

            var entity = await _context.MatchEvents
                .FromSqlRaw("SELECT * FROM MatchEvents WHERE ID = @ID", new SqlParameter("@ID", newId))
                .Include(e => e.Match)
                .Include(e => e.Player)
                .FirstAsync();

            var match = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .FirstOrDefaultAsync(m => m.ID == entity.MatchID);

            if (match == null)
                throw new InvalidOperationException($"Match with ID {entity.MatchID} not found");

            var matchDomain = _matchMapper.ToDomain(match);

            Player? playerDomain = null;
            if (entity.PlayerID.HasValue)
            {
                var player = await _context.Players
                    .FirstOrDefaultAsync(p => p.PlayerID == entity.PlayerID.Value);

                if (player != null)
                {
                    var teamPlayers = await _context.TeamPlayers
                        .Where(tp => tp.PlayerID == player.PlayerID)
                        .ToListAsync();

                    playerDomain = _playerMapper.ToDomain(player, teamPlayers);
                }
            }

            return _mapper.MapToDomain(entity, matchDomain, playerDomain);
        }

        /// <summary>
        /// Actualiza un evento de partido existente
        /// </summary>
        /// <param name="matchEvent">Evento con datos actualizados</param>
        public async Task UpdateAsync(MatchEvent matchEvent)
        {
            const string sql = @"
                UPDATE MatchEvents
                SET MatchID = @MatchID,
                    PlayerID = @PlayerID,
                    EventType = @EventType,
                    Minute = @Minute
                WHERE ID = @ID";

            var parameters = new[]
            {
                new SqlParameter("@ID", matchEvent.MatchEventID.Value),
                new SqlParameter("@MatchID", matchEvent.MatchID.Value),
                new SqlParameter("@PlayerID", matchEvent.PlayerID?.Value ?? (object)DBNull.Value),
                new SqlParameter("@EventType", (int)matchEvent.EventType),
                new SqlParameter("@Minute", matchEvent.Minute)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <summary>
        /// Elimina un evento de partido por su ID
        /// </summary>
        /// <param name="matchEventId">ID del evento</param>
        /// <returns>true si el evento fue eliminado</returns>
        public async Task<bool> DeleteAsync(MatchEventID matchEventId)
        {
            const string sql = "DELETE FROM MatchEvents WHERE ID = @ID";

            var rows = await _context.Database
                .ExecuteSqlRawAsync(sql, new SqlParameter("@ID", matchEventId.Value));

            return rows > 0;
        }
    }
}
