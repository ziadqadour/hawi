using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipReferee
{
    public long ChampionshipRefereeId { get; set; }

    public long UserProfileId { get; set; }

    public long? ChampionshipId { get; set; }

    public long? MatchRefereeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Championship? Championship { get; set; }

    public virtual MatchReferee? MatchReferee { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
