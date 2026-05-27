using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Services;

public interface IPatientCareDb
{
    IReadOnlyList<Patient> Patients { get; }
    IReadOnlyList<MedicationDue> MedicationsDue { get; }
    IReadOnlyList<CarePlan> CarePlans { get; }
    IReadOnlyList<Allergy> Allergies { get; }
    IReadOnlyList<Appointment> Appointments { get; }
    IReadOnlyList<HistoryEntry> History { get; }
}
