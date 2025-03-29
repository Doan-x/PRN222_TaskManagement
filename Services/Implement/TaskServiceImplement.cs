using PRN222_TaskManagement.Models;

namespace PRN222_TaskManagement.Services.Implement
{
    public class TaskServiceImplement : BaseServiceImplement<PRN222_TaskManagement.Models.Task, Int32>, ITaskService
    {
        public TaskServiceImplement(Prn222TaskManagementContext context, ILogger<BaseServiceImplement<Models.Task, int>> logger) : base(context, logger)
        {
        }
    }
}
