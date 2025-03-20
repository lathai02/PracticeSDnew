using Shares.Enums;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Dtos
{
    public class TeacherStatistic
    {
        public string Subject { get; set; } = null!;
        public string? Type { get; set; }
        public int Value { get; set; }
    }

    public class ClassWithStudentsResponse
    {
        public string SubjectName { get; set; } = string.Empty;
        public List<ClassResponseChart> Classes { get; set; } = new List<ClassResponseChart>();
        public List<StudentResponseChart> Students { get; set; } = new List<StudentResponseChart>();
    }
}
