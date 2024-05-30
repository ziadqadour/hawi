using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class Event
{
    public long EventId { get; set; }

    public long UserProfileId { get; set; }

    public string? EventTitle { get; set; }

    public string? EventText { get; set; }

    public DateTime? StratDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public string? EventPlaceLocation { get; set; }

    public DateTime? LastUpdate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime EventCreateDate { get; set; }

    public virtual ICollection<EventImage> EventImages { get; } = new List<EventImage>();

    public virtual ICollection<EventVideo> EventVideos { get; } = new List<EventVideo>();

    public virtual UserProfile UserProfile { get; set; } = null!;
}
