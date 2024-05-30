using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class MatchFriendlyMatchAgeCategory
{
    public long FriendlyMatchAgeCategoryId { get; set; }

    public byte AgeCategoryId { get; set; }

    public long? UserprofileId { get; set; }

    public long? FriendlyMatchId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual SportInstitutionAgePriceAgeCategory AgeCategory { get; set; } = null!;

    public virtual MatchFriendlyMatch? FriendlyMatch { get; set; }

    public virtual UserProfile? Userprofile { get; set; }
}
