using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStudentService
    {
        Task PrintStudentListAsync();
        Task AddStudentAsync();
        Task UpdateStudentAsync();
        Task DeleteStudentAsync();
        Task SortStudentListByNameAsync();
        Task SearchByStudentIdAsync();
    }
}
