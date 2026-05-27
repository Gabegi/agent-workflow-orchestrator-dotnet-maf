using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class HistoryHandlers
{
    public static IResult GetByPatient(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var history = db.History.Where(h => h.PatientId == patientId).ToList();
        return Results.Ok(history);
    }

    public static IResult GetRecent(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var cutoff = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
        var history = db.History
            .Where(h => h.PatientId == patientId &&
                        string.Compare(h.Date, cutoff, StringComparison.Ordinal) >= 0)
            .OrderByDescending(h => h.Date)
            .ToList();

        return Results.Ok(history);
    }
}
