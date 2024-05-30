using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public long CityId { get; set; }

    public string? ClubArabicName { get; set; }

    public string? ClubEnglishName { get; set; }

    public string? ClubLogoFileName { get; set; }

    public string? ClubLogoUrlfullPath { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<UserBasicDatum> UserBasicDatumInternationalFavoriteClubs { get; } = new List<UserBasicDatum>();

    public virtual ICollection<UserBasicDatum> UserBasicDatumLocalFavoriteClubs { get; } = new List<UserBasicDatum>();
}
