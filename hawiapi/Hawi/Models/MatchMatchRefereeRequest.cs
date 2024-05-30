using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchMatchRefereeRequest
{
    public long MatchRefereeRequestId { get; set; }

    public long UserProfileId { get; set; }

    public DateTime? MatchDate { get; set; }

    public string? Place { get; set; }

    public byte? EstimatedCost { get; set; }

    public byte? NumberOfReferee { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public long? MatchId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Match? Match { get; set; }

    public virtual ICollection<MatchMatchRefereeCandidate> MatchMatchRefereeCandidates { get; } = new List<MatchMatchRefereeCandidate>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
