using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Achievement
{
    public long AchievementId { get; set; }

    public long? AchievementUserProfileId { get; set; }

    public DateTime? AchievementDate { get; set; }

    public string? AchievementTitle { get; set; }

    public byte SeasonId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Season Season { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
