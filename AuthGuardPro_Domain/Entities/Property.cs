using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class Property
{
    public Guid PropertyId { get; set; }

    public Guid OwnerId { get; set; }

    public string? PropertyName { get; set; }

    public long TotalRooms { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteStatus { get; set; }

    public Guid AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
