using Domain.Entities.Users;
using Domain.Enum;
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
        private readonly IUserMapper _mapper;

        public UserRepository(
            ApplicationDbContext context,
            ILogger<UserRepository> logger,
            IUserMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Añade un nuevo usuario al sistema
        /// </summary>
        /// <param name="user">Usuario a añadir</param>
        /// <returns>Usuario creado con ID asignado</returns>
        public async Task<User> AddAsync(User user)
        {
            var entity = _mapper.ToEntity(user);

            const string sql = @"
                INSERT INTO Users (FullName, Email, PasswordHash, Role, CreatedAt)
                VALUES (@FullName, @Email, @PasswordHash, @Role, @CreatedAt)";

            var parameters = new[]
            {
                new SqlParameter("@FullName", entity.FullName),
                new SqlParameter("@Email", entity.Email),
                new SqlParameter("@PasswordHash", entity.PasswordHash),
                new SqlParameter("@Role", entity.Role),
                new SqlParameter("@CreatedAt", entity.CreatedAt)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            var newId = await _context.Users
                .FromSqlRaw("SELECT TOP 1 * FROM Users ORDER BY UserID DESC")
                .Select(u => u.UserID)
                .FirstAsync();

            var newEntity = await _context.Users
                .FromSqlRaw("SELECT * FROM Users WHERE UserID = @UserID", new SqlParameter("@UserID", newId))
                .FirstAsync();

            return _mapper.ToDomain(newEntity);
        }

        /// <summary>
        /// Autentica un usuario con email y contraseña
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <param name="passwordHash">Hash de la contraseña</param>
        /// <returns>Usuario autenticado o null si las credenciales son inválidas</returns>
        public async Task<User?> AuthenticateAsync(string email, string passwordHash)
        {
            var entity = await _context.Users
                .FromSqlRaw("SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash",
                    new SqlParameter("@Email", email),
                    new SqlParameter("@PasswordHash", passwordHash))
                .FirstOrDefaultAsync();

            return entity == null ? null : _mapper.ToDomain(entity);
        }

        /// <summary>
        /// Elimina un usuario del sistema
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>true si se eliminó, false si no existía</returns>
        public async Task<bool> DeleteAsync(UserID userId)
        {
            const string sql = "DELETE FROM Users WHERE UserID = @UserID";

            var rows = await _context.Database
                .ExecuteSqlRawAsync(sql, new SqlParameter("@UserID", userId.Value));

            return rows > 0;
        }

        /// <summary>
        /// Verifica si existe un usuario con el email especificado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>true si existe, false si no</returns>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var count = await _context.Users
                                      .AsNoTracking()
                                      .CountAsync(u => u.Email == email);
            return count > 0;
        }


        /// <summary>
        /// Obtiene todos los usuarios del sistema
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var entities = await _context.Users
                .FromSqlRaw("SELECT * FROM Users")
                .ToListAsync();

            return entities.Select(e => _mapper.ToDomain(e));
        }

        /// <summary>
        /// Obtiene un usuario por su email
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <returns>Usuario o null si no existe</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            var entity = await _context.Users
                .FromSqlRaw("SELECT * FROM Users WHERE Email = @Email",
                    new SqlParameter("@Email", email))
                .FirstOrDefaultAsync();

            return entity == null ? null : _mapper.ToDomain(entity);
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Usuario o null si no existe</returns>
        public async Task<User?> GetByIdAsync(UserID userId)
        {
            var entity = await _context.Users
                .FromSqlRaw("SELECT * FROM Users WHERE UserID = @UserID",
                    new SqlParameter("@UserID", userId.Value))
                .FirstOrDefaultAsync();

            return entity == null ? null : _mapper.ToDomain(entity);
        }

        /// <summary>
        /// Obtiene todos los usuarios con un rol específico
        /// </summary>
        /// <param name="role">Rol a filtrar</param>
        /// <returns>Lista de usuarios con el rol especificado</returns>
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            var entities = await _context.Users
                .FromSqlRaw("SELECT * FROM Users WHERE Role = @Role",
                    new SqlParameter("@Role", role.ToString()))
                .ToListAsync();

            return entities.Select(e => _mapper.ToDomain(e));
        }

        /// <summary>
        /// Actualiza los datos de un usuario
        /// </summary>
        /// <param name="user">Usuario con los datos actualizados</param>
        public async Task UpdateAsync(User user)
        {
            var entity = _mapper.ToEntity(user);

            const string sql = @"
                UPDATE Users
                SET FullName = @FullName,
                    Email = @Email,
                    PasswordHash = @PasswordHash,
                    Role = @Role
                WHERE UserID = @UserID";

            var parameters = new[]
            {
                new SqlParameter("@UserID", entity.UserID),
                new SqlParameter("@FullName", entity.FullName),
                new SqlParameter("@Email", entity.Email),
                new SqlParameter("@PasswordHash", entity.PasswordHash),
                new SqlParameter("@Role", entity.Role)
            };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="fullName">Nombre completo del usuario</param>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña sin encriptar</param>
        /// <param name="role">Rol del usuario (opcional, por defecto User)</param>
        /// <returns>Usuario registrado con ID asignado o null si el email ya existe</returns>
        public async Task<User?> RegisterAsync(string fullName, string email, string password, UserRole role = UserRole.Jugador)
        {
            if (await ExistsByEmailAsync(email))
            {
                _logger.LogWarning("Intento de registro con email ya existente: {Email}", email);
                return null;
            }

            string passwordHash = HashPassword(password);

            var user = new User(
                userID: UserID.New(),  
                fullName: fullName,
                email: email,
                passwordHash: passwordHash,
                role: role,
                createdAt: DateTime.UtcNow
            );

            return await AddAsync(user);
        }

        /// <summary>
        /// Autentica un usuario con email y contraseña
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <param name="password">Contraseña sin encriptar</param>
        /// <returns>Usuario autenticado o null si las credenciales son inválidas</returns>
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Intento de login con email inexistente: {Email}", email);
                return null;
            }

            string passwordHash = HashPassword(password);
            if (!user.PasswordHash.Equals(passwordHash))
            {
                _logger.LogWarning("Intento de login con contraseña incorrecta para: {Email}", email);
                return null;
            }

            _logger.LogInformation("Login exitoso para: {Email}", email);
            return user;
        }

        /// <summary>
        /// Método privado para generar el hash de una contraseña
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Hash de la contraseña</returns>
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
