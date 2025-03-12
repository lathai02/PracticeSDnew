using System;
using System.Collections.Generic;

namespace Shares.Models;

public partial class Teacher
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; } = null!;

    public virtual DateTime DateOfBirth { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
