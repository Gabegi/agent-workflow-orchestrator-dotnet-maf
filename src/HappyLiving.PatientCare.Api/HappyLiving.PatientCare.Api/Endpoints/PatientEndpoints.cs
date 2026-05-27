using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class PatientEndpoints
{
    public static void MapPatients(this WebApplication app)
    {
        var group = app.MapGroup("/patients")
            .WithTags("Patients");

        group.MapGet("/{id}", PatientHandlers.GetById)
            .WithName("GetPatientById")
            .WithDescription("Retrieve a patient by ID")
            .Produces<Patient>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/medications", PatientHandlers.GetMedications)
            .WithName("GetPatientMedications")
            .WithDescription("Retrieve all medications for a patient")
            .Produces<List<Medication>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/careplan", PatientHandlers.GetCarePlan)
            .WithName("GetPatientCarePlan")
            .WithDescription("Retrieve the care plan for a patient")
            .Produces<CarePlan>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/allergies", PatientHandlers.GetAllergies)
            .WithName("GetPatientAllergies")
            .WithDescription("Retrieve all allergies for a patient")
            .Produces<List<Allergy>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/history", PatientHandlers.GetHistory)
            .WithName("GetPatientHistory")
            .WithDescription("Retrieve medical history for a patient")
            .Produces<List<HistoryEntry>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/appointments", PatientHandlers.GetAppointments)
            .WithName("GetPatientAppointments")
            .WithDescription("Retrieve upcoming appointments for a patient")
            .Produces<List<Appointment>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
