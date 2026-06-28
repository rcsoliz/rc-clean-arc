using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseNpgsql("Host=monorail.proxy.rlwy.net;Port=32735;Database=railway;Username=postgres;Password=HgJtquiGdmuTnwxmaBpRefhuIVCEllWb;SSL Mode=Require;Trust Server Certificate=true");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
