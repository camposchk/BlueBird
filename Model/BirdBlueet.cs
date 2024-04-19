using System;
using System.Collections.Generic;

namespace Nova_pasta.Model;

public partial class BirdBlueet
{
    public int Id { get; set; }

    public string? Content { get; set; }

    public int Likes { get; set; }

    public DateTime? PostDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Like> LikesNavigation { get; } = new List<Like>();

    public virtual BirdUser? User { get; set; }
}
