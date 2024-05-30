using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionSportInstitutionType
{
    public byte SportInstitutionTypeId { get; set; }

    public string? SportInstitutionType { get; set; }

    public string? ImagePath { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Championship> Championships { get; } = new List<Championship>();

    public virtual ICollection<SportInstitutionAgePriceAgeCategory> SportInstitutionAgePriceAgeCategories { get; } = new List<SportInstitutionAgePriceAgeCategory>();

    public virtual ICollection<SportInstitutionAgePrice> SportInstitutionAgePrices { get; } = new List<SportInstitutionAgePrice>();

    public virtual ICollection<SportInstitution> SportInstitutions { get; } = new List<SportInstitution>();
}
