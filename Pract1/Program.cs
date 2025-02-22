using System;

namespace Pract1
{
    internal class Program
    {

        // Newton's numerical method
        static double SquareRoot(double number, double precision = 1e-6)
        {
            if (number < 0)
                return -1;

            double approximate_value = number;
            while (Math.Abs(approximate_value * approximate_value - number) > precision)
            {
                approximate_value = (approximate_value + number / approximate_value) / 2;
            }
            return approximate_value;
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Type a positive number: ");
            double number = double.Parse(Console.ReadLine());
            Console.WriteLine(string.Format("Square root of {0} is {1}.", number, SquareRoot(number)));
        }
    }
}
