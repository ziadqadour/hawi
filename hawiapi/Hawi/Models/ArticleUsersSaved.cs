using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleUsersSaved
{
    public long ArticleUsersSavedId { get; set; }

    public long ArticleId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
