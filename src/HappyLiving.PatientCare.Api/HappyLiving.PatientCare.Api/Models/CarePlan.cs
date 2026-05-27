namespace HappyLiving.PatientCare.Api.Models;

public record CarePlan(
    string Id,
    string PatientId,
    string CreatedDate,
    string ReviewDate,
    string AssignedNurse,
    List<Goal> Goals,
    List<Intervention> Interventions
);

public record Goal(
    string Id,
    string Description,
    string TargetDate,
    string Status
);

public record Intervention(
    string Id,
    string Description,
    string Frequency,
    string AssignedTo
);
