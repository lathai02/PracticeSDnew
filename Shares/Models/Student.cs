using System;
using System.Collections.Generic;

namespace Shares.Models;

public partial class Student
{
    public virtual string Id { get; set; } = null!;

    public virtual string Name { get; set; } = null!;

    public virtual DateTime DateOfBirth { get; set; }

    public virtual string Address { get; set; } = null!;

    public virtual int? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Date of Birth: {DateOfBirth}, Address: {Address}, Class: {Class?.Name}";
    }
}
