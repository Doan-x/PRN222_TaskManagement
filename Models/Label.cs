using System;
using System.Collections.Generic;

namespace PRN222_TaskManagement.Models;

public partial class Label
{
    public int LabelId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
