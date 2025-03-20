using Shares.Models;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Interfaces
{
    public interface ITeacherRepository : IGenericRepository<Teacher, int>
    {
        Task<Teacher?> GetStudentAndClassByTeacherName(string teacherName);
    }
}
