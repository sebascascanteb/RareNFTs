using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class ClientNft
{
    public Guid IdClient { get; set; }

    public Guid IdNft { get; set; }

    public DateTime? Date { get; set; }

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Nft IdNftNavigation { get; set; } = null!;
}
