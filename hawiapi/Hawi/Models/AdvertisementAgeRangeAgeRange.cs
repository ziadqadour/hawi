using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class AdvertisementAgeRangeAgeRange
{
    public byte AgeRangeId { get; set; }

    public string? AgeRange { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<AdvertisementAgeRange> AdvertisementAgeRanges { get; } = new List<AdvertisementAgeRange>();

    public virtual ICollection<ArticleAdvertisementAgeRange> ArticleAdvertisementAgeRanges { get; } = new List<ArticleAdvertisementAgeRange>();
}
