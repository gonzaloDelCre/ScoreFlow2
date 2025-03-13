using Domain.Entities.Users;
using Domain.Ports.Users;
using Infrastructure.Persistence.Conection;
using Microsoft.Extensions.Logging;
using Infrastructure.Persistence.Users.Mapper;
using Microsoft.EntityFrameworkCore;

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
                var dbUsers = await _context.Users.ToListAsync();
                return dbUsers.Select(_mapper.MapToDomain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de usuarios");
                return new List<User>();
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                var dbUser = await _context.Users.FindAsync(id);
                return dbUser != null ? _mapper.MapToDomain(dbUser) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {UserID}", id);
                return null;
            }
        }

        public async Task<User> AddAsync(User user)
        {
            try
            {
                var userEntity = _mapper.MapToEntity(user);
                _context.Users.Add(userEntity);
                await _context.SaveChangesAsync();
                return _mapper.MapToDomain(userEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo usuario");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID {UserID}", id);
                return false;
            }
        }
    }
}
