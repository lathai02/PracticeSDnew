using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shares.Utils
{
    public static class StringUtils
    {
        public static string InputString(string message, string? pattern = null)
        {
            string? input;

            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Error: Please enter a non-empty string!");
                    continue;
                }

                if (!string.IsNullOrEmpty(pattern) && !Regex.IsMatch(input, pattern))
                {
                    Console.WriteLine("Error: Invalid format! Please try again.");
                    continue;
                }

                return input;
            }
        }

        public static void PrintList<T>(List<T> items, string title = "List Data")
        {
            Console.WriteLine($"===== {title} =====");

            if (items == null || items.Count == 0)
            {
                Console.WriteLine("List is empty.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("====================");
        }
    }
}
