using Controllers;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using RepositoriesUseNHibernate.Implements;
using RepositoriesUseNHibernate.Interfaces;
using RepositoriesUseNHibernate.Mappings;
using Services.Implements;
using Services.Interfaces;
using Shares.Models;
using System.IO.Pipelines;

namespace Controllers
{
    static class Program
    {
        static void Main()
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .Build();

            string connectionString = configuration.GetConnectionString("MyConnectionString") ?? throw new InvalidOperationException("Cannot find string connection!");

            var serviceProvider = new ServiceCollection()
                .AddSingleton(factory =>
                {
                    return Fluently.Configure()
                         .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                         .Mappings(m => m.FluentMappings
                             .AddFromAssemblyOf<StudentMap>()
                             .AddFromAssemblyOf<ClassMap>()
                             .AddFromAssemblyOf<TeacherMap>())
                         .BuildSessionFactory();
                })
                .AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession())
                .AddSingleton<Controllers>()
                //.AddDbContext<SchoolDbContext>()
                .AddSingleton<IStudentService, StudentService>()
                .AddSingleton<IStudentRepository, StudentRepository>()
                .AddSingleton<IClassRepository, ClassRepository>()
                .BuildServiceProvider();

            var studentController = serviceProvider.GetRequiredService<Controllers>();
            studentController.ManageStudent();
        }
    }
}


