using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN222_TaskManagement.Models;

public partial class Event
{

    public int EventId { get; set; }

    [BindNever]
    public int UserId { get; set; }

    public int? CategoryId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    [Required(ErrorMessage = "Start time is required")]
    public DateTime StartTime { get; set; }
    [Required(ErrorMessage = "End time is required")]
    public DateTime EndTime { get; set; }

    public string? Location { get; set; }

    public string? Priority { get; set; }

    public string? Color { get; set; } = "#007bff";

    public bool EmailReminderSent { get; set; } = false;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<EventShare> EventShares { get; set; } = new List<EventShare>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ValidateNever]
    public virtual User User { get; set; } = null!;
}
