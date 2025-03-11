using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class ClassRepository : IClassRepository
    {
        private readonly SchoolDbContext _schoolDbContext;

        public ClassRepository(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }
        public List<Class> GetAllClass()
        {
            return _schoolDbContext.Classes.Include(t => t.Teacher).ToList();
        }

        public Class? GetClassById(int id)
        {
            return _schoolDbContext.Classes.FirstOrDefault(c => c.Id == id);
        }
    }
}
