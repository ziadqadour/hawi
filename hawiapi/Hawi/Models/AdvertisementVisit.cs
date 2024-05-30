using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class AdvertisementVisit
{
    public long AdvertisementVisitId { get; set; }

    public long AdvertisementId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Advertisement Advertisement { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
