using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using GrpcService.Services.Implements;
using NHibernate;
using ProtoBuf.Grpc.Server;
using RepositoriesUseNHibernate.Implements;
using RepositoriesUseNHibernate.Interfaces;
using Shares.ServiceContracts;
using RepositoriesUseNHibernate.Mappings;
using NHibernate.Mapping.ByCode.Impl;
using Shares.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Cấu hình Services
ConfigureServices(builder.Services);

var app = builder.Build();

// 2️⃣ Cấu hình Middleware
Configure(app);

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();

void ConfigureServices(IServiceCollection services)
{
    // Đọc cấu hình từ appsettings.json
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    string connectionString = configuration.GetConnectionString("MyConnectionString")
        ?? throw new InvalidOperationException("Cannot find string connection!");

    // 🔑 NHibernate Configuration
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

    // Đăng ký Session
    services.AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());

    // 🔧 Repository
    services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
    services.AddScoped<IStudentRepository, StudentRepository>();
    services.AddScoped<IClassRepository, ClassRepository>();

    // 🛠️ AutoMapper (gộp lại)
    services.AddAutoMapper(typeof(ClassMappingProfile), typeof(StudentMappingProfile));

    // 📌 gRPC Configuration
    services.AddGrpc();
    services.AddCodeFirstGrpc();
    services.AddGrpcReflection();

    // 🌐 Proto Service
    services.AddScoped<IStudentProto, StudentService>();
    services.AddScoped<IClassProto, ClassService>();
}

void Configure(WebApplication app)
{
    // Map gRPC Services
    app.MapGrpcService<StudentService>();
    app.MapGrpcService<ClassService>();

    // gRPC Reflection (chỉ dùng khi phát triển)
    if (app.Environment.IsDevelopment())
    {
        app.MapGrpcReflectionService();
    }
}
