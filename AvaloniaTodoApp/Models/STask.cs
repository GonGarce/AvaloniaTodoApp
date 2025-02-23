using System;
using System.Diagnostics.CodeAnalysis;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace AvaloniaTodoAPp.Models;

[Table("tasks")]
public class STask : BaseModel
{
#nullable disable
    [PrimaryKey("id", true)]
    public Guid Id { get; set; }

    [Column("collection_id")]
    public int CollectionId { get; set; }
    
    [Reference(typeof(SCollection))]
    public SCollection Collection { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("description")]
    
    public string Description { get; set; }

    [Column("due_date")]
    public DateTime? DueDate { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }
    
    [Column("critical")]
    public bool Critical { get; set; }
#nullable enable

    public override bool Equals(object? obj)
    {
        return obj is STask message && Id == message.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public bool IsCompleted()
    {
        return CompletedAt != null;
    }
}