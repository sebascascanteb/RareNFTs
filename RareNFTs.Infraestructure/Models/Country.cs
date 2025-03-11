using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class Country
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Client> Client { get; set; } = new List<Client>();
}
