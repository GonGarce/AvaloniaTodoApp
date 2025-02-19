using System;

namespace AvaloniaTodoAPp.Models;

public class TodoTask(int id, string description, DateTime? dueDate, DateTime? creationDate, bool done = false, bool priority = false)
{
    public int Id { get; set; } = id;
    public string Description { get; set; } = description;
    public DateTime? DueDate { get; set; } = dueDate;
    public DateTime? CreationDate { get; set; } = creationDate;
    public bool Done { get; set; } = done;
    public bool Priority { get; set; } = priority;
}