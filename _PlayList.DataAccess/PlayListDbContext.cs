using Microsoft.EntityFrameworkCore;
using PlayList.Models;

namespace PlayList.DataAccess
{
    public class PlayListDbContext : DbContext
    {
        public PlayListDbContext(DbContextOptions<PlayListDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<PlayInfo> PlayInfos { get; set; }
    }
}