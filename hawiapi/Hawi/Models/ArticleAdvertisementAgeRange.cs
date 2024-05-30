using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleAdvertisementAgeRange
{
    public long ArticleAdvertisementAgeRangeId { get; set; }

    public long ArticleId { get; set; }

    public byte AgeRangeId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual AdvertisementAgeRangeAgeRange AgeRange { get; set; } = null!;
}
