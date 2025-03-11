using Controllers;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Implements;
using Repositories.Interfaces;
using Services.Implements;
using Services.Interfaces;

namespace Controllers
{
    static class Program
    {
        static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Controllers>()
                .AddDbContext<SchoolDbContext>()
                .AddSingleton<IStudentService, StudentService>()
                .AddSingleton<IStudentRepository, StudentRepository>()
                .AddSingleton<IClassRepository, ClassRepository>()
                .BuildServiceProvider();

            var studentController = serviceProvider.GetRequiredService<Controllers>();
            studentController.ManageStudent();
        }
    }
}


