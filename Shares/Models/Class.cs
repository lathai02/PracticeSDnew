using System;
using System.Collections.Generic;

namespace Shares.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Teacher? Teacher { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Subject: {Subject}, Teacher: {Teacher?.Name}";
    }
}
