namespace HappyLiving.PatientCare.Api.Models;

public record Appointment(
    string Id,
    string PatientId,
    string Type,
    string DateTime,
    string Provider,
    string Location,
    string Status
);
