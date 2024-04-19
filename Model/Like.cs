using System;
using System.Collections.Generic;

namespace Nova_pasta.Model;

public partial class Like
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public virtual BirdBlueet? Post { get; set; }

    public virtual BirdUser? User { get; set; }
}
