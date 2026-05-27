using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class MedicationsDueEndpoints
{
    public static void MapMedicationsDue(this WebApplication app)
    {
        var group = app.MapGroup("/medications-due")
            .WithTags("Medications Due");

        group.MapGet("/{patientId}", MedicationsDueHandlers.GetByPatient)
            .WithName("GetMedicationsDueByPatient")
            .WithDescription("Retrieve all medications due for a patient")
            .Produces<List<MedicationDue>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{patientId}/now", MedicationsDueHandlers.GetDueNow)
            .WithName("GetMedicationsDueNow")
            .WithDescription("Retrieve medications due at the current hour for a patient")
            .Produces<List<MedicationDue>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/ward/{wardId}", MedicationsDueHandlers.GetByWard)
            .WithName("GetMedicationsDueByWard")
            .WithDescription("Retrieve all medications due across a ward")
            .Produces<List<MedicationDue>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
