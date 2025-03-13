using Microsoft.IdentityModel.Tokens;
using ProtoBuf.Grpc.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shares.ServiceContracts
{
    [DataContract]
    public class Empty
    {
    }

    [Service]
    public interface IStudentProto
    {
        [Operation]
        Task<Empty> PrintStudentListAsync(Empty request, CallContext? context = default);

        [Operation]
        Task<Empty> AddStudentAsync(Empty request, CallContext?  context = default);

        [Operation]
        Task<Empty> UpdateStudentAsync(Empty request, CallContext? context = default);

        [Operation]
        Task<Empty> DeleteStudentAsync(Empty request, CallContext? context = default);

        [Operation]
        Task<Empty> SortStudentListByNameAsync(Empty request, CallContext? context = default);

        [Operation]
        Task<Empty> SearchByStudentIdAsync(Empty request, CallContext? context = default);
    }
}
