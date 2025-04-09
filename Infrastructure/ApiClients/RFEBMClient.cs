using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ApiClients
{
    public class RFEBMClient
    {
        private readonly HttpClient _httpClient;

        public RFEBMClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetClasificacionPageAsync()
        {
            var url = "https://www.rfebm.com/competiciones/clasificacion.php?seleccion=0&id=1025342&id_ambito=0";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }


    }
}
