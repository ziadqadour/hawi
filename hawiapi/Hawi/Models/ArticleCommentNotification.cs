using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleCommentNotification
{
    public long ArticleCommentNotificationId { get; set; }

    public long CommentId { get; set; }

    public long UserProfileId { get; set; }

    public byte? CommentNotificationReasonId { get; set; }

    public string? CommentNotificationMemo { get; set; }

    public DateTime CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ArticleComment Comment { get; set; } = null!;

    public virtual ArticleNotificationNotificationReason? CommentNotificationReason { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
