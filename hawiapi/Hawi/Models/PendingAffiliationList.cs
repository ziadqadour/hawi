using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class PendingAffiliationList
{
    public long PendingAffiliationListId { get; set; }

    public long UserProfileId { get; set; }

    public byte PendingAffiliationListTypeId { get; set; }

    public string CertificateImageUrl { get; set; } = null!;

    public byte? YearsOfExperiance { get; set; }

    public byte? PendingAffiliationStatusId { get; set; }

    public string? PendingAffiliationStatusReason { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Comment { get; set; }

    public virtual PendingAffiliationListPendingAffiliationListType PendingAffiliationListType { get; set; } = null!;

    public virtual PendingStatus? PendingAffiliationStatus { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
