using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Training
{
    public long TrainingId { get; set; }

    public long UserProfileId { get; set; }

    public string? TrainingName { get; set; }

    public byte? TrainingTypeId { get; set; }

    public byte SeasonId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsRepeated { get; set; }

    public bool? IsActive { get; set; }

    public long? TrainingImageId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Season Season { get; set; } = null!;

    public virtual ICollection<TrainingAdmin> TrainingAdmins { get; } = new List<TrainingAdmin>();

    public virtual ICollection<TrainingBranch> TrainingBranches { get; } = new List<TrainingBranch>();

    public virtual ICollection<TrainingDetail> TrainingDetails { get; } = new List<TrainingDetail>();

    public virtual Image? TrainingImage { get; set; }

    public virtual ICollection<TrainingInvitationPending> TrainingInvitationPendings { get; } = new List<TrainingInvitationPending>();

    public virtual ICollection<TrainingSponsor> TrainingSponsors { get; } = new List<TrainingSponsor>();

    public virtual TrainingTrainingType? TrainingType { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
