using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? Name { get; set; }

    public string? Mobile { get; set; }

    public string? ParentChild { get; set; }

    public string? Password { get; set; }

    public bool? IsMale { get; set; }

    public byte? CityCountryId { get; set; }

    public long? CityId { get; set; }

    public short? VerifyCode { get; set; }

    public DateTime? LastSentVerifiedCodeTime { get; set; }

    public byte? UserStatusId { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public byte? LastLoginRoleId { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime UserCreateDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual City? City { get; set; }

    public virtual CityCountry? CityCountry { get; set; }

    public virtual Role? LastLoginRole { get; set; }

    public virtual ICollection<UserBasicDatum> UserBasicData { get; } = new List<UserBasicDatum>();

    public virtual ICollection<UserProfile> UserProfiles { get; } = new List<UserProfile>();

    public virtual UserUserStatus? UserStatus { get; set; }

    public virtual ICollection<UsersUsersActivationArchieve> UsersUsersActivationArchieves { get; } = new List<UsersUsersActivationArchieve>();
}
