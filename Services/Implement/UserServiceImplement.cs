using Microsoft.EntityFrameworkCore;
using PRN222_TaskManagement.Models;
using System.Linq.Expressions;
using TrongND;

namespace PRN222_TaskManagement.Services.Implement
{
    public class UserServiceImplement : IUserService
    {
        private readonly Prn222TaskManagementContext _context;
        private readonly ILogger<UserServiceImplement> _logger;

        public UserServiceImplement(Prn222TaskManagementContext context, ILogger<UserServiceImplement> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Task<User> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetByConditionAsync(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            _logger.LogInformationWithColor($"Get user by email: {email}");

            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Email.Equals(email));

            if(user == null)
            {
               _logger.LogInformationWithColor($"Get user by email failed");
            }
            else { _logger.LogInformationWithColor($"Get user by email success"); }

            return user;
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBaseService<User, int>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
