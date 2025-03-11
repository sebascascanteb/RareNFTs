using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Description { get; set; }

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
