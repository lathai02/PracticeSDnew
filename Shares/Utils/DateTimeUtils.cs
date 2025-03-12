using Shares.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime InputDateTime(string message, string format = AppConstants.DateFormat)
        {
            DateTime date;

            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (!DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    Console.WriteLine($"Error: Invalid date format! Please enter the date in '{format}' format.");
                    continue;
                }

                return date;
            }
        }
    }
}
