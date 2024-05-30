using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UserProfileImage
{
    public long UserProfileImageId { get; set; }

    public long UserProfileId { get; set; }

    public long ImageId { get; set; }

    public byte? ImageTypeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Image Image { get; set; } = null!;

    public virtual ImageImageType? ImageType { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
