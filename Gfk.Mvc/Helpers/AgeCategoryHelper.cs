using System;
namespace Gfk.Mvc.Helpers
{
	public class AgeCategoryHelper
	{
		public string DetermineAgeCategory( DateTime birthdate)
		{
			DateTime now = DateTime.UtcNow;
			int age = now.Year - birthdate.Year;

			string ageCategory;

			if( age >= 18 )
			{
				return ageCategory = "A Takım";
			}

			if( age < 18 && age == 17 )
			{
				return ageCategory = "U-18";
			}

            if ( age < 17 && age == 16 )
            {
				return ageCategory = "U-17";
            }

            if ( age < 16 && age ==15 )
            {
				return ageCategory = "U-16";
            }

            if ( age < 15 && age == 14 )
            {
				return ageCategory = "U-15";
            }

            if ( age < 14 && age == 13 )
            {
				return ageCategory = "U-14";
            }

			if ( age < 13 && age == 12)
			{
				return ageCategory = "U-13";
			}

			if ( age < 12 && age == 11)
			{
				return ageCategory = "U-12";
			}

			if ( age < 11 && age == 10)
			{
				return ageCategory = "U-11";
			}

			return ageCategory = "Minik";
        }
	}
}

