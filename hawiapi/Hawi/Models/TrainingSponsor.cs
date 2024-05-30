using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingSponsor
{
    public long TrainingSponsorId { get; set; }

    public long TrainingId { get; set; }

    public long SponsorId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Sponsor Sponsor { get; set; } = null!;

    public virtual Training Training { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
