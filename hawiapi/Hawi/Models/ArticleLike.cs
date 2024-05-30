using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleLike
{
    public long LikeId { get; set; }

    public long LikeUserProfileId { get; set; }

    public long ArticleId { get; set; }

    public DateTime? LikeCreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual UserProfile LikeUserProfile { get; set; } = null!;
}
