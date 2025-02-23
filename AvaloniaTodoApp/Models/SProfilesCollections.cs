using System.Collections.Generic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace AvaloniaTodoAPp.Models;
[Table("profiles_collections")]
public class SProfilesCollections : BaseModel
{    
    [Reference(typeof(SProfile))]
    public SProfile User { get; set; }
}