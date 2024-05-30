using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingPlayerXxx
{
    public long TrainingPlayerId { get; set; }

    public long TrainingDetailsId { get; set; }

    public long UserProfileId { get; set; }

    public long PlayerUserProfileId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual UserProfile PlayerUserProfile { get; set; } = null!;

    public virtual TrainingDetail TrainingDetails { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
