﻿using Application.Playes.DTOs;
using Domain.Entities.Players;
using Domain.Ports.Players;
using Domain.Services.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Playes.UseCases.Create
{
    public class CreatePlayer
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly PlayerService _playerService;

        public CreatePlayer(IPlayerRepository playerRepository, PlayerService playerService)
        {
            _playerRepository = playerRepository;
            _playerService = playerService;
        }

        public async Task<PlayerResponseDTO> Execute(PlayerRequestDTO playerDTO)
        {
            if (playerDTO == null)
                throw new ArgumentNullException(nameof(playerDTO), "Los detalles del jugador no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(playerDTO.Name))
                throw new ArgumentException("El nombre del jugador es obligatorio.");

            var existingPlayer = await _playerRepository.GetByNameAsync(playerDTO.Name);
            if (existingPlayer != null)
                throw new ArgumentException("Ya existe un jugador con el mismo nombre.");

            var player = await _playerService.CreatePlayerAsync(
                new PlayerName(playerDTO.Name),
                playerDTO.TeamID,
                playerDTO.Position,
                playerDTO.CreatedAt
            );

            return new PlayerResponseDTO
            {
                PlayerID = player.PlayerID,
                Name = player.Name.Value,
                TeamID = player.TeamID,
                Position = player.Position,
                CreatedAt = player.CreatedAt
            };
        }
    }
}
