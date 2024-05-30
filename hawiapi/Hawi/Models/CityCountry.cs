using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class CityCountry
{
    public byte CityCountryId { get; set; }

    public string? CountryArabicName { get; set; }

    public string? CountryEnglishName { get; set; }

    public string? ImageUrl { get; set; }

    public byte? Length { get; set; }

    public string? Code { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? RegionCode { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<UserBasicDatum> UserBasicDatumInternationalFavoriteTeams { get; } = new List<UserBasicDatum>();

    public virtual ICollection<UserBasicDatum> UserBasicDatumNationalities { get; } = new List<UserBasicDatum>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
