using System;
using System.Collections.Generic;

namespace Hawi.Models;

public partial class PlayerPlayerPlace
{
    public byte PlayerPlaceId { get; set; }

    public string? PlayerPlace { get; set; }

    public byte? PlayerPositionNumber { get; set; }

    public virtual ICollection<MatchPlayer> MatchPlayers { get; } = new List<MatchPlayer>();

    public virtual ICollection<Player> PlayerAlternativePlaces { get; } = new List<Player>();

    public virtual ICollection<Player> PlayerMainPlaces { get; } = new List<Player>();

    public virtual ICollection<UserBasicDatum> UserBasicDatumPlayerMainPlaces { get; } = new List<UserBasicDatum>();

    public virtual ICollection<UserBasicDatum> UserBasicDatumPlayerSecondaryPlaces { get; } = new List<UserBasicDatum>();
}
