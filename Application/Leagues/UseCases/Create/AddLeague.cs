using Application.Leagues.DTOs;
using Application.Leagues.Mapper;
using Application.Leagues.UseCases.Update;
using Domain.Entities.Leagues;
using Domain.Ports.Leagues;
using Domain.Services.Leagues;
using Domain.Shared;

namespace Application.Leagues.UseCases.Create
{
    public class AddLeague
    {
        private readonly ILeagueRepository _leagueRepository;

        public AddLeague(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        // Ejecuta la adición de una nueva liga
        public async Task<League> Execute(LeagueRequestDTO leagueDTO)
        {
            // Verificar si la liga ya existe
            var existingLeague = await _leagueRepository.GetByNameAsync(leagueDTO.Name);
            if (existingLeague != null)
            {
                throw new InvalidOperationException("La liga ya existe. Utilice la opción de actualizar.");
            }

            // Si no existe, agregarla
            var newLeague = new League(
                new LeagueID(1), // ID generado por la base de datos
                new LeagueName(leagueDTO.Name),
                leagueDTO.Description,
                leagueDTO.CreatedAt
            );

            return await _leagueRepository.AddAsync(newLeague);
        }
    }

}
