using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN222_TaskManagement.Mail;
using PRN222_TaskManagement.Models;
using PRN222_TaskManagement.Services;
using System.Globalization;
using System.Security.Claims;
using TrongND;

namespace PRN222_TaskManagement.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly MailHelper _mail;
        private readonly IEventShareService _eventShareS;
        public EventController(IEventService eventService, ILogger<EventController> logger, 
            ICategoryService categoryService, IUserService userService,
            MailHelper mail, IEventShareService eventShareService)
        {
            _eventService = eventService;
            _logger = logger;
            _categoryService = categoryService;
            _userService = userService;
            _mail = mail;
            _eventShareS = eventShareService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var user = HttpContext.User;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;


            User account = await _userService.GetByEmailAsync(userEmail);

            _logger.LogInformationWithColor("Get all events");
            var events = await _eventService.GetAllAsync();

            var model = events.Where(e => e.UserId == account.UserId);
            return new JsonResult(model);

        }

        [HttpGet("/event/form")]
        public async Task<IActionResult> GetEventForm(int id, string? date = null)
        {
            _logger.LogInformationWithColor("Get event form " + id + " " + date);

            var user = HttpContext.User;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;


            User account = await _userService.GetByEmailAsync(userEmail);

            var eventItem = await _eventService.GetByIdAsync(id) ?? new Event();


            // Create event
            if (id == 0 && !string.IsNullOrEmpty(date))
            {
                if (long.TryParse(date, out long timestamp))
                {
                    DateTime dateParsed = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;

                    eventItem.StartTime = dateParsed;
                    eventItem.EndTime = dateParsed;
                }
                else
                {
                    Console.WriteLine("Lỗi: Không thể parse timestamp");
                }

            }
            _logger.LogInformationWithColor("Provide event for form");
            var categories = await _categoryService.GetAllAsync();
            ViewData["Categories"] = categories;
            return PartialView("_EventForm", eventItem);
        }

        [HttpPost("/event/saveEvent")]
        public async Task<IActionResult> SaveEventMove(int eventId, string startTime, string endTime)
        {
            if (eventId <= 0)
            {
                return BadRequest(new { status = false, message = "EventId không hợp lệ!" });
            }


            var existingEvent = await _eventService.GetByIdAsync(eventId);
            if (existingEvent != null)
            {
                string format = "yyyy-MM-dd HH:mm";
                CultureInfo culture = CultureInfo.InvariantCulture;

                DateTime? startDateTime = null;
                if (!string.IsNullOrEmpty(startTime) && startTime != "null")
                {
                    startDateTime = DateTime.ParseExact(startTime, format, culture, DateTimeStyles.AssumeUniversal);
                    existingEvent.StartTime = startDateTime.Value;
                }

                DateTime? endDateTime = null;
                if (!string.IsNullOrEmpty(endTime) && endTime != "null")
                {
                    endDateTime = DateTime.ParseExact(endTime, format, culture, DateTimeStyles.AssumeUniversal);
                    existingEvent.EndTime = endDateTime.Value;
                }

                existingEvent.UpdatedAt = DateTime.Now;
                await _eventService.UpdateAsync(existingEvent);
            }
            else
            {
                return NotFound(new { status = false, message = "Sự kiện không tồn tại!" });
            }

            return Ok(new { status = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting event with ID: {id}");

                var eventToDelete = await _eventService.GetByIdAsync(id);
                if (eventToDelete == null)
                {
                    return Json(new { status = false, message = "Event not found!" });
                }
                else
                {
                    await _eventService.DeleteAsync(id);
                }

                _logger.LogInformation($"Event with ID: {id} deleted successfully.");
                return Json(new { status = true, message = "Event deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting event: {ex.Message}");
                return Json(new { status = false, message = "Error deleting event!" });
            }
        }

        [HttpGet("/event/detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        [HttpGet("/event/delete/{id}")]
        public async Task<IActionResult> Delete(int id, string? returnUrl)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            else
            {
                await _eventService.DeleteAsync(id);
            }
            TempData["Success"] = "Delete event successfully";
            return Redirect(returnUrl ?? Url.Action("Index"));
        }

        [HttpGet]
        public async Task<IActionResult> List(string? titleSearch,
               int? categoryId,
               string? priority)
        {
            var user = HttpContext.User;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;


            User account = await _userService.GetByEmailAsync(userEmail);

            var today = DateTime.Today;
            var eventsQuery1 = await _eventService.GetAllAsync();


            var eventsQuery = eventsQuery1.Where(e => e.UserId == account.UserId);
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

            ViewData["Categories"] = await _categoryService.GetAllAsync();

            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedPriority = priority;
            return View(eventsQuery);
        }

        [HttpGet]
        public async Task<IActionResult> SaveFromCalendar(string id, string start, string end)
        {
            try
            {
                _logger.LogInformationWithColor("Tine" + start);
                int eventId = int.Parse(id);

                var eventModel = new Event();

                string format = "yyyy-MM-dd HH:mm";
                CultureInfo culture = CultureInfo.InvariantCulture;

                DateTime? startDateTime = null;
                if (!string.IsNullOrEmpty(start) && start != "null")
                {
                    startDateTime = DateTime.ParseExact(start, format, culture);
                    eventModel.StartTime = startDateTime.Value;
                }

                DateTime? endDateTime = null;
                if (!string.IsNullOrEmpty(end) && end != "null")
                {
                    endDateTime = DateTime.ParseExact(end, format, culture);
                    eventModel.EndTime = endDateTime.Value;
                }

                if (eventId != 0)
                {
                    eventModel = await _eventService.GetByIdAsync(eventId);
                }
                _logger.LogInformationWithColor("Save from calendar event id: " + eventId);
                var categories = await _categoryService.GetAllAsync();
                ViewData["Categories"] = categories.ToList();

                return View("Save", eventModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xử lý dữ liệu!", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Save(int? id, string returnUrl)
        {

            var categories = await _categoryService.GetAllAsync();
            ViewBag.ReturnUrl = returnUrl;
            ViewData["Categories"] = categories.ToList();

            if (id != null)
            {
                var model = await _eventService.GetByIdAsync(id.Value);
                if (model == null)
                {
                    return NotFound();
                }
                else { return View(model); }
            }

            return View();
        }

        [HttpPost("/event/save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Event model, string? returnUrl)
        {
            _logger.LogInformationWithColor("save event with id: " + model.EventId);
            var categories = await _categoryService.GetAllAsync();
            ViewBag.ReturnUrl = returnUrl;
            ViewData["Categories"] = categories.ToList();


            if (model.StartTime >= model.EndTime)
            {
                TempData["Error"] = "Save Failed: Invalid start time";
                ModelState.AddModelError("StartTime", "Start time must not be greater than end time");
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Error"] = $"Save Failed: {string.Join(", ", errors)}";

                _logger.LogInformationWithColor("Save Failed: " + string.Join(", ", errors));

                return View(model);
            }
            else
            {
                var user = HttpContext.User;
                var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;


                User account = await _userService.GetByEmailAsync(userEmail);

                model.User = account;
                if (model.EventId == 0)
                {
                    await _eventService.AddAsync(model);  // Thêm mới
                }
                else
                {
                    await _eventService.UpdateAsync(model);  // Cập nhật
                }
                _logger.LogInformationWithColor("Save event success");

                TempData["Success"] = "Save event successfully";
            }
            if (!returnUrl.IsNullOrEmpty())
            {
                return Redirect(returnUrl);
            }

            return View("Index");
        }

        [HttpPost]
        [Route("event/share")]
        public async Task<IActionResult> ShareEvent([FromBody] ShareEventRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Subject) ||
                string.IsNullOrWhiteSpace(request.Body))
            {
                return BadRequest(new {status = false, message = "⚠️ Vui lòng điền đầy đủ thông tin!" });
            }


            try
            {
                await _mail.SendGmail(request.Email, request.Subject, request.Body);
                _logger.LogInformationWithColor($"Email sent to {request.Email} with subject {request.Subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email} with subject {Subject}", request.Email, request.Subject);

                return BadRequest(new { status = false, message = "Send mail failed" });
            }
            var eventShare = new EventShare
            {
                EventId = request.EventId,
                SharedWithEmail = request.Email,
                SharedAt = DateTime.Now
            };
            await _eventShareS.AddAsync(eventShare);

            return Ok(new { status = true, message = "📤 Email đã được gửi thành công!" });

        }
    }
    public class ShareEventRequest
    {
        public int EventId { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }


}
