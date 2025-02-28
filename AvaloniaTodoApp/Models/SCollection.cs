using System;
using System.Collections.Generic;
using AvaloniaTodoApp.Global;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace AvaloniaTodoApp.Models;

[Table("collections")]
public class SCollection : BaseModel
{
#nullable disable
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("order")]
    public int Order { get; set; }

    [Column("owner_id")]
    public string OwnerId { get; set; }

    [Reference(typeof(SProfile), foreignKey: "collections_owner_id_fkey")]
    public SProfile Owner { get; set; }

    [Reference(typeof(SProfilesCollections), columnName: "collection_id")]
    public List<SProfilesCollections> Users { get; set; }
#nullable enable

    public override bool Equals(object? obj)
    {
        return obj is SCollection message && Id == message.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public bool IsOwner()
    {
        return OwnerId == AppState.Instance.UserId;
    }
}