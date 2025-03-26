using App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class PlayerListViewModel : BindableObject
    {
        public ObservableCollection<Player> Players { get; set; }

        public PlayerListViewModel()
        {
            Players = new ObservableCollection<Player>
        {
            new Player { Name = "Gonzalo Delgado", Position = "Lateral Izq", Number = 18, Team = "San Agustin", ImageUrl = "shrek.png" },
            new Player { Name = "Shinchan", Position = "Central", Number = 80, Team = "San Agustin", ImageUrl = "shinchan.png" }
        };
        }
    }

}
