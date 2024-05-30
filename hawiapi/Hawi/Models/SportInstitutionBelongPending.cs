using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionBelongPending
{
    public long SportInstitutionBelongPendingId { get; set; }

    public long BelongUserProfileId { get; set; }

    public string? ShortDescription { get; set; }

    public byte? BelongTypeId { get; set; }

    public long? SportInstitutionBranchId { get; set; }

    public byte? SeasonId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual SportInstitutionBelongBelongType? BelongType { get; set; }

    public virtual UserProfile BelongUserProfile { get; set; } = null!;

    public virtual Season? Season { get; set; }

    public virtual ICollection<SportInstitutionBelongPendingAgeCategory> SportInstitutionBelongPendingAgeCategories { get; } = new List<SportInstitutionBelongPendingAgeCategory>();
}
