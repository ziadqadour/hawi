using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class UserBasicDatum
{
    public long UserBasicDataId { get; set; }

    public long UserId { get; set; }

    public string? Idnumber { get; set; }

    public string? FirstName { get; set; }

    public string? FatherName { get; set; }

    public string? GrandFatherName { get; set; }

    public string? FamilyName { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public DateTime? Dob { get; set; }

    public string? Pob { get; set; }

    public string? PlaceOfResidence { get; set; }

    public byte? NationalityId { get; set; }

    public string? LocalFavoritePlayer { get; set; }

    public string? InternationalFavoritePlayer { get; set; }

    public int? LocalFavoriteClubId { get; set; }

    public int? InternationalFavoriteClubId { get; set; }

    public byte? InternationalFavoriteTeamId { get; set; }

    public byte? QualificationTypeId { get; set; }

    public byte? PlayerMainPlaceId { get; set; }

    public byte? PlayerSecondaryPlaceId { get; set; }

    public byte? PlayerFeetId { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? OfficialPhotoForMatches { get; set; }

    public virtual Club? InternationalFavoriteClub { get; set; }

    public virtual CityCountry? InternationalFavoriteTeam { get; set; }

    public virtual Club? LocalFavoriteClub { get; set; }

    public virtual CityCountry? Nationality { get; set; }

    public virtual PlayerPlayerFoot? PlayerFeet { get; set; }

    public virtual PlayerPlayerPlace? PlayerMainPlace { get; set; }

    public virtual PlayerPlayerPlace? PlayerSecondaryPlace { get; set; }

    public virtual UserBasicDataQualificationType? QualificationType { get; set; }

    public virtual User User { get; set; } = null!;
}
