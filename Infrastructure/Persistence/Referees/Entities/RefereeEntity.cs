using Infrastructure.Persistence.MatchReferees.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Referees.Entities
{
    public class RefereeEntity
    {
        [Key]
        public int RefereeID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public ICollection<MatchRefereeEntity> MatchReferees { get; set; }
    }
}
