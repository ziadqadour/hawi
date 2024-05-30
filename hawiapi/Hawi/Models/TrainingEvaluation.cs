using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingEvaluation
{
    public long EvaluationId { get; set; }

    public long UserProfileId { get; set; }

    public long AttendanceId { get; set; }

    public double? OutOfFive { get; set; }

    public string? EvaluationComment { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual TrainingAttendance Attendance { get; set; } = null!;

    public virtual UserProfile UserProfile { get; set; } = null!;
}
