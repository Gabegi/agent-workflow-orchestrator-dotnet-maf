using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Services;

public class JsonPatientCareDb : IPatientCareDb
{
    public IReadOnlyList<Patient> Patients { get; } = [];
    public IReadOnlyList<MedicationDue> MedicationsDue { get; } = [];
    public IReadOnlyList<CarePlan> CarePlans { get; } = [];
    public IReadOnlyList<Allergy> Allergies { get; } = [];
    public IReadOnlyList<Appointment> Appointments { get; } = [];
    public IReadOnlyList<HistoryEntry> History { get; } = [];
}
