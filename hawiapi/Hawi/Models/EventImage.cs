using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class EventImage
{
    public long EventImageId { get; set; }

    public long EventId { get; set; }

    public long ImageId { get; set; }

    public byte? ImageTypeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;

    public virtual ImageImageType? ImageType { get; set; }
}
