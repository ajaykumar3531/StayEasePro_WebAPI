using System;
using System.Collections.Generic;

namespace StayEasePro_Core.Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public short Role { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ExpectedJoinDate { get; set; }

    public DateTime? JoinedDate { get; set; }

    public bool DeleteStatus { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public short Gender { get; set; }

    public Guid? AddressId { get; set; }

    public Guid? PropertyId { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
