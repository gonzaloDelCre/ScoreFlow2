using Domain.Entities.Users;
using Domain.Ports.Users;
using Domain.Shared;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Users.Mapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        private readonly UserMapper _mapper;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger, UserMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                string sql = "SELECT * FROM Users";
                var dbUsers = await _context.Users
                    .FromSqlRaw(sql)
                    .ToListAsync();

                return dbUsers.Select(dbUser => _mapper.MapToDomain(dbUser)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios");
                return new List<User>();
            }
        }
        public async Task<User?> GetByIdAsync(UserID userId)
        {
            try
            {
                string sql = "SELECT * FROM Users WHERE UserID = @UserID";
                var parameter = new SqlParameter("@UserID", userId.Value);

                var dbUsers = await _context.Users
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbUser = dbUsers.FirstOrDefault();
                return dbUser != null ? _mapper.MapToDomain(dbUser) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserID}", userId.Value);
                return null;
            }
        }

        public async Task<User> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser null");

            try
            {
                var userEntity = _mapper.MapToEntity(user);

                var role = userEntity.Role != null ? userEntity.Role.ToString() : "Espectador";

                string insertSql = @"INSERT INTO Users (FullName, Email, PasswordHash, CreatedAt, Role) 
                             VALUES (@FullName, @Email, @PasswordHash, @CreatedAt, @Role);";

                var parameters = new[]
                {
                    new SqlParameter("@FullName", userEntity.FullName),
                    new SqlParameter("@Email", userEntity.Email),
                    new SqlParameter("@PasswordHash", userEntity.PasswordHash),
                    new SqlParameter("@CreatedAt", userEntity.CreatedAt),
                    new SqlParameter("@Role", role)
                };

                await _context.Database.ExecuteSqlRawAsync(insertSql, parameters);

                string selectSql = "SELECT TOP 1 UserID FROM Users ORDER BY UserID DESC";
                var newUserId = await _context.Users
                    .FromSqlRaw(selectSql)
                    .Select(u => u.UserID)
                    .FirstOrDefaultAsync();

                return new User(
                    new UserID(newUserId),
                    user.FullName,
                    user.Email,
                    user.PasswordHash,
                    user.Role,
                    user.CreatedAt
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo usuario");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(UserID userId)
        {
            try
            {
                string sql = "DELETE FROM Users WHERE UserID = @UserID";
                var parameter = new SqlParameter("@UserID", userId.Value);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID {UserID}", userId.Value);
                return false;
            }
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser null");

            try
            {
                var userEntity = _mapper.MapToEntity(user);

                string updateSql = @"UPDATE Users 
                                     SET FullName = @FullName, Email = @Email, PasswordHash = @PasswordHash, CreatedAt = @CreatedAt
                                     WHERE UserID = @UserID";

                var parameters = new[]
                {
                    new SqlParameter("@UserID", userEntity.UserID),
                    new SqlParameter("@FullName", userEntity.FullName),
                    new SqlParameter("@Email", userEntity.Email),
                    new SqlParameter("@PasswordHash", userEntity.PasswordHash),
                    new SqlParameter("@CreatedAt", userEntity.CreatedAt)
                };

                await _context.Database.ExecuteSqlRawAsync(updateSql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID {UserID}", user.UserID.Value);
                throw;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                string sql = "SELECT * FROM Users WHERE Email = @Email";
                var parameter = new SqlParameter("@Email", email);

                var dbUsers = await _context.Users
                    .FromSqlRaw(sql, parameter)
                    .ToListAsync();

                var dbUser = dbUsers.FirstOrDefault();
                return dbUser != null ? _mapper.MapToDomain(dbUser) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con correo electrónico {Email}", email);
                return null;
            }
        }
    }
}
