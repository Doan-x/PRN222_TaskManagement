using PRN222_TaskManagement.Models;

namespace PRN222_TaskManagement.Services
{
    public interface IUserService : IBaseService<User, Int32>
    {
        Task<User> GetByEmailAsync(string email);

    }
}
