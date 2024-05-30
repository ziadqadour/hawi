using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingInjuryInjuryPosition
{
    public byte InjuryPositionId { get; set; }

    public string? InjuryPositionInEnglish { get; set; }

    public string? InjuryPositionInArabic { get; set; }

    public virtual ICollection<TrainingInjury> TrainingInjuries { get; } = new List<TrainingInjury>();
}
