using DocApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DocApp.Data
{
    public class DocAppContext : DbContext
    {
        public DocAppContext(DbContextOptions<DocAppContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Appointment> Appointments { get; set; }
    }
}