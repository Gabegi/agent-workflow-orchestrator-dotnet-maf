using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class HistoryEndpoints
{
    public static void MapHistory(this WebApplication app)
    {
        var group = app.MapGroup("/history")
            .WithTags("History");

        group.MapGet("/{patientId}", HistoryHandlers.GetByPatient)
            .WithName("GetHistoryByPatient")
            .WithDescription("Retrieve full medical history for a patient")
            .Produces<List<HistoryEntry>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{patientId}/recent", HistoryHandlers.GetRecent)
            .WithName("GetRecentHistory")
            .WithDescription("Retrieve medical history entries from the last 30 days")
            .Produces<List<HistoryEntry>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
