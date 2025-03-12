using RepositoriesUseNHibernate.Interfaces;
using Services.Interfaces;
using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public List<Class> GetAllClassWithTeacher()
        {
            return _classRepository.GetAllClassWithTeacher();
        }

        public Class? GetClassById(int id)
        {
            return _classRepository.GetById(id);
        }
    }
}
