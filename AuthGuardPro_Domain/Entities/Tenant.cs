using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class Tenant
{
    public Guid TenantId { get; set; }

    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime? CheckOutDate { get; set; }

    public DateTime RentDueDate { get; set; }

    public bool ActiveStatus { get; set; }

    public bool DeleteStatus { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
