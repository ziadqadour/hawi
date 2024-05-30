using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchMatchRefereeCandidate
{
    public long MatchRefereeCandidateId { get; set; }

    public long? UserProfileId { get; set; }

    public long? MatchRefereeRequestsId { get; set; }

    public byte? ExpectedCost { get; set; }

    public string? Comment { get; set; }

    public long? MatchId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Match? Match { get; set; }

    public virtual MatchMatchRefereeRequest? MatchRefereeRequests { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
