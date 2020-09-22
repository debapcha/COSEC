using COSEC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace COSEC.Data
{
    public class CosecContext : DbContext
    {
        public CosecContext(DbContextOptions<CosecContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ApprovedUser> ApprovedUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<ApprovedUser>().ToTable("ApprovedUser");
        }
    }
}
