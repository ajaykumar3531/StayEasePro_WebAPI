using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class State
{
    public Guid StateId { get; set; }

    public string StateName { get; set; } = null!;

    public Guid CountryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}
