using FluentNHibernate.Mapping;
using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Mappings
{
    public class TeacherMap : ClassMap<Teacher>
    {
        public TeacherMap()
        {
            Table("Teacher");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.DateOfBirth);

            HasMany(x => x.Classes).KeyColumn("TeacherId").Inverse().Cascade.All();
        }
    }
}
