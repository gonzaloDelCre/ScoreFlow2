using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Position
    {
        public string Name { get; set; }
        public Player AssignedPlayer { get; set; } // Puede estar vacío si no se ha asignado jugador
    }

}
