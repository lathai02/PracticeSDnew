using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Shares.ServiceContracts
{
    [DataContract]
    public class ClassRequest
    {
        [DataMember(Order = 1)]
        public int ClassId { get; set; }
    }

    [DataContract]
    public class ClassResponse
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Subject { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string? TeacherName { get; set; }
    }

    [DataContract]
    public class ClassListResponse
    {
        [DataMember(Order = 1)]
        public List<ClassResponse> Classes { get; set; } = new List<ClassResponse>();
    }

    [DataContract]
    public class ClassResponseChart
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string? Name { get; set; }

        [DataMember(Order = 3)]
        public string? Subject { get; set; }

        [DataMember(Order = 4)]
        public string? TeacherName { get; set; }

        [DataMember(Order = 5)]
        public List<StudentResponseChart> Students { get; set; } = new List<StudentResponseChart>();
    }

    [ServiceContract]
    public interface IClassProto
    {
        [OperationContract]
        Task<ResponseObj<ClassListResponse>> GetAllClassWithTeacherAsync(Empty request);

        [OperationContract]
        Task<ResponseObj<ClassResponse>> GetClassByIdAsync(ClassRequest request);
    }
}

