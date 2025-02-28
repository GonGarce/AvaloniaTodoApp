using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace AvaloniaTodoApp.Models;

[Table("profiles")]
public class SProfile : BaseModel
{
#nullable disable
    [PrimaryKey("id", true)]
    public string Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("username")]
    
    public string Username { get; set; }


    [Column("image")]
    
    public string Image { get; set; }

#nullable enable
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}