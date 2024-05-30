using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchPlayer
{
    public long MatchPlayerId { get; set; }

    public long? TeamId { get; set; }

    public long? PlayerId { get; set; }

    public byte? PlaceId { get; set; }

    public byte? PlayerNumber { get; set; }

    public bool? IsStart { get; set; }

    public long UserProfileId { get; set; }

    public DateTime CreateDate { get; set; }

    public long? MatchId { get; set; }

    public bool? IsPrepare { get; set; }

    public long? PlayerUserProfileId { get; set; }

    public bool? IsRefereeSubstituted { get; set; }

    public bool? IsCoachSubstituted { get; set; }

    public virtual Match? Match { get; set; }

    public virtual PlayerPlayerPlace? Place { get; set; }

    public virtual Player? Player { get; set; }

    public virtual UserProfile? PlayerUserProfile { get; set; }

    public virtual SportInstitution? Team { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
