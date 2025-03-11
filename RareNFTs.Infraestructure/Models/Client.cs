using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class Client
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Genre { get; set; }

    public string? IdCountry { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<ClientNft> ClientNft { get; set; } = new List<ClientNft>();

    public virtual Country? IdCountryNavigation { get; set; }

    public virtual ICollection<InvoiceHeader> InvoiceHeader { get; set; } = new List<InvoiceHeader>();
}
