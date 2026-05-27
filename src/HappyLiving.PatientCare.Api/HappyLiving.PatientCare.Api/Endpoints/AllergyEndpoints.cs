using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class AllergyEndpoints
{
    public static void MapAllergies(this WebApplication app)
    {
        var group = app.MapGroup("/allergies")
            .WithTags("Allergies");

        group.MapGet("/{patientId}", AllergyHandlers.GetByPatient)
            .WithName("GetAllergiesByPatient")
            .WithDescription("Retrieve all allergies for a patient")
            .Produces<List<Allergy>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{patientId}/severe", AllergyHandlers.GetSevere)
            .WithName("GetSevereAllergies")
            .WithDescription("Retrieve only severe allergies for a patient")
            .Produces<List<Allergy>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
