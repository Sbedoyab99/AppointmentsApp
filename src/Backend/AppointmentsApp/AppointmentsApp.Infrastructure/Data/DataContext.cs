using Microsoft.EntityFrameworkCore;

namespace AppointmentsApp.Infrastructure.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {

    }
}
