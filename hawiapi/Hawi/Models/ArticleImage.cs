using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleImage
{
    public long ArticleImageId { get; set; }

    public long ArticleId { get; set; }

    public long ImageId { get; set; }

    public byte? ImageTypeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;

    public virtual ImageImageType? ImageType { get; set; }
}
