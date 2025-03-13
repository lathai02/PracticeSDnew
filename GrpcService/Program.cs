using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using GrpcService.Services.Implements;
using GrpcService.Services.Interfaces;
using NHibernate;
using ProtoBuf.Grpc.Server;
using RepositoriesUseNHibernate.Implements;
using RepositoriesUseNHibernate.Interfaces;
using Shares.ServiceContracts;
using RepositoriesUseNHibernate.Mappings;

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
    services.AddScoped<IStudentProto, StudentService>();
    services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
    services.AddScoped<IClassService, ClassService>();
    services.AddScoped<IStudentRepository, StudentRepository>();
    services.AddScoped<IClassRepository, ClassRepository>();
}

void Configure(WebApplication app)
{
    app.MapGrpcService<StudentService>();
}
