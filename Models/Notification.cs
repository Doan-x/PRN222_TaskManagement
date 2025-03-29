using System;
using System.Collections.Generic;

namespace PRN222_TaskManagement.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public int EventId { get; set; }

    public DateTime NotifyTime { get; set; }

    public string? NotifyMethod { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
