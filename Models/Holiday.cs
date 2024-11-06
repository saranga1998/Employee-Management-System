using System;
using System.Collections.Generic;

namespace EMS_Project.Models;

public partial class Holiday
{
    public int DayId { get; set; }

    public DateOnly? Holiday1 { get; set; }

    public string Title { get; set; } = null!;
}
