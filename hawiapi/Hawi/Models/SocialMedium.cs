using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SocialMedium
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string? SocialMediaName { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UserTypeId { get; set; }

    public int? IdType { get; set; }

    public bool? IsActive { get; set; }

    public int? SocialMediaType { get; set; }
}
