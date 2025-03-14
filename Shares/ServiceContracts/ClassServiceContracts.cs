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

    [ServiceContract]
    public interface IClassProto
    {
        [OperationContract]
        Task<ClassListResponse> GetAllClassWithTeacherAsync(Empty request);

        [OperationContract]
        Task<ClassResponse?> GetClassByIdAsync(ClassRequest request);
    }
}

