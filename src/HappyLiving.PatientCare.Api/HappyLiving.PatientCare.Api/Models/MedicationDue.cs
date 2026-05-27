namespace HappyLiving.PatientCare.Api.Models;

public record MedicationDue(
    string Id,
    string PatientId,
    string MedicationName,
    string Dose,
    string Route,
    string ScheduledTime,
    string Status,
    string PrescribedBy,
    string? Notes
);
