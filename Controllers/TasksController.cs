using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN222_TaskManagement.Models;
using PRN222_TaskManagement.Services;
using TrongND;

namespace PRN222_TaskManagement.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, IUserService userService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _userService = userService;
            _logger = logger;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string status = "pending", string priority = "", string sortOrder = "asc")
        {
            var tasks = await _taskService.GetAllAsync();


            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                tasks = tasks.Where(t => t.Status == status);
            }


            if (!string.IsNullOrEmpty(priority))
            {
                tasks = tasks.Where(t => t.Priority == priority);
            }

            tasks = sortOrder == "desc"
                    ? tasks.OrderByDescending(t => t.DueDate)
                    : tasks.OrderBy(t => t.DueDate);

            // Giữ giá trị lọc khi reload
            ViewBag.Status = status;
            ViewBag.Priority = priority;
            ViewBag.SortOrder = sortOrder;


            return View(tasks.ToList());
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PRN222_TaskManagement.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                if (task.DueDate.HasValue && task.DueDate.Value < today)
                {
                    ModelState.AddModelError("DueDate", "Invalid due date");
                    return View(task);
                }

                var user = HttpContext.User;
                var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;

                if(userEmail == null)
                {
                    TempData["Error"] = "You must login to create task";
                    return RedirectToAction("Login", "Authentication");
                }
                task.User = _userService.GetByEmailAsync(userEmail).Result;

                await _taskService.AddAsync(task);
                TempData["Success"] = "Create task successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            else
            {
                var taskSelected = await _taskService.GetByIdAsync(id.Value);

                if(taskSelected == null)
                {
                    return NotFound();
                }
                else
                {
                   await _taskService.DeleteAsync(id.Value);
                    TempData["Success"] = "Delete task successfully";

                }
            }
            return RedirectToAction("Index");            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            else
            {
                var taskSelected = await _taskService.GetByIdAsync(id.Value);

                if (taskSelected == null)
                {
                    return NotFound();
                }
                return View(taskSelected);
            }
        }

        [HttpPost]
        public async Task<IActionResult>Edit(PRN222_TaskManagement.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                if (task.DueDate.HasValue && task.DueDate.Value < today)
                {
                    _logger.LogInformationWithColor("Edit Tasks : " + task.DueDate.Value + "today: " +today);

                    ModelState.AddModelError("DueDate", "Invalid due date");
                    return View(task);
                }

                var taskOld = await _taskService.GetByIdAsync(task.TaskId);

                taskOld.UpdatedAt = DateTime.Now;
                taskOld.Title = task.Title;
                taskOld.Description = task.Description;
                taskOld.DueDate = task.DueDate;
                taskOld.Priority = task.Priority;

                await _taskService.UpdateAsync(taskOld);
                TempData["Success"] = "Update task successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }


        [HttpPost]
        [Route("Task/UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            _logger.LogInformationWithColor($"Update status {id} : {status}");
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });
            }

            task.Status = status;
            task.CompletedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;

            await _taskService.UpdateAsync(task);                

            return Json(new { message = "Status updated successfully!", status = task.Status });
        }
    }
}
