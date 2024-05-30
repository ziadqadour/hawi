using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipsPlayGround
{
    public long ChampionshipPlayGroundId { get; set; }

    public long ChampionshipId { get; set; }

    public long PlayGroundId { get; set; }

    public long? UserProfileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Championship Championship { get; set; } = null!;

    public virtual Playground PlayGround { get; set; } = null!;

    public virtual UserProfile? UserProfile { get; set; }
}
