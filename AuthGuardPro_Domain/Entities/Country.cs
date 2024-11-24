using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class Country
{
    public Guid CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public string? CountryCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
