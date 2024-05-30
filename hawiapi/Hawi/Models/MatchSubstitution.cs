using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchSubstitution
{
    public long MatchSubstitutionsId { get; set; }

    public long MatchId { get; set; }

    public long PlayerInUserProfileId { get; set; }

    public long PlayerOutUserProfileId { get; set; }

    public string? Minite { get; set; }

    public string? Reason { get; set; }

    public DateTime CreateDate { get; set; }

    public bool? ConfirmedByTheReferee { get; set; }

    public long? TeamId { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual UserProfile PlayerInUserProfile { get; set; } = null!;

    public virtual UserProfile PlayerOutUserProfile { get; set; } = null!;

    public virtual SportInstitution? Team { get; set; }
}
