using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class Room
{
    public Guid RoomId { get; set; }

    public Guid PropertyId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int MaxOccupancy { get; set; }

    public decimal RentPerMonth { get; set; }

    public bool OccupiedStatus { get; set; }

    public bool DeleteStatus { get; set; }

    public short? FloorNumber { get; set; }

    public string? BlockName { get; set; }

    public virtual Property Property { get; set; } = null!;

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
