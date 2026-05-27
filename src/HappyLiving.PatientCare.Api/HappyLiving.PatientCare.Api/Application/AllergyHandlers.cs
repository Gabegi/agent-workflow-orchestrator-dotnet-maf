using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class AllergyHandlers
{
    public static IResult GetByPatient(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var allergies = db.Allergies.Where(a => a.PatientId == patientId).ToList();
        return Results.Ok(allergies);
    }

    public static IResult GetSevere(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var allergies = db.Allergies
            .Where(a => a.PatientId == patientId &&
                        a.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Results.Ok(allergies);
    }
}
