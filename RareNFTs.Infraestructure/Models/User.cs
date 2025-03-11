using System;
using System.Collections.Generic;

namespace RareNFTs.Infraestructure.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public Guid IdRole { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
