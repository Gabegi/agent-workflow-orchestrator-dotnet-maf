using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class WardEndpoints
{
    public static void MapWards(this WebApplication app)
    {
        var group = app.MapGroup("/wards")
            .WithTags("Wards");

        group.MapGet("/{id}/patients", WardHandlers.GetPatients)
            .WithName("GetWardPatients")
            .WithDescription("Retrieve all patients currently in a ward")
            .Produces<List<Patient>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}/medications-due", WardHandlers.GetMedicationsDue)
            .WithName("GetWardMedicationsDue")
            .WithDescription("Retrieve medications due at a specific time for a ward")
            .Produces<List<Medication>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
