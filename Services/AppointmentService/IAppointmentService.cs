using DocApp.Models;

namespace DocApp.Services.AppointmentService
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAll();
        Appointment GetById(Guid Id);
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Guid Id);
        void SaveChanges();
    }
}
