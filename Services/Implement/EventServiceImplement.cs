using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PRN222_TaskManagement.Models;
using System.Linq.Expressions;
using TrongND;

namespace PRN222_TaskManagement.Services.Implement
{
    public class EventServiceImplement : BaseServiceImplement<Event, int>, IEventService
    {
        public EventServiceImplement(Prn222TaskManagementContext context, ILogger<EventServiceImplement> logger) : base(context,logger)
        {
            
        }

        public override async Task<IEnumerable<Event>> GetAllAsync()
        {
            _logger.LogInformationWithColor("Get all events:");
            return await _context.Events
                .Include(e => e.Category)
                .Include(e =>e.EventShares).ToListAsync();
        }

        public override async Task<Event> GetByIdAsync(int id)
        {
            _logger.LogInformationWithColor("Get event by id: "+id);

            var eventSearch = await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.EventId == id);
            if(eventSearch == null)
            {
                _logger.LogInformationWithColor($"Found event with id: {id} failed");
            }
            return eventSearch;
        }

        public override async Task<IEnumerable<Event>> GetByConditionAsync(Expression<Func<Event, bool>> predicate)
        {
            var events = await _context.Events.Where(predicate).Include(e => e.Category).ToListAsync();
            _logger.LogInformationWithColor("Get event by condition: ");
            return events;
        }
    }
}
