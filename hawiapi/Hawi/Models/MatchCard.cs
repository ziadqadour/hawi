using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchCard
{
    public long MatchCardId { get; set; }

    public byte MatchCardTypeId { get; set; }

    public long PlayerUserProfileId { get; set; }

    public long MatchId { get; set; }

    public string? Minte { get; set; }

    public string? Reason { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual MatchCardMatchCardType MatchCardType { get; set; } = null!;

    public virtual UserProfile PlayerUserProfile { get; set; } = null!;
}
