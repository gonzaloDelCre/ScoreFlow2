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
            // 1) Obtiene el equipo por ExternalID
            var externalId = teamExternalId.ToString();
            var team = await _teamRepo.GetByExternalIdAsync(externalId);
            if (team == null)
                throw new InvalidOperationException($"Equipo con ExternalID={externalId} no existe.");

            // 2) Scrapea la plantilla de jugadores
            var scraped = await _scraper.GetPlayersByTeamExternalIdAsync(teamExternalId);

            // 3) Para cada jugador scrappeado, añadir o actualizar
            foreach (var (name, age, positionRaw, goals, _, photoUrl) in scraped)
            {
                // 3.1) Intentamos recuperar por nombre
                var existing = await _playerRepo.GetByNameAsync(name);
                var pos = TryParsePosition(positionRaw);
                var ageVo = new PlayerAge(age);

                if (existing != null)
                {
                    // 3.2) Si cambian foto o goles, actualizamos
                    if (existing.Photo != photoUrl || existing.Goals != goals)
                    {
                        existing.Update(
                            name: existing.Name,         // no cambiamos nombre
                            position: pos,
                            age: ageVo,
                            goals: goals,
                            photo: photoUrl,
                            createdAt: existing.CreatedAt     // mantenemos CreatedAt original
                        );

                        await _playerRepo.UpdateAsync(existing);
                    }
                }
                else
                {
                    // 3.3) Si no existe, lo creamos
                    var toCreate = new Player(
                        playerID: new PlayerID(0),
                        name: new PlayerName(name),
                        position: pos,
                        age: ageVo,
                        goals: goals,
                        photo: photoUrl,
                        createdAt: DateTime.UtcNow,
                        teamPlayers: new List<TeamPlayer>()  // vacío, se vincula más abajo
                    );

                    var created = await _playerRepo.AddAsync(toCreate);
                    var role = TryParseRole(positionRaw);

                    // 3.4) Y vinculamos el nuevo jugador al equipo
                    var tp = new TeamPlayer(
                        team.TeamID,
                        created.PlayerID,
                        new JoinedAt(DateTime.UtcNow),
                        role
                    );
                    await _tpRepo.AddAsync(tp);
                }
            }
        }

        private PlayerPosition TryParsePosition(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return PlayerPosition.JUGADOR;

            var key = raw.Trim().ToUpper().Replace(" ", "_");
            return key switch
            {
                "JUGADOR" => PlayerPosition.JUGADOR,
                "INVITADO" => PlayerPosition.INVITADO,
                "ENTRENADOR" => PlayerPosition.ENTRENADOR,
                "AYTE_ENTRENADOR" => PlayerPosition.AYTE_ENTRENADOR,
                "OFICIAL" => PlayerPosition.OFICIAL,
                "STAFF_ADICIONAL" => PlayerPosition.STAFF_ADICIONAL,
                _ => PlayerPosition.JUGADOR
            };
        }
        private RoleInTeam? TryParseRole(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            return raw.Trim().ToUpper() switch
            {
                "JUGADOR" => RoleInTeam.JUGADOR,
                "INVITADO" => RoleInTeam.INVITADO,
                "ENTRENADOR" => RoleInTeam.ENTRENADOR,
                "AYTE_ENTRENADOR" => RoleInTeam.AYTE_ENTRENADOR,
                "OFICIAL" => RoleInTeam.OFICIAL,
                "STAFF_ADICIONAL" => RoleInTeam.STAFF_ADICIONAL,
                _ => RoleInTeam.STAFF_ADICIONAL
            };
        }

    }

}
