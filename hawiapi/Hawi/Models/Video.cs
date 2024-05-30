using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Video
{
    public long VideoId { get; set; }

    public string VideoUrl { get; set; } = null!;

    public string? VideoFileName { get; set; }

    public string? VideoShortDescription { get; set; }

    public byte? VideoTypeId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<ArticleVideo> ArticleVideos { get; } = new List<ArticleVideo>();

    public virtual ICollection<EventVideo> EventVideos { get; } = new List<EventVideo>();

    public virtual VideoVideoType? VideoType { get; set; }
}
