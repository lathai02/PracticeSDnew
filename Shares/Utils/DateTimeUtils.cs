using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime InputDateTime(string message)
        {
            DateTime date;
            while (true)
            {
                Console.Write(message);
                if (!DateTime.TryParse(Console.ReadLine(), out date))
                {
                    Console.WriteLine("Error: Invalid date format! Please try again.");
                    continue;
                }
                return date;
            }
        }

        public static DateOnly InputDateOnly(string message, string formart)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (DateOnly.TryParseExact(input, formart, null, System.Globalization.DateTimeStyles.None, out DateOnly date))
                {
                    return date;
                }
                Console.WriteLine("Error: Invalid date format! Please try again.");
            }
        }
    }
}
