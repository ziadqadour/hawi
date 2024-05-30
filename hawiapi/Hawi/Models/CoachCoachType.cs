using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class CoachCoachType
{
    public byte CoachTypeId { get; set; }

    public string? CoachType { get; set; }

    public virtual ICollection<Coach> Coaches { get; } = new List<Coach>();
}
