using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionBelong
{
    public long SportInstitutionBelongId { get; set; }

    public long SportInstitutionBranchId { get; set; }

    public long BelongUserProfileId { get; set; }

    public string? ShortDescription { get; set; }

    public byte? BelongTypeId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public byte? SeasonId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual UserProfile BelongUserProfile { get; set; } = null!;

    public virtual Season? Season { get; set; }

    public virtual ICollection<SportInstitutionAgeCategoryBelong> SportInstitutionAgeCategoryBelongs { get; } = new List<SportInstitutionAgeCategoryBelong>();

    public virtual SportInstitutionBranch SportInstitutionBranch { get; set; } = null!;
}
