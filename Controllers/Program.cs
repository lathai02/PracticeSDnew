using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using Shares.ServiceContracts;
using Controllers;
using Microsoft.Extensions.Configuration;
using Shares.MappingProfiles;

namespace Controllers
{
    static class Program
    {
        static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .Build();

            string grpcUrl = configuration.GetSection("GrpcServer")["Url"] ?? throw new InvalidOperationException("Cannot find gRPC URL!");

            var serviceProvider = new ServiceCollection()
                // Khởi tạo GrpcChannel một lần
                .AddSingleton(GrpcChannel.ForAddress(grpcUrl))

                // Tạo gRPC service từ GrpcChannel
                .AddSingleton(serviceProvider =>
                    serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<IStudentProto>())
                .AddSingleton(serviceProvider =>
                    serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<IClassProto>())

                // AutoMapper chỉ cần thêm 1 lần với nhiều profiles
                .AddAutoMapper(typeof(ClassMappingProfile), typeof(StudentMappingProfile))

                // Controller
                .AddSingleton<Controllers>()
                .BuildServiceProvider();

            var studentController = serviceProvider.GetRequiredService<Controllers>();
            await studentController.ManageStudentAsync();
        }
    }
}
