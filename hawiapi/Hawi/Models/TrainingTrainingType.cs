using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingTrainingType
{
    public byte TrainingTypeId { get; set; }

    public string? TrainingType { get; set; }

    public virtual ICollection<Training> Training { get; } = new List<Training>();
}
