using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GrpcService.Services.Implements;
using GrpcService.Services.Interfaces;
using NHibernate;
using ProtoBuf.Grpc.Server;
using RepositoriesUseNHibernate.Implements;
using RepositoriesUseNHibernate.Interfaces;
using RepositoriesUseNHibernate.Mappings;
using Shares.ServiceContracts;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);
var app = builder.Build();
Configure(app);

await app.RunAsync();

void ConfigureServices(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();

    string connectionString = configuration.GetConnectionString("MyConnectionString") ?? throw new InvalidOperationException("Cannot find string connection!");

    services.AddGrpc();
    services.AddCodeFirstGrpc();

    services.AddSingleton(factory =>
      {
          return Fluently.Configure()
               .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
               .Mappings(m => m.FluentMappings
                   .AddFromAssemblyOf<StudentMap>()
                   .AddFromAssemblyOf<ClassMap>()
                   .AddFromAssemblyOf<TeacherMap>())
               .BuildSessionFactory();
      });
    services.AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());
    services.AddSingleton<IStudentProto, StudentService>();
    services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
    services.AddSingleton<IClassService, ClassService>();
    services.AddSingleton<IStudentRepository, StudentRepository>();
    services.AddSingleton<IClassRepository, ClassRepository>();
}

void Configure(WebApplication app)
{
    app.MapGrpcService<StudentService>();
}
