using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN222_TaskManagement.Models;
using PRN222_TaskManagement.Services;
using TrongND;

namespace PRN222_TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, IEventService eventService, ICategoryService categoryService)
        {
            _logger = logger;
            _eventService = eventService;
            _categoryService = categoryService;
        }

        [Authorize]
        public async Task<IActionResult> Index(
            string? titleSearch,
               int? categoryId,
               string? priority,
               string sortField = "start_time",
               string sortDirection = "asc")
        {
            var today = DateTime.Today;
            var eventsQuery = await _eventService.GetByConditionAsync(e => e.StartTime.Date == today);

            if (!string.IsNullOrEmpty(titleSearch))
            {

                eventsQuery = await _eventService.GetByConditionAsync(e => e.Title.Contains(titleSearch));
                TempData["titleSearch"] = titleSearch;
            }

            // Lọc theo category_id nếu có
            if (categoryId.HasValue)
            {
                eventsQuery = eventsQuery.Where(e => e.CategoryId == categoryId.Value);
            }

            // Lọc theo priority nếu có
            if (!string.IsNullOrEmpty(priority))
            {
                eventsQuery = eventsQuery.Where(e => e.Priority == priority);
            }

            // Sắp xếp dựa trên tiêu chí được người dùng chọn
            switch (sortField)
            {
                case "start_time":
                    if (sortDirection == "desc")
                        eventsQuery = eventsQuery.OrderByDescending(e => e.StartTime);
                    else
                        eventsQuery = eventsQuery.OrderBy(e => e.StartTime);
                    break;

                case "priority":
                    //  low (1), medium (2), high (3)
                    if (sortDirection == "asc")
                    {
                        eventsQuery = eventsQuery.OrderBy(e =>
                            e.Priority == "low" ? 1 :
                            e.Priority == "medium" ? 2 : 3);
                    }
                    else // descending: high trước, sau đó medium, low
                    {
                        eventsQuery = eventsQuery.OrderByDescending(e =>
                            e.Priority == "high" ? 3 :
                            e.Priority == "medium" ? 2 : 1);
                    }
                    break;

                default:
                    // Mặc định sắp xếp theo thời gian bắt đầu
                    eventsQuery = eventsQuery.OrderBy(e => e.StartTime);
                    break;
            }
            
            
            var categories = await _categoryService.GetAllAsync();

            ViewData["Categories"] = categories.ToList();

            if (!titleSearch.IsNullOrEmpty())
            {
                return RedirectToAction("List", "Event");
            }

            return View(eventsQuery);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}


