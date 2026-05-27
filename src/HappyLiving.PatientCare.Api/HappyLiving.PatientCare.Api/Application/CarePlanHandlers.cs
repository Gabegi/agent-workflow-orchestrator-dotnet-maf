using HappyLiving.PatientCare.Api.Services;

namespace HappyLiving.PatientCare.Api.Application;

public static class CarePlanHandlers
{
    public static IResult GetByPatient(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var carePlan = db.CarePlans.FirstOrDefault(c => c.PatientId == patientId);
        return carePlan is not null ? Results.Ok(carePlan) : Results.NotFound();
    }

    public static IResult GetGoals(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var carePlan = db.CarePlans.FirstOrDefault(c => c.PatientId == patientId);
        return carePlan is not null ? Results.Ok(carePlan.Goals) : Results.NotFound();
    }

    public static IResult GetInterventions(string patientId, IPatientCareDb db)
    {
        if (!db.Patients.Any(p => p.Id == patientId))
            return Results.NotFound();

        var carePlan = db.CarePlans.FirstOrDefault(c => c.PatientId == patientId);
        return carePlan is not null ? Results.Ok(carePlan.Interventions) : Results.NotFound();
    }
}
