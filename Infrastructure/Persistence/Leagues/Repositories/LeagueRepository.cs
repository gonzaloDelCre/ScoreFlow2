using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Entities;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Players.Entities;
using Infrastructure.Persistence.TeamPlayers.Entities;
using Infrastructure.Persistence.Teams.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Leagues.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeagueRepository> _logger;
        private readonly LeagueMapper _mapper;

        public LeagueRepository(
            ApplicationDbContext context,
            ILogger<LeagueRepository> logger,
            LeagueMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<League>> GetAllAsync()
        {
            try
            {
                // Cargo Ligas + Equipos
                var leagues = await _context.Leagues
                    .Include(l => l.Teams)
                    .ToListAsync();

                // Cargo relaciones TeamPlayers y Players UNA VEZ
                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                // Mapeo todo
                return leagues
                    .Select(l => _mapper.MapToDomain(l, allTeamPlayers, allPlayers))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de ligas");
                return Enumerable.Empty<League>();
            }
        }

        public async Task<League?> GetByIdAsync(LeagueID leagueId)
        {
            try
            {
                var leagueEntity = await _context.Leagues
                    .Include(l => l.Teams)
                    .FirstOrDefaultAsync(l => l.LeagueID == leagueId.Value);

                if (leagueEntity == null) return null;

                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                return _mapper.MapToDomain(leagueEntity, allTeamPlayers, allPlayers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la liga con ID {LeagueID}", leagueId.Value);
                return null;
            }
        }

        public async Task<League?> GetByNameAsync(string name)
        {
            try
            {
                var leagueEntity = await _context.Leagues
                    .Include(l => l.Teams)
                    .FirstOrDefaultAsync(l => l.Name == name);

                if (leagueEntity == null) return null;

                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                return _mapper.MapToDomain(leagueEntity, allTeamPlayers, allPlayers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la liga con nombre {Name}", name);
                return null;
            }
        }

        public async Task<League> AddAsync(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league));

            try
            {
                var entity = _mapper.MapToEntity(league);
                _context.Leagues.Add(entity);
                await _context.SaveChangesAsync();

                // Recargamos con Teams vacíos (se podrán añadir luego)
                return league;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar una nueva liga");
                throw;
            }
        }

        public async Task UpdateAsync(League league)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league));

            try
            {
                var entity = await _context.Leagues
                    .FirstOrDefaultAsync(l => l.LeagueID == league.LeagueID.Value);

                if (entity == null)
                    throw new InvalidOperationException("Liga no encontrada.");

                // Mapa campos simples
                entity.Name = league.Name.Value;
                entity.Description = league.Description;
                entity.CreatedAt = league.CreatedAt;

                // NOTA: no actualizamos Teams aquí—usa otro repositorio o caso de uso
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la liga con ID {LeagueID}", league.LeagueID.Value);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(LeagueID leagueId)
        {
            try
            {
                var entity = await _context.Leagues
                    .FirstOrDefaultAsync(l => l.LeagueID == leagueId.Value);

                if (entity == null) return false;

                _context.Leagues.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la liga con ID {LeagueID}", leagueId.Value);
                return false;
            }
        }
    }
}
