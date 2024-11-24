using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class Address
{
    public Guid AddressId { get; set; }

    public string? Street { get; set; }

    public string? ZipCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteStatus { get; set; }

    public Guid? StateId { get; set; }

    public Guid? CountryId { get; set; }

    public Guid? CityId { get; set; }

    public string? Landmark { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
