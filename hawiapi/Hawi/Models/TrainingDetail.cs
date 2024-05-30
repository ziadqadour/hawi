using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingDetail
{
    public long TrainingDetailsId { get; set; }

    public long TrainingId { get; set; }

    public string? TrainingCost { get; set; }

    public string? TrainingPlaceLocation { get; set; }

    public byte? PlaygroundSizeId { get; set; }

    public byte? PlaygroundFloorId { get; set; }

    public DateTime? TrainingDate { get; set; }

    public double? TrainingTime { get; set; }

    public double? DurationInMinutes { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual TrainingDetailsPlaygroundFloor? PlaygroundFloor { get; set; }

    public virtual TrainingDetailsPlaygroundSize? PlaygroundSize { get; set; }

    public virtual Training Training { get; set; } = null!;

    public virtual ICollection<TrainingAttendance> TrainingAttendances { get; } = new List<TrainingAttendance>();

    public virtual ICollection<TrainingInjury> TrainingInjuries { get; } = new List<TrainingInjury>();

    public virtual ICollection<TrainingPlayerXxx> TrainingPlayerXxxes { get; } = new List<TrainingPlayerXxx>();

    public virtual ICollection<TrainingTeam> TrainingTeams { get; } = new List<TrainingTeam>();
}
