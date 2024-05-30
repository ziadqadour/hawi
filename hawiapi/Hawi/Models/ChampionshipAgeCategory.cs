using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ChampionshipAgeCategory
{
    public long ChampionshipAgeCategoryId { get; set; }

    public long ChampionshipId { get; set; }

    public byte AgeCategoryId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual SportInstitutionAgePriceAgeCategory AgeCategory { get; set; } = null!;

    public virtual Championship Championship { get; set; } = null!;
}
