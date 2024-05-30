using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionAgeCategoryBelong
{
    public long AgeCategoryBelongId { get; set; }

    public long SportInstitutionBelongId { get; set; }

    public byte AgeCategoryId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual SportInstitutionAgePriceAgeCategory AgeCategory { get; set; } = null!;

    public virtual SportInstitutionBelong SportInstitutionBelong { get; set; } = null!;
}
