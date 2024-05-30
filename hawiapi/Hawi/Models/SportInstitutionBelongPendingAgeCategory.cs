using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionBelongPendingAgeCategory
{
    public long SportInstitutionBelongPendingAgeCategoryId { get; set; }

    public long SportInstitutionBelongPendingId { get; set; }

    public byte AgeCategoryId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SportInstitutionAgePriceAgeCategory AgeCategory { get; set; } = null!;

    public virtual SportInstitutionBelongPending SportInstitutionBelongPending { get; set; } = null!;
}
