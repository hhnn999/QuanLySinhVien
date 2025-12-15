using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GRPC_NHibernate.Mappings;
using GRPC_NHibernate.Services;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();         
builder.Services.AddCodeFirstGrpc();

// =============== NHIBERNATE CONFIG ===============
ISessionFactory sessionFactory = Fluently.Configure()
    .Database(
        MsSqlConfiguration.MsSql2012
            .ConnectionString("Server=.\\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;")
            .ShowSql()
    )
    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<StudentMap>())
    .ExposeConfiguration(cfg =>
    {
        new SchemaUpdate(cfg).Execute(false, true);
    })
    .BuildSessionFactory();

// DI Injection NHibernate
builder.Services.AddSingleton(sessionFactory);
builder.Services.AddScoped(factory => sessionFactory.OpenSession());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7102") // URL client
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("grpc-status", "grpc-message", "grpc-status-details-bin");
    });
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");
app.UseGrpcWeb();

app.MapGrpcService<StudentService>()
   .EnableGrpcWeb()
   .RequireCors("AllowBlazorClient");

app.MapGrpcService<ClassService>()
   .EnableGrpcWeb()
   .RequireCors("AllowBlazorClient");

app.MapGrpcService<TeacherService>()
   .EnableGrpcWeb()
   .RequireCors("AllowBlazorClient");

app.MapGet("/", () => "Server is running...");

app.Run();
