using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Article
{
    public long ArticleId { get; set; }

    public long UserProfileId { get; set; }

    public string? Title { get; set; }

    public string? ArticleText { get; set; }

    public DateTime? LastUpdate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime ArticleCreateDate { get; set; }

    public virtual ICollection<ArticleAdvertisementCity> ArticleAdvertisementCities { get; } = new List<ArticleAdvertisementCity>();

    public virtual ICollection<ArticleAdvertisement> ArticleAdvertisements { get; } = new List<ArticleAdvertisement>();

    public virtual ICollection<ArticleComment> ArticleComments { get; } = new List<ArticleComment>();

    public virtual ICollection<ArticleHide> ArticleHides { get; } = new List<ArticleHide>();

    public virtual ICollection<ArticleImage> ArticleImages { get; } = new List<ArticleImage>();

    public virtual ICollection<ArticleLike> ArticleLikes { get; } = new List<ArticleLike>();

    public virtual ICollection<ArticleNotification> ArticleNotifications { get; } = new List<ArticleNotification>();

    public virtual ICollection<ArticleUsersSaved> ArticleUsersSaveds { get; } = new List<ArticleUsersSaved>();

    public virtual ICollection<ArticleVideo> ArticleVideos { get; } = new List<ArticleVideo>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
