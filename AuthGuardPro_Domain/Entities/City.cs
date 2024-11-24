using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class City
{
    public Guid CityId { get; set; }

    public string CityName { get; set; } = null!;

    public Guid StateId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual State State { get; set; } = null!;
}
