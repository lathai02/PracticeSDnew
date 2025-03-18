using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Interfaces
{
    public interface IClassRepository : IGenericRepository<Class, int>
    {
        Task<List<Class>> GetAllClassWithTeacherAsync();
    }
}
