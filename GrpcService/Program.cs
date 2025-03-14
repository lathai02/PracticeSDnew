using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using GrpcService.Services.Implements;
using NHibernate;
using ProtoBuf.Grpc.Server;
using RepositoriesUseNHibernate.Implements;
using RepositoriesUseNHibernate.Interfaces;
using Shares.ServiceContracts;
using RepositoriesUseNHibernate.Mappings;
using Shares.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Lấy connection string từ builder.Configuration
string connectionString = builder.Configuration.GetConnectionString("MyConnectionString")
    ?? throw new InvalidOperationException("Cannot find string connection!");

// NHibernate Configuration
builder.Services.AddSingleton(factory =>
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
builder.Services.AddScoped(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());

// Repository
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ClassMappingProfile), typeof(StudentMappingProfile));

// gRPC Configuration
builder.Services.AddGrpc();
builder.Services.AddCodeFirstGrpc();
builder.Services.AddGrpcReflection();

// Proto Service
builder.Services.AddScoped<IStudentProto, StudentService>();
builder.Services.AddScoped<IClassProto, ClassService>();

var app = builder.Build();

// Map gRPC Services
app.MapGrpcService<StudentService>();
app.MapGrpcService<ClassService>();

// gRPC Reflection (chỉ dùng khi phát triển)
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();
