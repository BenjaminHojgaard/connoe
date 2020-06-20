using ConnOe.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConnOe.Data
{
    public class ConnoeDbContext : IdentityDbContext
    {
        public ConnoeDbContext() { }

        public ConnoeDbContext(DbContextOptions options) : base(options) { }
        
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }




    }
}