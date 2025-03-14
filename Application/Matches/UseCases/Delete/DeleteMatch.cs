using Domain.Ports.Matches;
using Domain.Services.Matches;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Matches.UseCases.Delete
{
    public class DeleteMatch
    {
        private readonly MatchService _matchService;

        public DeleteMatch(MatchService matchService)
        {
            _matchService = matchService;
        }

        // Ejecuta la eliminación de un partido por su ID
        public async Task<bool> Execute(MatchID matchID)
        {
            if (matchID == null)
                throw new ArgumentNullException(nameof(matchID), "El ID del partido no puede ser nulo.");

            var match = await _matchService.GetMatchByIdAsync(matchID);
            if (match == null)
                throw new InvalidOperationException("El partido no existe. No se puede eliminar.");

            return await _matchService.DeleteMatchAsync(matchID);
        }
    }
}
