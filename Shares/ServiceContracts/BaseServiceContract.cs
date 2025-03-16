using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shares.ServiceContracts
{
    [DataContract]
    public class Empty { }

    [DataContract]
    public class ResponseObj<T>
    {
        [DataMember(Order = 1)]
        public string Message { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public T? Data { get; set; }
    }

    [DataContract]
    public class EmptyResponse { }
}
