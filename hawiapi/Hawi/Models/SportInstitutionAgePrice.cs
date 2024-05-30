using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionAgePrice
{
    public int SportInstitutionAgePriceId { get; set; }

    public long SportInstitutionBranchId { get; set; }

    public byte AgeCategoryId { get; set; }

    public byte? SubscriptionPeriodId { get; set; }

    public byte? SportInstitutionTypeId { get; set; }

    public double? Price { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SportInstitutionAgePriceAgeCategory AgeCategory { get; set; } = null!;

    public virtual SportInstitutionBranch SportInstitutionBranch { get; set; } = null!;

    public virtual SportInstitutionSportInstitutionType? SportInstitutionType { get; set; }

    public virtual SportInstitutionAgePriceSubscriptionPeriod? SubscriptionPeriod { get; set; }
}
