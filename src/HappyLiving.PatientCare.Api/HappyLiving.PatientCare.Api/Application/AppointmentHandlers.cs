using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class AppointmentHandlers
{
    public static IResult GetByPatient(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var appointments = db.Appointments.Where(a => a.PatientId == patientId).ToList();
        return Results.Ok(appointments);
    }

    public static IResult GetUpcoming(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var appointments = db.Appointments
            .Where(a => a.PatientId == patientId &&
                        string.Compare(a.DateTime, today, StringComparison.Ordinal) >= 0)
            .OrderBy(a => a.DateTime)
            .ToList();

        return Results.Ok(appointments);
    }
}
