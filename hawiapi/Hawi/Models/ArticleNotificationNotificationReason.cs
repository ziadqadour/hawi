using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleNotificationNotificationReason
{
    public byte NotificationReasonId { get; set; }

    public string? NotificationReason { get; set; }

    public virtual ICollection<ArticleCommentNotification> ArticleCommentNotifications { get; } = new List<ArticleCommentNotification>();
}
