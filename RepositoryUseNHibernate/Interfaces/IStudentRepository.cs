using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student, string>
    {
        Task<List<Student>> GetStudentListWithClassAsync();
        Task<List<Student>> GetStudentListSortByNameAsync();
    }
}
