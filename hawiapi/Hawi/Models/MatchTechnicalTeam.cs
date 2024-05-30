using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchTechnicalTeam
{
    public long MatchTechnicalTeamId { get; set; }

    public long UserProfileId { get; set; }

    public byte MatchTechnicalTeamTypeId { get; set; }

    public long MatchId { get; set; }

    public DateTime CreateDate { get; set; }

    public long? TeamId { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual MatchTechnicalTeamType MatchTechnicalTeamType { get; set; } = null!;

    public virtual SportInstitution? Team { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
