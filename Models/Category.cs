using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PRN222_TaskManagement.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public int? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public  ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual User? User { get; set; }
}
