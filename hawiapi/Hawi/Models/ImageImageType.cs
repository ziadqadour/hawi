using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ImageImageType
{
    public byte ImageTypeId { get; set; }

    public string? ImageType { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; } = new List<ArticleImage>();

    public virtual ICollection<EventImage> EventImages { get; } = new List<EventImage>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual ICollection<UserProfileImage> UserProfileImages { get; } = new List<UserProfileImage>();
}
