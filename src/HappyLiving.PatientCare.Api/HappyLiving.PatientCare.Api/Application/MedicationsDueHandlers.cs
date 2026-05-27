using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class MedicationsDueHandlers
{
    public static IResult GetByPatient(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var medications = db.MedicationsDue.Where(m => m.PatientId == patientId).ToList();
        return Results.Ok(medications);
    }

    public static IResult GetDueNow(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var currentHour = DateTime.Now.ToString("HH:00");
        var medications = db.MedicationsDue
            .Where(m => m.PatientId == patientId && m.ScheduledTime == currentHour)
            .ToList();

        return Results.Ok(medications);
    }

    public static IResult GetByWard(string wardId, IPatientCareDb db)
    {
        var patientIds = db.Patients
            .Where(p => p.WardId == wardId)
            .Select(p => p.Id)
            .ToHashSet();

        if (patientIds.Count == 0)
            return Results.NotFound();

        var medications = db.MedicationsDue
            .Where(m => patientIds.Contains(m.PatientId))
            .ToList();

        return Results.Ok(medications);
    }
}
