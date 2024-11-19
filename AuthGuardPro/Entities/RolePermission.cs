using System;
using System.Collections.Generic;

namespace AuthGuardPro.Entities;

public partial class RolePermission
{
    public Guid RolePermissionId { get; set; }

    public Guid? RoleId { get; set; }

    public Guid? PermissionId { get; set; }

    public DateTime? DateAssigned { get; set; }

    public DateTime? DateUpdated { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Role? Role { get; set; }
}
