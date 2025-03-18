using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Utils
{
    public static class NumberUtils
    {
        public static int InputIntegerNumber(string message, int min, int max)
        {
            int number;

            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out number))
                {
                    if (number >= min && number <= max)
                    {
                        return number;
                    }
                    Console.WriteLine($"Error: Please enter a number between {min} and {max}!");
                }
                else
                {
                    Console.WriteLine("Error: Please enter a valid integer number!");
                }
            }
        }
    }
}
