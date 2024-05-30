using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchApplicantsSportInstituationForFriendlyMatch
{
    public long ApplicantsSportInstituationId { get; set; }

    public long SportInstituationId { get; set; }

    public long FriendlyMatchId { get; set; }

    public bool? IsSelectedToPlayMatch { get; set; }

    public DateTime? CreateDateTime { get; set; }

    public virtual MatchFriendlyMatch FriendlyMatch { get; set; } = null!;

    public virtual SportInstitution SportInstituation { get; set; } = null!;
}
