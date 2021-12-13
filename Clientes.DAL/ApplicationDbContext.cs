using Clientes.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clientes.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<Value> Values { get; set; }
    }
}