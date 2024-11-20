using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class Address
{
    public Guid AddressId { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteStatus { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
