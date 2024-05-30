using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingAttendance
{
    public long AttendanceId { get; set; }

    public long UserProfileId { get; set; }

    public long TrainingDetailsId { get; set; }

    public long AttendanceUserProfileId { get; set; }

    public bool IsPresent { get; set; }

    public byte? AttendanceTypeId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserProfile AttendanceUserProfile { get; set; } = null!;

    public virtual TrainingDetail TrainingDetails { get; set; } = null!;

    public virtual ICollection<TrainingEvaluation> TrainingEvaluations { get; } = new List<TrainingEvaluation>();

    public virtual ICollection<TrainingInjury> TrainingInjuries { get; } = new List<TrainingInjury>();

    public virtual ICollection<TrainingTeamPlayer> TrainingTeamPlayers { get; } = new List<TrainingTeamPlayer>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
