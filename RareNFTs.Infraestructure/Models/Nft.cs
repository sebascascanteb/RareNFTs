using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class Nft
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public byte[] Image { get; set; } = default!;

    public DateTime? Date { get; set; }

    public string? Author { get; set; }

    public int? Quantity { get; set; }

    public virtual ICollection<ClientNft> ClientNft { get; set; } = new List<ClientNft>();

    public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; } = new List<InvoiceDetail>();
}
