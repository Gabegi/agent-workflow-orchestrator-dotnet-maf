using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class PatientHandlers
{
    public static IResult GetById(string id, IPatientCareDb db)
    {
        var patient = db.Patients.FirstOrDefault(p => p.Id == id);
        return patient is not null ? Results.Ok(patient) : Results.NotFound();
    }

    public static IResult GetMedications(string id, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == id))
            return Results.NotFound();

        var medications = db.Medications.Where(m => m.PatientId == id).ToList();
        return Results.Ok(medications);
    }

    public static IResult GetCarePlan(string id, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == id))
            return Results.NotFound();

        var carePlan = db.CarePlans.FirstOrDefault(c => c.PatientId == id);
        return carePlan is not null ? Results.Ok(carePlan) : Results.NotFound();
    }

    public static IResult GetAllergies(string id, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == id))
            return Results.NotFound();

        var allergies = db.Allergies.Where(a => a.PatientId == id).ToList();
        return Results.Ok(allergies);
    }

    public static IResult GetHistory(string id, IPatientCareDb db)
    {
        var patient = db.Patients.FirstOrDefault(p => p.Id == id);
        return patient is not null ? Results.Ok(patient.History) : Results.NotFound();
    }

    public static IResult GetAppointments(string id, IPatientCareDb db)
    {
        var patient = db.Patients.FirstOrDefault(p => p.Id == id);
        return patient is not null ? Results.Ok(patient.Appointments) : Results.NotFound();
    }
}
