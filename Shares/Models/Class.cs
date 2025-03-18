using System;
using System.Collections.Generic;

namespace Shares.Models;

public partial class Class
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; } = null!;

    public virtual string Subject { get; set; } = null!;

    public virtual int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Teacher? Teacher { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Subject: {Subject}, Teacher: {Teacher?.Name}";
    }
}
