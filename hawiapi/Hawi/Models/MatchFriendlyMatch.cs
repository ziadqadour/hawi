using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchFriendlyMatch
{
    public long FriendlyMatchId { get; set; }

    public long SportInstituationId { get; set; }

    public bool IsActive { get; set; }

    public DateTime MatchDate { get; set; }

    public string MatchTime { get; set; } = null!;

    public byte? MatchDuration { get; set; }

    public long? PlayGroundId { get; set; }

    public bool? IsRefereeRequest { get; set; }

    public byte? NumberOfReferee { get; set; }

    public double? Price { get; set; }

    public DateTime? CreateDateTime { get; set; }

    public long? UserProfileId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<MatchApplicantsSportInstituationForFriendlyMatch> MatchApplicantsSportInstituationForFriendlyMatches { get; } = new List<MatchApplicantsSportInstituationForFriendlyMatch>();

    public virtual ICollection<MatchFriendlyMatchAgeCategory> MatchFriendlyMatchAgeCategories { get; } = new List<MatchFriendlyMatchAgeCategory>();

    public virtual Playground? PlayGround { get; set; }

    public virtual SportInstitution SportInstituation { get; set; } = null!;

    public virtual UserProfile? UserProfile { get; set; }
}
