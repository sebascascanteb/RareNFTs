using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class InvoiceDetail
{
    public Guid IdInvoice { get; set; }

    public Guid IdNft { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int Sequence { get; set; }

    public virtual InvoiceHeader IdInvoiceNavigation { get; set; } = null!;

    public virtual Nft IdNftNavigation { get; set; } = null!;
}
