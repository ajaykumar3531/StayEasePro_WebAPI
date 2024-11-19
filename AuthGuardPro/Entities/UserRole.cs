using System;
using System.Collections.Generic;

namespace AuthGuardPro.Entities;

public partial class UserRole
{
    public Guid UserRoleId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? RoleId { get; set; }

    public DateTime? DateAssigned { get; set; }

    public DateTime? DateUpdated { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
