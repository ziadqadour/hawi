using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchScoreGoalMethod
{
    public byte GoalMethodId { get; set; }

    public string? GoalMethod { get; set; }

    public virtual ICollection<MatchScore> MatchScores { get; } = new List<MatchScore>();
}
