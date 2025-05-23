﻿namespace Domain.Entities.Players
{
    public class PlayerName
    {
        public string Value { get; private set; }

        public PlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("El nombre del jugador no puede estar vacío.");
            if (value.Length > 100)
                throw new ArgumentException("El nombre del jugador es demasiado largo.");
            Value = value;
        }

        public static implicit operator string(PlayerName playerName) => playerName.Value;
        public static implicit operator PlayerName(string value) => new PlayerName(value);
    }
}
