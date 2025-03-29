using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN222_TaskManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Email is invalid")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<CalendarSync> CalendarSyncs { get; set; } = new List<CalendarSync>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<EventShare> EventShares { get; set; } = new List<EventShare>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
