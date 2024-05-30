using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionAgePriceAgeCategory
{
    public byte AgeCategoryId { get; set; }

    public string? AgeCategory { get; set; }

    public byte? SportInstitutionTypeId { get; set; }

    public byte? Age { get; set; }

    public virtual ICollection<ChampionshipAgeCategory> ChampionshipAgeCategories { get; } = new List<ChampionshipAgeCategory>();

    public virtual ICollection<MatchFriendlyMatchAgeCategory> MatchFriendlyMatchAgeCategories { get; } = new List<MatchFriendlyMatchAgeCategory>();

    public virtual ICollection<SportInstitutionAgeCategoryBelong> SportInstitutionAgeCategoryBelongs { get; } = new List<SportInstitutionAgeCategoryBelong>();

    public virtual ICollection<SportInstitutionAgePrice> SportInstitutionAgePrices { get; } = new List<SportInstitutionAgePrice>();

    public virtual ICollection<SportInstitutionBelongPendingAgeCategory> SportInstitutionBelongPendingAgeCategories { get; } = new List<SportInstitutionBelongPendingAgeCategory>();

    public virtual SportInstitutionSportInstitutionType? SportInstitutionType { get; set; }
}
