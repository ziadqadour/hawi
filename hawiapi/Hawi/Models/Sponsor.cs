using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Sponsor
{
    public long SponsorId { get; set; }

    public long UserProfileId { get; set; }

    public string? SponsorName { get; set; }

    public long? SponsorLogoId { get; set; }

    public byte? SponsorTypeId { get; set; }

    public long? SponsorUserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Image? SponsorLogo { get; set; }

    public virtual SponsorSponsorType? SponsorType { get; set; }

    public virtual UserProfile? SponsorUserProfile { get; set; }

    public virtual ICollection<TrainingSponsor> TrainingSponsors { get; } = new List<TrainingSponsor>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
