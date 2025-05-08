using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PRN222_TaskManagement.Mail;
using PRN222_TaskManagement.Models;
using TrongND;

namespace PRN222_TaskManagement.Utils
{
    public class EventReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventReminderService> _logger;

        public EventReminderService(IServiceProvider serviceProvider, ILogger<EventReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformationWithColor("Background Service đã bắt đầu chạy.");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformationWithColor("Bắt đầu một vòng lặp kiểm tra sự kiện...");
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<Prn222TaskManagementContext>();
                        var emailService = scope.ServiceProvider.GetRequiredService<MailHelper>();

                        var now = DateTime.Now;
                        _logger.LogInformationWithColor($"Thời gian hiện tại : {now}");

                        var upcomingEvents = dbContext.Events.Include(e => e.User)
                            .Where(e => e.StartTime >= now && e.StartTime <= now.AddHours(1))
                            .ToList();
                        var upcomingTasks = dbContext.Tasks.Include(e => e.User)
                            .Where(e => !e.IsRepeated && e.DueDate != null && e.DueDate.Value == DateOnly.FromDateTime(now.AddDays(1)))
                            .ToList();
                        var taskDaily = dbContext.Tasks.Include(e => e.User)
                            .Where(e => e.DueDate != null && e.DueDate.Value == DateOnly.FromDateTime(now) && e.IsRepeated)
                            .ToList();


                        foreach (var ev in upcomingEvents)
                        {
                            _logger.LogInformationWithColor($"Sự kiện {ev.EventId}: {ev.Title} lúc {ev.StartTime} UTC");

                            if (!ev.EmailReminderSent)
                            {
                                try
                                {
                                    await emailService.SendGmail(ev.User.Email, "Sự kiện sắp diễn ra!",
                                        $"Sự kiện '{ev.Title}' sẽ bắt đầu lúc {ev.StartTime}.");

                                    _logger.LogInformationWithColor($"Đã gửi email nhắc nhở cho {ev.User.Email}.");
                                    ev.EmailReminderSent = true; // Đánh dấu đã gửi email
                                    dbContext.Events.Update(ev);
                                }
                                catch (Exception emailEx)
                                {
                                    _logger.LogError(emailEx, $"Lỗi khi gửi email cho {ev.User.Email}.");
                                }
                            }
                            else
                            {
                                _logger.LogInformationWithColor($"Đã gửi email cho sự kiện {ev.EventId} trước đó, bỏ qua.");
                            }

                            await dbContext.SaveChangesAsync();
                        }

                        foreach (var ev in upcomingTasks)
                        {
                            _logger.LogInformationWithColor($"Task {ev.TaskId}: {ev.Title} due date {ev.DueDate} UTC");
                            if (!ev.EmailReminderSent)
                            {
                                try
                                {

                                    await emailService.SendGmail(ev.User.Email, "Reminder of today's work",
                                    $"Task '{ev.Title}' sắp hết hạn vào {ev.DueDate}.");
                                    _logger.LogInformationWithColor($"Đã gửi email nhắc nhở TASK cho {ev.User.Email}.");
                                    ev.EmailReminderSent = true;
                                    dbContext.Tasks.Update(ev);
                                }
                                catch (Exception emailEx)
                                {
                                    _logger.LogError(emailEx, $"Lỗi khi gửi email cho {ev.User.Email}.");
                                }
                            }
                            else
                            {
                                _logger.LogInformationWithColor($"Đã gửi email cho TASKS {ev.TaskId} trước đó, bỏ qua.");
                            }
                            await dbContext.SaveChangesAsync();
                        }

                        if (taskDaily != null && taskDaily.Count > 0)
                        {
                            foreach (var ev in taskDaily)
                            {
                                _logger.LogInformationWithColor($"Daily task {ev.TaskId}: {ev.Title}: sent: {ev.EmailReminderSent}");
                                if (!ev.EmailReminderSent)
                                {
                                    try
                                    {
                                        if (ev.IsRepeated && ev.TimeReminder.HasValue && ev.TimeReminder.Value == TimeOnly.FromDateTime(now))
                                        {
                                            await emailService.SendGmail(ev.User.Email, "Reminder of today's work",
                                                        $"Task today of you : '{ev.Title}' ");


                                            _logger.LogInformationWithColor($"Đã gửi email nhắc nhở TASK DAILY cho {ev.User.Email}.");
                                            ev.EmailReminderSent = true;
                                            dbContext.Tasks.Update(ev);
                                        }

                                    }
                                    catch (Exception emailEx)
                                    {
                                        _logger.LogError(emailEx, $"Lỗi khi gửi email cho {ev.User.Email}.");
                                    }
                                }
                                else
                                {
                                    _logger.LogInformationWithColor($"Đã gửi email cho TASKS DAILY {ev.TaskId} trước đó, bỏ qua.");
                                }
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gửi email nhắc nhở sự kiện");
                }

                // Delay 5 phút trước khi kiểm tra lại
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

        }
    }
}
