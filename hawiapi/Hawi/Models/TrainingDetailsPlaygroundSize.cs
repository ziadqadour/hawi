using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingDetailsPlaygroundSize
{
    public byte PlaygroundSizeId { get; set; }

    public string? PlaygroundSize { get; set; }

    public virtual ICollection<Playground> Playgrounds { get; } = new List<Playground>();

    public virtual ICollection<TrainingDetail> TrainingDetails { get; } = new List<TrainingDetail>();
}
