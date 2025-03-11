using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class Card
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<InvoiceHeader> InvoiceHeader { get; set; } = new List<InvoiceHeader>();
}
