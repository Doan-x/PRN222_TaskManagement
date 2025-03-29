using System;
using System.Collections.Generic;

namespace PRN222_TaskManagement.Models;

public partial class CalendarSync
{
    public int SyncId { get; set; }

    public int UserId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime? LastSynced { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
