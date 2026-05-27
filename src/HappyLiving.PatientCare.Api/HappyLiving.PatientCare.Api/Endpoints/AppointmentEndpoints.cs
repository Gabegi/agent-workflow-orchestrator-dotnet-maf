using HappyLiving.PatientCare.Api.Application;
using HappyLiving.PatientCare.Api.Models;

namespace HappyLiving.PatientCare.Api.Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointments(this WebApplication app)
    {
        var group = app.MapGroup("/appointments")
            .WithTags("Appointments");

        group.MapGet("/{patientId}", AppointmentHandlers.GetByPatient)
            .WithName("GetAppointmentsByPatient")
            .WithDescription("Retrieve all appointments for a patient")
            .Produces<List<Appointment>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{patientId}/upcoming", AppointmentHandlers.GetUpcoming)
            .WithName("GetUpcomingAppointments")
            .WithDescription("Retrieve upcoming appointments for a patient")
            .Produces<List<Appointment>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
