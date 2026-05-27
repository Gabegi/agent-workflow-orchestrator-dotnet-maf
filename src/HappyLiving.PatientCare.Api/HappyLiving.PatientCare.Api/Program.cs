using HappyLiving.PatientCare.Api.Endpoints;
using HappyLiving.PatientCare.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPatientCareDb, JsonPatientCareDb>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.MapMedicationsDue();
app.MapCarePlans();
app.MapAllergies();
app.MapAppointments();
app.MapHistory();

app.Run();
