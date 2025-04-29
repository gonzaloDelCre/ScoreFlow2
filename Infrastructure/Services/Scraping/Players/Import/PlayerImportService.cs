using Domain.Entities.Players;
using Domain.Entities.TeamPlayers;
using Domain.Enum;
using Domain.Ports.Players;
using Domain.Ports.TeamPlayers;
using Domain.Ports.Teams;
using Domain.Shared;
using Infrastructure.Services.Scraping.Players.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Scraping.Players.Import
{
    public class PlayerImportService
    {
        private readonly PlayerScraperService _scraper;
        private readonly IPlayerRepository _playerRepo;
        private readonly ITeamPlayerRepository _tpRepo;
        private readonly ITeamRepository _teamRepo;

        public PlayerImportService(
            PlayerScraperService scraper,
            IPlayerRepository playerRepo,
            ITeamPlayerRepository tpRepo,
            ITeamRepository teamRepo)
        {
            _scraper = scraper;
            _playerRepo = playerRepo;
            _tpRepo = tpRepo;
            _teamRepo = teamRepo;
        }

        public async Task ImportByTeamExternalIdAsync(int teamExternalId)
        {
            var team = await _teamRepo.GetByExternalIdAsync(teamExternalId.ToString());
            if (team == null)
                throw new InvalidOperationException($"Equipo con ExternalID={teamExternalId} no existe.");

            var scraped = await _scraper.GetPlayersByTeamExternalIdAsync(teamExternalId);

            foreach (var (name, age, position, goals, extId, photoUrl) in scraped)
            {
                var existingPlayer = await _playerRepo.GetByNameAsync(name);

                if (existingPlayer != null)
                {
                    // Si no cambian ni foto ni goles, saltamos
                    if (existingPlayer.Photo == photoUrl && existingPlayer.Goals == goals)
                        continue;

                    // Actualizamos los campos que han cambiado
                    existingPlayer.Update(
                        existingPlayer.Name,                         // Mantenemos el nombre
                        TryParsePosition(position),                  // Convertimos string → enum
                        new PlayerAge(age),                          // Wrappeamos el age
                        goals,                                       // Goals es int
                        photoUrl,                                    // Url de la foto
                        existingPlayer.CreatedAt                     // O DateTime.UtcNow si prefieres
                    );

                    await _playerRepo.UpdateAsync(existingPlayer);
                }
                else
                {
                    // Creamos nuevo jugador con foto y lo asociamos al equipo
                    var created = await _playerRepo.AddAsync(new Player(
                        new PlayerID(0),
                        new PlayerName(name),
                        TryParsePosition(position),
                        new PlayerAge(age),
                        goals,
                        photo: photoUrl,
                        createdAt: DateTime.UtcNow,
                        teamPlayers: new List<TeamPlayer>()
                    ));

                    await _tpRepo.AddAsync(team.TeamID, created.PlayerID);
                }
            }
        }

        private PlayerPosition TryParsePosition(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return PlayerPosition.JUGADOR;

            raw = raw.ToUpper().Replace(" ", "");

            return raw switch
            {
                "JUGADOR" => PlayerPosition.JUGADOR,
                "INVITADO" => PlayerPosition.INVITADO,
                "ENTRENADOR" => PlayerPosition.ENTRENADOR,
                "AYTE_ENTRENADOR" => PlayerPosition.AYTE_ENTRENADOR,
                "OFICIAL" => PlayerPosition.OFICIAL,
                "STAFF_ADICIONAL" => PlayerPosition.STAFF_ADICIONAL,
                _ => PlayerPosition.STAFF_ADICIONAL,
            };
        }



    }

}
