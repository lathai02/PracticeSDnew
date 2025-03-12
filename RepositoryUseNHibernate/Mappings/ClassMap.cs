using FluentNHibernate.Mapping;
using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Mappings
{
    public class ClassMap : ClassMap<Class>
    {
        public ClassMap()
        {
            Table("Class");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Subject);
            Map(x => x.TeacherId).Nullable();

            // LazyLoad(): The Teacher property will be loaded only when it is accessed.
            References(x => x.Teacher).Column("TeacherId").LazyLoad();

            // Inverse(): The Students collection is the inverse side of the relationship.
            HasMany(x => x.Students).KeyColumn("ClassId").Inverse().Cascade.All();
        }
    }
}
