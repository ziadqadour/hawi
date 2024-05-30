using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Advertisement
{
    public long AdvertisementId { get; set; }

    public long UserProfileId { get; set; }

    public string? AdvertisementTitle { get; set; }

    public string? AdvertisementText { get; set; }

    public long TargetUserProfileId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? AdvertisementFileName { get; set; }

    public string? AdvertisementUrlfullPath { get; set; }

    public string? TargetUserLogoFileName { get; set; }

    public string? TargetUserLogoUrlfullPath { get; set; }

    public string? TargetSite { get; set; }

    public bool? IsMale { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<AdvertisementAgeRange> AdvertisementAgeRanges { get; } = new List<AdvertisementAgeRange>();

    public virtual ICollection<AdvertisementCity> AdvertisementCities { get; } = new List<AdvertisementCity>();

    public virtual ICollection<AdvertisementSeen> AdvertisementSeens { get; } = new List<AdvertisementSeen>();

    public virtual ICollection<AdvertisementVisit> AdvertisementVisits { get; } = new List<AdvertisementVisit>();

    public virtual UserProfile TargetUserProfile { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
