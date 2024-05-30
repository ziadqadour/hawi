using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleNotification
{
    public long ArticleNotificationId { get; set; }

    public long ArticleId { get; set; }

    public long UserProfileId { get; set; }

    public byte? NotificationReasonId { get; set; }

    public string? NotificationMemo { get; set; }

    public DateTime CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
