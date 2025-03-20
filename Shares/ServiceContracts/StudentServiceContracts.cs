using Grpc.Core;
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
    public class RequestStudent
    {
        [DataMember(Order = 1)]
        public string StudentId { get; set; } = null!;
    }

    [DataContract]
    public class RequestStudentAdd
    {
        [DataMember(Order = 1)]
        public string Id { get; set; } = null!;

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Address { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public DateTime DateOfBirth { get; set; }

        [DataMember(Order = 5)]
        public ClassResponse ClassResponse { get; set; } = new ClassResponse();
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

        [DataMember(Order = 6)]
        public ClassResponse ClassResponse { get; set; } = new ClassResponse();
    }

    [DataContract]
    public class StudentListResponse
    {
        [DataMember(Order = 1)]
        public List<StudentResponse> Students { get; set; } = new List<StudentResponse>();
    }

    [DataContract]
    public class StudentResponseChart
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

    [ServiceContract]
    public interface IStudentProto
    {
        [OperationContract]
        Task<ResponseObj<StudentListResponse>> GetListStudentAsync(PagingRequest request);

        [OperationContract]
        Task<ResponseObj<EmptyResponse>> AddStudentAsync(RequestStudentAdd request);

        [OperationContract]
        Task<ResponseObj<EmptyResponse>> UpdateStudentAsync(RequestStudentAdd request);

        [OperationContract]
        Task<ResponseObj<EmptyResponse>> DeleteStudentAsync(RequestStudent request);

        [OperationContract]
        Task<ResponseObj<StudentListResponse>> SortStudentListByNameAsync(PagingRequest request);

        [OperationContract]
        Task<ResponseObj<StudentResponse>> SearchByStudentIdAsync(RequestStudent request);

        [OperationContract]
        Task<ResponseObj<ResponseNumber>> GetTotalCountAsync(Empty empty);

        [OperationContract]
        Task<ResponseObj<StudentListResponse>> SortStudentListByOptAsync(PagingRequestSort request);
    }
}