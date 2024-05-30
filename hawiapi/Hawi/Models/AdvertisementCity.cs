using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class AdvertisementCity
{
    public long AdvertisementCityId { get; set; }

    public long AdvertisementId { get; set; }

    public long CityId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Advertisement Advertisement { get; set; } = null!;

    public virtual City City { get; set; } = null!;
}
