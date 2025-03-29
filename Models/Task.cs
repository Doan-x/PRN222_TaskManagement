using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN222_TaskManagement.Models;

public partial class Task
{
    public int TaskId { get; set; }

    [ValidateNever]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Title is not Empty")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Due Date is not Empty")]
    public DateOnly? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public bool? IsRepeated { get; set; } = false;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ValidateNever]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();
}
