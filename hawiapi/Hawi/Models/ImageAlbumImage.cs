using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ImageAlbumImage
{
    public long ImageAlbumImagesId { get; set; }

    public long AlbumId { get; set; }

    public long ImageId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ImageAlbum Album { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;
}
