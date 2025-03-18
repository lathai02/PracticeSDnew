﻿using NHibernate;
using RepositoriesUseNHibernate.Interfaces;
using Shares.Models;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Implements
{
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        private readonly ISession _session;

        public StudentRepository(ISession session) : base(session)
        {
            _session = session;
        }

        public async Task<List<Student>> GetStudentListWithClassAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _session.Query<Student>()
                                  .Fetch(s => s.Class)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<Student>();
        }

        public async Task<List<Student>> GetStudentListSortByNameAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _session.Query<Student>()
                                     .Fetch(s => s.Class)
                                     .OrderBy(s => s.Name)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<Student>();
        }
    }
}
