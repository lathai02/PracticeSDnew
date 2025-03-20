using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ProtoBuf.Grpc.Client;
using Shares.MappingProfiles;
using Shares.ServiceContracts;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);

string grpcUrl = builder.Configuration.GetSection("GrpcServer")["Url"] ?? throw new InvalidOperationException("Cannot find gRPC URL!");

// Add services to the container.
builder.Services.AddAntDesign();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(GrpcChannel.ForAddress(grpcUrl));
builder.Services.AddSingleton(serviceProvider =>
    serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<IStudentProto>());
builder.Services.AddSingleton(serviceProvider =>
    serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<IClassProto>());
builder.Services.AddSingleton(serviceProvider =>
    serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<ITeacherProto>());
builder.Services.AddAutoMapper(typeof(ClassMappingProfile), typeof(StudentMappingProfile), typeof(TeacherMappingProfile));
builder.Services.AddSingleton<StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();
