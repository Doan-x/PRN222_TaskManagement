using PRN222_TaskManagement.Models;

namespace PRN222_TaskManagement.Services.Implement
{
    public class EventShareServiceImplement : BaseServiceImplement<EventShare, Int32>, IEventShareService
    {
        public EventShareServiceImplement(Prn222TaskManagementContext context, ILogger<BaseServiceImplement<EventShare, int>> logger) : base(context, logger)
        {
        }
    }
}
