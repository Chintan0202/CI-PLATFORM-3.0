using System;
using System.Collections.Generic;

namespace CIPlatformMain.Entities.Models;

public partial class Country
{
    public long CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Iso { get; set; }

    public byte[] CreatedAt { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();
}
