using App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class LineupViewModel : BindableObject
    {
        public ObservableCollection<Player> Players { get; set; }
        public ObservableCollection<Position> Positions { get; set; }

        public Command<int> SelectPositionCommand { get; }

        public LineupViewModel()
        {
            Positions = new ObservableCollection<Position>
        {
            new Position { Name = "Delantero Izq" },
            new Position { Name = "Delantero Centro" },
            new Position { Name = "Delantero Der" },
            new Position { Name = "Mediocentro" },
            new Position { Name = "Defensa Izq" },
            new Position { Name = "Defensa Der" },
            new Position { Name = "Portero" }
        };

            Players = new ObservableCollection<Player>
        {
            new Player { Name = "Shinchan", Position = "Mediocentro", Number = 80, Team = "San Agustin", ImageUrl = "shinchan.png" },
            new Player { Name = "Shrek", Position = "Lateral Izq", Number = 18, Team = "San Agustin", ImageUrl = "shrek.png" }
        };

            SelectPositionCommand = new Command<int>(OnSelectPosition);
        }

        private async void OnSelectPosition(int positionIndex)
        {
            // Guardamos la posición seleccionada y vamos a la pantalla de selección de jugadores
            await Shell.Current.GoToAsync($"SelectPlayerPage?positionIndex={positionIndex}");
        }
    }


}
