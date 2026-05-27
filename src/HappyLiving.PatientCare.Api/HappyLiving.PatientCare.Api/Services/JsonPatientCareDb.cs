using System.Text.Json;
using System.Text.Json.Serialization;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Services;

public class JsonPatientCareDb : IPatientCareDb
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public IReadOnlyList<Patient> Patients { get; }
    public IReadOnlyList<MedicationDue> MedicationsDue { get; }
    public IReadOnlyList<CarePlan> CarePlans { get; }
    public IReadOnlyList<Allergy> Allergies { get; }
    public IReadOnlyList<Appointment> Appointments { get; }
    public IReadOnlyList<HistoryEntry> History { get; }

    public JsonPatientCareDb()
    {
        Patients = Load<Patient>("patients.json");
        MedicationsDue = Load<MedicationDue>("medications-due.json");
        CarePlans = Load<CarePlan>("careplans.json");
        Allergies = Load<Allergy>("allergies.json");
        Appointments = Load<Appointment>("appointments.json");
        History = Load<HistoryEntry>("history.json");
    }

    private static IReadOnlyList<T> Load<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "data", fileName);
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? [];
    }
}
