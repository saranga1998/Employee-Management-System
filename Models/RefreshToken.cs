using System;
using System.Collections.Generic;

namespace EMS_Project.Models;

public partial class RefreshToken
{
    public string TokenId { get; set; } = null!;

    public string Token { get; set; } = null!;

    public string Id { get; set; } = null!;
}
