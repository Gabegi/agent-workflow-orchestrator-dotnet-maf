using HappyLiving.PatientCare.Mcp.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("PatientCareApi", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PatientCareApi:BaseUrl"] ?? "http://localhost:5158");
});

builder.Services
    .AddMcpServer()
    .WithTools<PatientCareTools>();

var app = builder.Build();

app.MapMcp();

app.Run();
