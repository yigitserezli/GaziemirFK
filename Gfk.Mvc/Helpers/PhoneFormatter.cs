using System;
namespace Gfk.Mvc.Helpers
{
	public class PhoneFormatter
	{
		public static string FormatPhone(string rawPhone)
		{
			string digits = new string(rawPhone.Where(char.IsDigit).ToArray());

			if(digits.Length == 10)
			{
				return $"+90 ({digits.Substring(0, 3)}) {digits.Substring(3, 3)} {digits.Substring(6)}";
			}
			else
			{
				return rawPhone;
			}
		}
	}
}

