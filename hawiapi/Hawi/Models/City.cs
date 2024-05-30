using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class City
{
    public long CityId { get; set; }

    public byte? CityCountryId { get; set; }

    public string? CityArabicName { get; set; }

    public string? CityEnglishName { get; set; }

    public string? CityAdressLatitude { get; set; }

    public string? CityAdressLongitude { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<AdvertisementCity> AdvertisementCities { get; } = new List<AdvertisementCity>();

    public virtual ICollection<ArticleAdvertisementCity> ArticleAdvertisementCities { get; } = new List<ArticleAdvertisementCity>();

    public virtual ICollection<Championship> Championships { get; } = new List<Championship>();

    public virtual CityCountry? CityCountry { get; set; }

    public virtual ICollection<CityDistrict> CityDistricts { get; } = new List<CityDistrict>();

    public virtual ICollection<Club> Clubs { get; } = new List<Club>();

    public virtual ICollection<Playground> Playgrounds { get; } = new List<Playground>();

    public virtual ICollection<SportInstitutionBranch> SportInstitutionBranches { get; } = new List<SportInstitutionBranch>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
