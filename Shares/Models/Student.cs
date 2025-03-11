using System;
using System.Collections.Generic;

namespace Shares.Models;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; } = null!;

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }
}
