using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchTechnicalTeamType
{
    public byte MatchTechnicalTeamTypeId { get; set; }

    public string? TechnicalTeamType { get; set; }

    public virtual ICollection<MatchTechnicalTeam> MatchTechnicalTeams { get; } = new List<MatchTechnicalTeam>();
}
