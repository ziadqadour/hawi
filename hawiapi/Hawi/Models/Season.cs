using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Season
{
    public byte SeasonId { get; set; }

    public string? SeasonName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Achievement> Achievements { get; } = new List<Achievement>();

    public virtual ICollection<Coach> Coaches { get; } = new List<Coach>();

    public virtual ICollection<Match> Matches { get; } = new List<Match>();

    public virtual ICollection<Player> Players { get; } = new List<Player>();

    public virtual ICollection<Referee> Referees { get; } = new List<Referee>();

    public virtual ICollection<SportInstitutionBelongPending> SportInstitutionBelongPendings { get; } = new List<SportInstitutionBelongPending>();

    public virtual ICollection<SportInstitutionBelong> SportInstitutionBelongs { get; } = new List<SportInstitutionBelong>();

    public virtual ICollection<Training> Training { get; } = new List<Training>();
}
