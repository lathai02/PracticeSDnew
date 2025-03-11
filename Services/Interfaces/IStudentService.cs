using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStudentService
    {
        void PrintStudentList();
        void AddStudent();
        void UpdateStudent();
        void DeleteStudent();
        void SortStudentListByName();
        void SearchByStudentId();
    }
}
