using HappyLiving.PatientCare.Api.Models;
using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class PatientEndpoints
{
    public static void MapPatients(this WebApplication app)
    {
        var group = app.MapGroup("/patients")
            .WithTags("Patients");

        group.MapGet("/{id}", GetPatientById)
            .WithName("GetPatientById")
            .WithDescription("Retrieve a patient by ID")
            .Produces<Patient>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/medications", GetPatientMedications)
            .WithName("GetPatientMedications")
            .WithDescription("Retrieve all medications for a patient")
            .Produces<List<Medication>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/careplan", GetPatientCarePlan)
            .WithName("GetPatientCarePlan")
            .WithDescription("Retrieve the care plan for a patient")
            .Produces<CarePlan>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/allergies", GetPatientAllergies)
            .WithName("GetPatientAllergies")
            .WithDescription("Retrieve all allergies for a patient")
            .Produces<List<Allergy>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/history", GetPatientHistory)
            .WithName("GetPatientHistory")
            .WithDescription("Retrieve medical history for a patient")
            .Produces<List<HistoryEntry>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/appointments", GetPatientAppointments)
            .WithName("GetPatientAppointments")
            .WithDescription("Retrieve upcoming appointments for a patient")
            .Produces<List<Appointment>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static IResult GetPatientById(string id, IPatientCareDb db)
    {
        var patient = db.Patients.FirstOrDefault(p => p.Id == id);
        return patient is not null ? Results.Ok(patient) : Results.NotFound();
    }

    private static IResult GetPatientMedications(string id, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == id))
            return Results.NotFound();

        var medications = db.Medications.Where(m => m.PatientId == id).ToList();
        return Results.Ok(medications);
    }

    private static IResult GetPatientCarePlan(string id, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == id))
            return Results.NotFound();

        var carePlan = db.CarePlans.FirstOrDefault(c => c.PatientId == id);
        return carePlan is not null ? Results.Ok(carePlan) : Results.NotFound();
    }

    private static IResult GetPatientAllergies(string id, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == id))
            return Results.NotFound();

        var allergies = db.Allergies.Where(a => a.PatientId == id).ToList();
        return Results.Ok(allergies);
    }

    private static IResult GetPatientHistory(string id, IPatientCareDb db)
    {
        var patient = db.Patients.FirstOrDefault(p => p.Id == id);
        return patient is not null ? Results.Ok(patient.History) : Results.NotFound();
    }

    private static IResult GetPatientAppointments(string id, IPatientCareDb db)
    {
        var patient = db.Patients.FirstOrDefault(p => p.Id == id);
        return patient is not null ? Results.Ok(patient.Appointments) : Results.NotFound();
    }
}
