using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingDetailsPlaygroundFloor
{
    public byte PlaygroundFloorId { get; set; }

    public string? PlaygroundFloor { get; set; }

    public virtual ICollection<Playground> Playgrounds { get; } = new List<Playground>();

    public virtual ICollection<TrainingDetail> TrainingDetails { get; } = new List<TrainingDetail>();
}
