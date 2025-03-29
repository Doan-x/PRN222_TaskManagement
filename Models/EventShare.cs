using System;
using System.Collections.Generic;

namespace PRN222_TaskManagement.Models;

public partial class EventShare
{
    public int ShareId { get; set; }

    public int EventId { get; set; }

    public string? SharedWithEmail { get; set; }

    public int? SharedWithUserId { get; set; }

    public string? Permission { get; set; }

    public DateTime? SharedAt { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User? SharedWithUser { get; set; }
}
