using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcService.Services.Interfaces
{
    public interface IClassService
    {
        Task<List<Class>> GetAllClassWithTeacherAsync();
        Task<Class?> GetClassByIdAsync(int id);
    }
}
