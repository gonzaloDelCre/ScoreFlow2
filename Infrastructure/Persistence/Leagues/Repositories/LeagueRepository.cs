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
using Infrastructure.Persistence.Teams.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Leagues.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeagueRepository> _logger;

        public LeagueRepository(
            ApplicationDbContext context,
            ILogger<LeagueRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<League>> GetAllAsync()
        {
            try
            {
                var leagueEntities = await _context.Leagues
                    .Include(l => l.Teams)
                    .ToListAsync();

                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                var result = leagueEntities
                    .Select(entity =>
                    {
                        // Mapea entidad base
                        var league = LeagueMapper.MapToDomainSimple(entity);

                        // Mapea equipos asociados
                        foreach (var teamEnt in entity.Teams)
                        {
                            var teamPlayers = allTeamPlayers.Where(tp => tp.TeamID == teamEnt.TeamID).ToList();
                            var team = TeamMapper.MapToDomain(
                                teamEnt,
                                league,
                                teamPlayers,
                                allPlayers
                            );
                            league.AddTeam(team);
                        }
                        return league;
                    })
                    .ToList();

                return result;
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
                var entity = await _context.Leagues
                    .Include(l => l.Teams)
                    .FirstOrDefaultAsync(l => l.LeagueID == leagueId.Value);

                if (entity == null) return null;

                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                var league = LeagueMapper.MapToDomain(entity);
                foreach (var teamEnt in entity.Teams)
                {
                    var teamPlayers = allTeamPlayers.Where(tp => tp.TeamID == teamEnt.TeamID).ToList();
                    var team = TeamMapper.MapToDomain(
                        teamEnt,
                        league,
                        teamPlayers,
                        allPlayers
                    );
                    league.AddTeam(team);
                }
                return league;
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
                var entity = await _context.Leagues
                    .Include(l => l.Teams)
                    .FirstOrDefaultAsync(l => l.Name == name);

                if (entity == null) return null;

                var allTeamPlayers = await _context.TeamPlayers.ToListAsync();
                var allPlayers = await _context.Players.ToListAsync();

                var league = LeagueMapper.MapToDomain(entity);
                foreach (var teamEnt in entity.Teams)
                {
                    var teamPlayers = allTeamPlayers.Where(tp => tp.TeamID == teamEnt.TeamID).ToList();
                    var team = TeamMapper.MapToDomain(
                        teamEnt,
                        league,
                        teamPlayers,
                        allPlayers
                    );
                    league.AddTeam(team);
                }
                return league;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la liga con nombre {Name}", name);
                return null;
            }
        }

        public async Task<League> AddAsync(League domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            try
            {
                // 1) Mapear sin ID
                var entity = domain.MapToEntity();
                _context.Leagues.Add(entity);

                // 2) EF Core insertará con LeagueID = IDENTITY
                await _context.SaveChangesAsync();

                // 3) Devolver un nuevo dominio con el ID generado
                return new League(
                    new LeagueID(entity.LeagueID),
                    domain.Name,
                    domain.Description,
                    domain.CreatedAt
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar una nueva liga");
                throw;
            }
        }

        public async Task UpdateAsync(League domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            try
            {
                var entity = await _context.Leagues
                    .FirstOrDefaultAsync(l => l.LeagueID == domain.LeagueID.Value);

                if (entity == null)
                    throw new InvalidOperationException("Liga no encontrada.");

                entity.Name = domain.Name.Value;
                entity.Description = domain.Description;
                entity.CreatedAt = domain.CreatedAt;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la liga con ID {LeagueID}", domain.LeagueID.Value);
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


       
