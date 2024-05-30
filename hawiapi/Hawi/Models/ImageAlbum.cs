using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ImageAlbum
{
    public long ImageAlbumsId { get; set; }

    public long? UserProfileId { get; set; }

    public string? AlbumName { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ImageAlbumImage> ImageAlbumImages { get; } = new List<ImageAlbumImage>();

    public virtual UserProfile? UserProfile { get; set; }
}
