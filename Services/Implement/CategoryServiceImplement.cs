using PRN222_TaskManagement.Models;

namespace PRN222_TaskManagement.Services.Implement
{
    public class CategoryServiceImplement : BaseServiceImplement<Category, Int32>, ICategoryService
    {
        public CategoryServiceImplement(Prn222TaskManagementContext context, ILogger<BaseServiceImplement<Category, int>> logger) : base(context, logger)
        {
        }
    }
}
