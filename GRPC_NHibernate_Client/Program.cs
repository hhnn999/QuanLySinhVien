using AntDesign;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GRPC_NHibernate_Client;
using GRPC_NHibernate_Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProtoBuf.Grpc.Client;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var culture = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;


builder.Services.AddSingleton(services =>
{
    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

    return GrpcChannel.ForAddress("https://localhost:7068", new GrpcChannelOptions
    {
        HttpHandler = httpHandler
    });
});


builder.Services.AddScoped<StudentApiClient>();
builder.Services.AddScoped<ClassRoomApiClient>();
builder.Services.AddScoped<TeacherApiClient>();
builder.Services.AddAntDesign();
    

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

await builder.Build().RunAsync();
