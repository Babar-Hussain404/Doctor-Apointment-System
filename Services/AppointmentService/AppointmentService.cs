using DocApp.Models;
using DocApp.GenericRepository;

namespace DocApp.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _repository;

        public AppointmentService(IGenericRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _repository.GetAll();
        }

        public Appointment GetById(Guid Id)
        {
            return _repository.GetById(Id);
        }

        public void Add(Appointment appointment)
        {
            _repository.Add(appointment);
        }

        public void Update(Appointment appointment)
        {
            _repository.Update(appointment);
        }

        public void Delete(Guid Id)
        {
            _repository.Delete(Id);
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }
    }
}
