using System;
using System.Collections.Generic;

namespace AuthGuardPro.Entities;

public partial class EmailLog
{
    public Guid LogId { get; set; }

    public Guid UserId { get; set; }

    public string FromEmail { get; set; } = null!;

    public string ToEmail { get; set; } = null!;

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime? SentDate { get; set; }

    public string? Status { get; set; }
}
