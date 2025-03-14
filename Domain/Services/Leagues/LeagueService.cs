using Domain.Entities.Leagues;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Leagues
{
    public class LeagueService
    {
        public void ValidateLeague(LeagueID leagueID, LeagueName name, string description, DateTime createdAt)
        {
            if (leagueID == null || leagueID.Value <= 0)
                throw new ArgumentException("El ID de la liga debe ser mayor que 0.", nameof(leagueID));

            if (name == null)
                throw new ArgumentNullException(nameof(name), "El nombre de la liga no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción de la liga no puede estar vacía.", nameof(description));

            if (description.Length > 500)
                throw new ArgumentException("La descripción de la liga no puede superar los 500 caracteres.", nameof(description));

            if (createdAt > DateTime.UtcNow)
                throw new ArgumentException("La fecha de creación no puede estar en el futuro.", nameof(createdAt));
        }

    }
}
