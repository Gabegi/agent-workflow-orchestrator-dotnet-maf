namespace HappyLiving.PatientCare.Api.Models;

public record Patient(
    string Id,
    string FullName,
    string DateOfBirth,
    string Gender,
    string WardId,
    string RoomNumber,
    string AdmissionDate,
    string PrimaryNurse,
    string PrimaryDoctor
);
