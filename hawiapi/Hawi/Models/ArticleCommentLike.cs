using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleCommentLike
{
    public long ArticleCommentLikeId { get; set; }

    public long UserProfileId { get; set; }

    public long CommentId { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ArticleComment Comment { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
