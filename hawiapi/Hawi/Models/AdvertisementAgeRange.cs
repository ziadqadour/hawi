using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class AdvertisementAgeRange
{
    public long AdvertisementAgeRangeId { get; set; }

    public long AdvertisementId { get; set; }

    public byte? AgeRangeId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Advertisement Advertisement { get; set; } = null!;

    public virtual AdvertisementAgeRangeAgeRange? AgeRange { get; set; }
}
