using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using RepositoriesUseNHibernate.Interfaces;
using Shares.Models;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Implements
{
    public class TeacherRepository : GenericRepository<Teacher, int>, ITeacherRepository
    {
        private readonly ISession _session;

        public TeacherRepository(ISession session) : base(session)
        {
            _session = session;
        }

        public async Task<Teacher?> GetStudentAndClassByTeacherName(string teacherName)
        {
            try
            {

                // Truy vấn Teacher và Classes trước
                var teacher = await _session.QueryOver<Teacher>()
                    .Fetch(SelectMode.Fetch, t => t.Classes)
                    .Where(t => t.Name == teacherName)
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .SingleOrDefaultAsync();

                // Load Students riêng cho từng Class (Tránh multiple bags)
                if (teacher != null)
                {
                    foreach (var cls in teacher.Classes)
                    {
                        NHibernateUtil.Initialize(cls.Students);  // Nạp Students từng Class
                    }
                }



                return teacher;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
