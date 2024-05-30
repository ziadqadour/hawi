using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleComment
{
    public long CommentId { get; set; }

    public long ArticleId { get; set; }

    public long CommentUserProfileId { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual ICollection<ArticleCommentLike> ArticleCommentLikes { get; } = new List<ArticleCommentLike>();

    public virtual ICollection<ArticleCommentNotification> ArticleCommentNotifications { get; } = new List<ArticleCommentNotification>();

    public virtual UserProfile CommentUserProfile { get; set; } = null!;
}
