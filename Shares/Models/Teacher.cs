using System;
using System.Collections.Generic;

namespace Shares.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
