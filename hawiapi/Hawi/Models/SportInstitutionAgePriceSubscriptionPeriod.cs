using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class SportInstitutionAgePriceSubscriptionPeriod
{
    public byte SubscriptionPeriodId { get; set; }

    public string? SubscriptionPeriod { get; set; }

    public virtual ICollection<SportInstitutionAgePrice> SportInstitutionAgePrices { get; } = new List<SportInstitutionAgePrice>();
}
