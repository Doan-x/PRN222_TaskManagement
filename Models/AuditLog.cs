using System;
using System.Collections.Generic;

namespace PRN222_TaskManagement.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public int? EventId { get; set; }

    public string Action { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? ActionTime { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? User { get; set; }
}
