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
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var player = new List<PlayerEntity>();
			var faker = new Faker("en");
		
			for (int i = 0; i < 50; i++)
			{
				DateTime birthdate = faker.Date.Between(DateTime.Now.AddYears(-25), DateTime.Now.AddYears(-5));
				string ageCategory = _ageCategoryHelper.DetermineAgeCategory(birthdate);

                player.Add(new PlayerEntity
				{
					Id = i + 1,
					Name = faker.Name.FirstName(),
					Surname = faker.Name.LastName(),
					BornDate = birthdate,
                    Category = ageCategory,
                    Phone = faker.Phone.PhoneNumberFormat(),
					Email = faker.Internet.Email(),
					Note = faker.Lorem.Sentence(),
					Licance = faker.Random.Bool(),
					ParentContact = (faker.Name.FullName())+ (faker.Phone.PhoneNumberFormat()) + (faker.Address.StreetName()),
				});
			}

			modelBuilder.Entity<PlayerEntity>().HasData(player);
		}
	}
}
