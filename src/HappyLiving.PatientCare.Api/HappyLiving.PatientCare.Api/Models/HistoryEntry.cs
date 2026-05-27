namespace HappyLiving.PatientCare.Api.Models;

public record HistoryEntry(
    string Id,
    string PatientId,
    string Date,
    string Type,
    string Description,
    string Provider
);
