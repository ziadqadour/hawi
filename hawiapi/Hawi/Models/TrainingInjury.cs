using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingInjury
{
    public long InjuryId { get; set; }

    public long UserProfileId { get; set; }

    public long AttendanceId { get; set; }

    public byte? InjuryPositionId { get; set; }

    public string? InjuryComment { get; set; }

    public long? TrainingDetailsId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual TrainingAttendance Attendance { get; set; } = null!;

    public virtual TrainingInjuryInjuryPosition? InjuryPosition { get; set; }

    public virtual TrainingDetail? TrainingDetails { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
