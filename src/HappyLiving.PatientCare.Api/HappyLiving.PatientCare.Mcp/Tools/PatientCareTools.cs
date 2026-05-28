using System.ComponentModel;
using ModelContextProtocol.Server;

namespace HappyLiving.PatientCare.Mcp.Tools;

[McpServerToolType]
public class PatientCareTools
{
    private readonly HttpClient _http;

    public PatientCareTools(IHttpClientFactory httpClientFactory)
        => _http = httpClientFactory.CreateClient("PatientCareApi");

    [McpServerTool, Description("Get all medications due for a patient")]
    public Task<string> GetMedicationsDue([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/medications-due/{patientId}");

    [McpServerTool, Description("Get medications due at the current hour for a patient")]
    public Task<string> GetMedicationsDueNow([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/medications-due/{patientId}/now");

    [McpServerTool, Description("Get all medications due across a ward")]
    public Task<string> GetMedicationsDueByWard([Description("Ward ID (e.g. W001)")] string wardId)
        => _http.GetStringAsync($"/medications-due/ward/{wardId}");

    [McpServerTool, Description("Get the care plan for a patient")]
    public Task<string> GetCarePlan([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/careplans/{patientId}");

    [McpServerTool, Description("Get care plan goals for a patient")]
    public Task<string> GetCarePlanGoals([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/careplans/{patientId}/goals");

    [McpServerTool, Description("Get care plan interventions for a patient")]
    public Task<string> GetCarePlanInterventions([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/careplans/{patientId}/interventions");

    [McpServerTool, Description("Get all allergies for a patient")]
    public Task<string> GetAllergies([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/allergies/{patientId}");

    [McpServerTool, Description("Get only severe allergies for a patient")]
    public Task<string> GetSevereAllergies([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/allergies/{patientId}/severe");

    [McpServerTool, Description("Get all appointments for a patient")]
    public Task<string> GetAppointments([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/appointments/{patientId}");

    [McpServerTool, Description("Get upcoming appointments for a patient")]
    public Task<string> GetUpcomingAppointments([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/appointments/{patientId}/upcoming");

    [McpServerTool, Description("Get full medical history for a patient")]
    public Task<string> GetHistory([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/history/{patientId}");

    [McpServerTool, Description("Get recent medical history entries for a patient")]
    public Task<string> GetRecentHistory([Description("Patient ID (e.g. P001)")] string patientId)
        => _http.GetStringAsync($"/history/{patientId}/recent");
}
