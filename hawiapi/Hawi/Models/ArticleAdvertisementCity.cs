using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class ArticleAdvertisementCity
{
    public long ArticleAdvertisementCityId { get; set; }

    public long ArticleId { get; set; }

    public long CityId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual City City { get; set; } = null!;
}
