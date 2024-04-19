using System;
using System.Collections.Generic;

namespace Nova_pasta.Model;

public partial class BirdUser
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Pass { get; set; }

    public virtual ICollection<BirdBlueet> BirdBlueets { get; } = new List<BirdBlueet>();

    public virtual ICollection<Follower> FollowerUserIdFollowedNavigations { get; } = new List<Follower>();

    public virtual ICollection<Follower> FollowerUserIdFollowingNavigations { get; } = new List<Follower>();

    public virtual ICollection<Like> Likes { get; } = new List<Like>();
}
