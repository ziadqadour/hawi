using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Coach
{
    public long CoachId { get; set; }

    public long UserProfileId { get; set; }

    public byte SeasonId { get; set; }

    public byte? CoachTypeId { get; set; }

    public short? NumberOfMatches { get; set; }

    public short? Win { get; set; }

    public short? Lose { get; set; }

    public short? Draw { get; set; }

    public short? GoalScored { get; set; }

    public short? GoalConceded { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual CoachCoachType? CoachType { get; set; }

    public virtual Season Season { get; set; } = null!;
}
