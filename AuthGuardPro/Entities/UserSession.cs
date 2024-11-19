using System;
using System.Collections.Generic;

namespace AuthGuardPro.Entities;

public partial class UserSession
{
    public Guid SessionId { get; set; }

    public Guid? UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? User { get; set; }
}
