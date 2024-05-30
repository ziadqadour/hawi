using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class TrainingTeamPlayer
{
    public long TrainingTeamPlayerId { get; set; }

    public long AttendanceId { get; set; }

    public long TrainingTeamId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual TrainingAttendance Attendance { get; set; } = null!;

    public virtual TrainingTeam TrainingTeam { get; set; } = null!;
}
