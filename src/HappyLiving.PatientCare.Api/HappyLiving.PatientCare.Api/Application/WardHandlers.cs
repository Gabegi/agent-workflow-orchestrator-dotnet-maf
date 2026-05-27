using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class WardHandlers
{
    public static IResult GetPatients(string id, IPatientCareDb db)
    {
        if (!db.Wards.Any(w => w.Id == id))
            return Results.NotFound();

        var patients = db.Patients.Where(p => p.WardId == id).ToList();
        return Results.Ok(patients);
    }

    public static IResult GetMedicationsDue(string id, string? time, IPatientCareDb db)
    {
        if (!db.Wards.Any(w => w.Id == id))
            return Results.NotFound();

        var patientIds = db.Patients.Where(p => p.WardId == id).Select(p => p.Id).ToHashSet();

        var medications = db.Medications
            .Where(m => patientIds.Contains(m.PatientId) &&
                        (time is null || m.ScheduledTime == time))
            .ToList();

        return Results.Ok(medications);
    }
}
