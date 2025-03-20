using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Dtos
{
    public class StudentDto
    {
        public int Number { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Class? Class { get; set; }
    }
}
