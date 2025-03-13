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
var configuration = builder.Configuration;

builder.WebHost.ConfigureKestrel(options =>
{
    var grpcUrl = configuration["Kestrel:Endpoints:Grpc:Url"] ?? "https://localhost:5001";
    options.ListenLocalhost(new Uri(grpcUrl).Port, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        listenOptions.UseHttps();
    });
});

ConfigureServices(builder.Services, configuration);
var app = builder.Build();
Configure(app);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
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
