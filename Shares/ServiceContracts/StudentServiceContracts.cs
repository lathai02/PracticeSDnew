using Microsoft.IdentityModel.Tokens;
using ProtoBuf.Grpc.Configuration;
using Shares.Models;
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
    public class Empty { }

    [DataContract]
    public class RequestStudent
    {
        [DataMember(Order = 1)]
        public string StudentId { get; set; } = null!;
    }

    [DataContract]
    public class RequestStudentAdd
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Address { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public DateTime DateOfBirth { get; set; }

        [DataMember(Order = 5)]
        public Class Class { get; set; } = new Class();
    }

    [DataContract]
    public class StudentResponse
    {
        [DataMember(Order = 1)]
        public string StudentId { get; set; } = null!;

        [DataMember(Order = 2)]
        public string StudentName { get; set; } = null!;

        [DataMember(Order = 3)]
        public string StudentDateOfBirth { get; set; } = null!;

        [DataMember(Order = 4)]
        public string StudentAddress { get; set; } = null!;

        [DataMember(Order = 5)]
        public string ClassName { get; set; } = null!;
    }

    [DataContract]
    public class StudentListResponse
    {
        [DataMember(Order = 1)]
        public List<StudentResponse> Students { get; set; } = new List<StudentResponse>();
    }

    [ServiceContract]
    public interface IStudentProto
    {
        [OperationContract]
        Task<StudentListResponse> PrintStudentListAsync(Empty request, CallContext? context = default);

        [OperationContract]
        Task<Empty> AddStudentAsync(RequestStudentAdd request, CallContext? context = default);

        [OperationContract]
        Task<Empty> UpdateStudentAsync(RequestStudentAdd request, CallContext? context = default);

        [OperationContract]
        Task<Empty> DeleteStudentAsync(RequestStudent request, CallContext? context = default);

        [OperationContract]
        Task<StudentListResponse> SortStudentListByNameAsync(Empty request, CallContext? context = default);

        [OperationContract]
        Task<StudentResponse?> SearchByStudentIdAsync(RequestStudent request, CallContext? context = default);
    }
}