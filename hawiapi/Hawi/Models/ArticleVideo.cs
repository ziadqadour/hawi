using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleVideo
{
    public long ArticleVideoId { get; set; }

    public long ArticleId { get; set; }

    public long VideoId { get; set; }

    public byte? VideoTypeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;

    public virtual VideoVideoType? VideoType { get; set; }
}
