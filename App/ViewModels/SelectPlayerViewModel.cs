using App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class SelectPlayerViewModel : BindableObject
    {
        public ObservableCollection<Player> Players { get; set; }
        public Command<Player> SelectPlayerCommand { get; }

        public SelectPlayerViewModel()
        {
            Players = new ObservableCollection<Player>
        {
            new Player { Name = "Shinchan", Position = "Mediocentro", Number = 80, Team = "San Agustin", ImageUrl = "shinchan.png" },
            new Player { Name = "Shrek", Position = "Lateral Izq", Number = 18, Team = "San Agustin", ImageUrl = "shrek.png" }
        };

            SelectPlayerCommand = new Command<Player>(OnSelectPlayer);
        }

        private async void OnSelectPlayer(Player selectedPlayer)
        {
            // Aquí deberías actualizar la posición seleccionada con el jugador escogido
            await Shell.Current.GoToAsync(".."); // Volver a la alineación
        }
    }

}
