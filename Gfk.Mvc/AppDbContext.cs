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
    }
}
