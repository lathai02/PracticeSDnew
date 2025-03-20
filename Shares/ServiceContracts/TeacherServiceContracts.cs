using Shares.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Shares.ServiceContracts
{
    [DataContract]
    public class RequestTeacherChart
    {
        [DataMember(Order = 1)]
        public string TeacherName { get; set; } = null!;
    }

    [DataContract]
    public class TeacherResponseChart
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string DateOfBirth { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public List<ClassResponseChart> Classes { get; set; } = new List<ClassResponseChart>();
    }

    [ServiceContract]
    public interface ITeacherProto
    {
        [OperationContract]
        Task<ResponseObj<TeacherResponseChart>> GetListTeacherWithClassStudentAsync(RequestTeacherChart request);

        [OperationContract]
        Task<ResponseObj<List<TeacherResponseChart>>> GetAllTeacher(Empty request);
    }
}
