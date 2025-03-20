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
        Task<List<Student>> GetStudentListWithClassAsync(int pageNumber, int pageSize);
        Task<List<Student>> GetStudentListSortByNameAsync(int pageNumber, int pageSize);
        Task<List<Student>> GetStudentListAsync(int pageNumber, int pageSize, string sortBy, bool isAscending = true);
    }
}
