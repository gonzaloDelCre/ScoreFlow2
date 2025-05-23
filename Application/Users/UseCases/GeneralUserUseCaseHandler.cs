﻿using Application.Users.DTOs;
using Application.Users.UseCases.Access;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
using Application.Users.UseCases.Profile;
using Application.Users.UseCases.Update;
using Domain.Shared;
using System;
using System.Threading.Tasks;

namespace Application.Users.UseCases
{
    public class GeneralUserUseCaseHandler
    {
        private readonly GetUserByIdUseCase _getUserById;
        private readonly CreateUserUseCase _createUser;
        private readonly UpdateUserUseCase _updateUser;
        private readonly DeleteUserUseCase _deleteUser;
        private readonly GetAllUsersUseCase _getAllUsers;
        private readonly GetUserByEmailUseCase _getUserByEmail;
        private readonly LoginUserUseCase _loginUser;
        private readonly RegisterUserUseCase _registerUser;
        private readonly GuestLoginUseCase _guestLoginUseCase;
        private readonly UpdateUserProfileUseCase _updateUserProfile;
        private readonly GetUserProfileUseCase _getUserProfile;

        public GeneralUserUseCaseHandler(
            GetUserByIdUseCase getUserById,
            CreateUserUseCase createUser,
            UpdateUserUseCase updateUser,
            DeleteUserUseCase deleteUser,
            GetAllUsersUseCase getAllUsers,
            GetUserByEmailUseCase getUserByEmail,
            LoginUserUseCase loginUser,
            RegisterUserUseCase registerUser,
            GuestLoginUseCase guestLoginUseCase,
            UpdateUserProfileUseCase updateUserProfile,
            GetUserProfileUseCase getUserProfile)
        {
            _getUserById = getUserById;
            _createUser = createUser;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
            _getAllUsers = getAllUsers;
            _getUserByEmail = getUserByEmail;
            _loginUser = loginUser;
            _registerUser = registerUser;
            _guestLoginUseCase = guestLoginUseCase;
            _updateUserProfile = updateUserProfile;
            _getUserProfile = getUserProfile;
        }

        public async Task<object> GetAllUsersAsync()
        {
            return await _getAllUsers.ExecuteAsync();
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id)
        {
            return await _getUserById.ExecuteAsync(id);
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            return await _getUserByEmail.ExecuteAsync(email);
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserRequestDTO userDTO)
        {
            return await _createUser.ExecuteAsync(userDTO);
        }

        public async Task<object> UpdateUserAsync(int id, UserRequestDTO userDTO)
        {
            userDTO.UserID = id;
            return await _updateUser.ExecuteAsync(userDTO);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _deleteUser.ExecuteAsync(new UserID(id));
        }

        public async Task<UserResponseDTO> RegisterUserAsync(RegisterRequestDTO registerRequest)
        {
            return await _registerUser.ExecuteAsync(registerRequest);
        }

        public async Task<UserResponseDTO> LoginUserAsync(string email, string password)
        {
            return await _loginUser.ExecuteAsync(email, password);
        }

        public async Task<UserResponseDTO> GuestLoginAsync()
        {
            return await _guestLoginUseCase.ExecuteAsync();
        }

        public async Task<UserProfileResponseDTO> GetUserProfileAsync(UserID userId)
        {
            return await _getUserProfile.ExecuteAsync(userId);
        }

        public async Task<UserProfileResponseDTO> UpdateUserProfileAsync(UserID userId, UserProfileUpdateDTO updateDTO)
        {
            return await _updateUserProfile.ExecuteAsync(userId, updateDTO);
        }

    }
}
