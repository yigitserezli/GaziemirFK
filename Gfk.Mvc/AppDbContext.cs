using Bogus;
using Gfk.Mvc.Helpers;
using Gfk.Mvc.Models;
using Gfk.Mvc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gfk.Mvc
{
    public class AppDbContext : DbContext
    {
        private readonly AgeCategoryHelper _ageCategoryHelper;

        public AppDbContext(DbContextOptions<AppDbContext> options, AgeCategoryHelper ageCategoryHelper) : base(options)
        {
            _ageCategoryHelper = ageCategoryHelper;
        }

        public DbSet<PlayerEntity> Players { get; set; } = null!;
        public DbSet<ParentEntity> Parents { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<PaymentEntity> Payments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminUser = new UserEntity
            {
                Id = 999,
                Name = "Ercan",
                Surname = "Serezli",
                Email = "ercanserezli21@icloud.com",
                Password = HashHelper.HashToString("ercan123"),
                ActivationCode = "0",
                KVKK = true,
                Phone = PhoneFormatter.FormatPhone("5059581824"),
                Role = "Admin"
            };
            modelBuilder.Entity<UserEntity>().HasData(adminUser);
        }
    }
}
