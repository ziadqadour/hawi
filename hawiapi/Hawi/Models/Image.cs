using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Image
{
    public long ImageId { get; set; }

    public byte? ImageTypeId { get; set; }

    public string? ImageFileName { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageUrlfullPath { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ArticleImage> ArticleImages { get; } = new List<ArticleImage>();

    public virtual ICollection<EventImage> EventImages { get; } = new List<EventImage>();

    public virtual ICollection<ImageAlbumImage> ImageAlbumImages { get; } = new List<ImageAlbumImage>();

    public virtual ImageImageType? ImageType { get; set; }

    public virtual ICollection<Sponsor> Sponsors { get; } = new List<Sponsor>();

    public virtual ICollection<Training> Training { get; } = new List<Training>();

    public virtual ICollection<Uniform> Uniforms { get; } = new List<Uniform>();

    public virtual ICollection<UserProfileImage> UserProfileImages { get; } = new List<UserProfileImage>();
}
