using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class CityDistrict
{
    public string CityDistrictsId { get; set; } = null!;

    public long CityId { get; set; }

    public string? DistractArabicName { get; set; }

    public string? DistractEnglishName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual City City { get; set; } = null!;
}
