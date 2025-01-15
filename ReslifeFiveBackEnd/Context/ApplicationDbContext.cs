using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReslifeFiveBackEnd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }
        public DbSet<Block> Block { get; set; }
        public DbSet<BlockTest> BlockTest { get; set; }
        public DbSet<BlockTestC> BlockTestC { get; set; }
        public DbSet<Slot> Slot { get; set; }
        public DbSet<TestUnit> TestUnit { get; set; }
        public DbSet<TimePeriodTest> TimePeriodTest { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
