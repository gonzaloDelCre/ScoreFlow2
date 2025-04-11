using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Services.Scraping.Teams
{
    public class TeamScraperService
    {
        private const string Url = "https://www.rfebm.com/competiciones/clasificacion.php?seleccion=0&id=1025342&id_ambito=0";

        public async Task<List<TeamRequestDTO>> ScrapeTeamsAsync()
        {
            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(Url);

            // Seleccionamos todas las filas de la tabla que contienen equipos
            var teamRows = document.DocumentNode.SelectNodes("//table[@class='table table-striped clasificacion']/tbody/tr");

            if (teamRows == null)
                return new List<TeamRequestDTO>();

            var teams = new List<TeamRequestDTO>();

            foreach (var row in teamRows)
            {
                // Extraemos el ID del equipo desde el enlace
                var teamLinkNode = row.SelectSingleNode(".//td[@class='p-t-15'][1]/a");
                var teamIdHref = teamLinkNode?.GetAttributeValue("href", "");
                var teamId = ExtractTeamId(teamIdHref);

                // Extraemos el nombre del equipo
                var teamName = teamLinkNode?.InnerText.Trim();

                // Extraemos la URL del logo
                var logoNode = row.SelectSingleNode(".//td[@class='celda_peque'][1]/a/img");
                var logoUrl = logoNode?.GetAttributeValue("src", "");


                // Creamos el objeto DTO con los valores necesarios
                var team = new TeamRequestDTO
                {
                    TeamID = teamId ?? 0,
                    Name = teamName ?? string.Empty,
                    Logo = logoUrl ?? string.Empty,
                    PlayerIds = new List<int>() // Los IDs de jugadores no están en la tabla
                };


                teams.Add(team);
            }

            return teams;
        }


        private int? ExtractTeamId(string href)
        {
            if (string.IsNullOrEmpty(href)) return null;

            // Extraemos el parámetro id_equipo del enlace
            var idParam = "id_equipo=";
            var startIndex = href.IndexOf(idParam);

            if (startIndex == -1) return null;

            startIndex += idParam.Length;
            var endIndex = href.IndexOf('&', startIndex);

            var idValue = endIndex > startIndex
                ? href.Substring(startIndex, endIndex - startIndex)
                : href.Substring(startIndex);

            return int.TryParse(idValue, out var id) ? id : (int?)null;
        }
    }
}