using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using Shares.ServiceContracts;
using Controllers;
using Microsoft.Extensions.Configuration;

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
                .AddSingleton(GrpcChannel.ForAddress(grpcUrl))
                .AddSingleton(serviceProvider =>
                {
                    return serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<IStudentProto>();
                })
                .AddSingleton<Controllers>()
                .BuildServiceProvider();

            var studentController = serviceProvider.GetRequiredService<Controllers>();
            await studentController.ManageStudentAsync();
        }
    }
}
