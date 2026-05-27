using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class CarePlanEndpoints
{
    public static void MapCarePlans(this WebApplication app)
    {
        var group = app.MapGroup("/careplans")
            .WithTags("Care Plans");

        group.MapGet("/{patientId}", CarePlanHandlers.GetByPatient)
            .WithName("GetCarePlanByPatient")
            .WithDescription("Retrieve the full care plan for a patient")
            .Produces<CarePlan>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{patientId}/goals", CarePlanHandlers.GetGoals)
            .WithName("GetCarePlanGoals")
            .WithDescription("Retrieve goals from a patient's care plan")
            .Produces<List<Goal>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{patientId}/interventions", CarePlanHandlers.GetInterventions)
            .WithName("GetCarePlanInterventions")
            .WithDescription("Retrieve interventions from a patient's care plan")
            .Produces<List<Intervention>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
