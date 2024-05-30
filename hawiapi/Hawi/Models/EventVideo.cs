using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class EventVideo
{
    public long EventVideoId { get; set; }

    public long EventId { get; set; }

    public long VideoId { get; set; }

    public byte? VideoTypeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;

    public virtual VideoVideoType? VideoType { get; set; }
}
