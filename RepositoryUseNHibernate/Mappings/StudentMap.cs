using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Mappings
{
    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Table("Student");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.DateOfBirth);
            Map(x => x.Address);

            References(x => x.Class).Column("ClassId").LazyLoad();
        }
    }
}
