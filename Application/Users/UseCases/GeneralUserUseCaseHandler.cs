using Application.Users.DTOs;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
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

        public GeneralUserUseCaseHandler(
            GetUserByIdUseCase getUserById,
            CreateUserUseCase createUser,
            UpdateUserUseCase updateUser,
            DeleteUserUseCase deleteUser,
            GetAllUsersUseCase getAllUsers,
            GetUserByEmailUseCase getUserByEmail)
        {
            _getUserById = getUserById;
            _createUser = createUser;
            _updateUser = updateUser;
            _deleteUser = deleteUser;
            _getAllUsers = getAllUsers;
            _getUserByEmail = getUserByEmail;
        }

        public async Task<object> ExecuteAsync(UserActionDTO actionDTO)
        {
            if (string.IsNullOrWhiteSpace(actionDTO.Action))
                throw new ArgumentException("La acción no puede estar vacía.");

            switch (actionDTO.Action.ToLower())
            {
                case "getall":
                    return await _getAllUsers.ExecuteAsync();

                case "getbyid":
                    if (actionDTO.UserID == null)
                        throw new ArgumentException("El ID del usuario es necesario para obtenerlo.");
                    return await _getUserById.ExecuteAsync(actionDTO.UserID.Value);

                case "getbyemail":
                    if (actionDTO.User?.Email == null)
                        throw new ArgumentException("El correo electrónico es necesario para obtener al usuario.");
                    return await _getUserByEmail.ExecuteAsync(actionDTO.User.Email);

                case "add":
                    if (actionDTO.User == null)
                        throw new ArgumentException("Los detalles del usuario son necesarios para agregarlo.");
                    return await _createUser.ExecuteAsync(actionDTO.User);

                case "update":
                    if (actionDTO.User == null)
                        throw new ArgumentException("Los detalles del usuario son necesarios para actualizarlo.");
                    return await _updateUser.ExecuteAsync(actionDTO.User);

                case "delete":
                    if (actionDTO.UserID == null)
                        throw new ArgumentException("El ID del usuario es necesario para eliminarlo.");
                    return await _deleteUser.ExecuteAsync(new UserID(actionDTO.UserID.Value));

                default:
                    throw new ArgumentException($"Acción '{actionDTO.Action}' no soportada.");
            }
        }

    }
}
