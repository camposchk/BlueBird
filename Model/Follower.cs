using System;
using System.Collections.Generic;

namespace Nova_pasta.Model;

public partial class Follower
{
    public int Id { get; set; }

    public int? UserIdFollowing { get; set; }

    public int? UserIdFollowed { get; set; }

    public virtual BirdUser? UserIdFollowedNavigation { get; set; }

    public virtual BirdUser? UserIdFollowingNavigation { get; set; }
}
