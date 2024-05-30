using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class VideoVideoType
{
    public byte VideoTypeId { get; set; }

    public string? VideoType { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ArticleVideo> ArticleVideos { get; } = new List<ArticleVideo>();

    public virtual ICollection<EventVideo> EventVideos { get; } = new List<EventVideo>();

    public virtual ICollection<Video> Videos { get; } = new List<Video>();
}
