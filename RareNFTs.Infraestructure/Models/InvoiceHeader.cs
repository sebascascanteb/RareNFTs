using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class InvoiceHeader
{
    public Guid Id { get; set; }

    public Guid IdCard { get; set; }

    public Guid IdClient { get; set; }

    public DateTime? Date { get; set; }

    public int? NumCard { get; set; }

    public decimal? Total { get; set; }

    public bool Status { get; set; }

    public virtual Card IdCardNavigation { get; set; } = null!;

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; } = new List<InvoiceDetail>();
}
