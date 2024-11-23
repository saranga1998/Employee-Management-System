using System;
using System.Collections.Generic;

namespace EMS_Project.Models;

public partial class RegistedUser
{
    public string Userid { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;
}
