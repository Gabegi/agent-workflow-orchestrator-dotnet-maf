namespace HappyLiving.PatientCare.Api.Models;

public record Allergy(
    string Id,
    string PatientId,
    string Allergen,
    string Severity,
    string Reaction,
    string? Notes
);
