using System;

namespace HT1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the string: ");
            string user_input = Convert.ToString(Console.ReadLine());

            for (int i = 0; i < Convert.ToUInt32(user_input.Length / 2); ++i)
            {
                if (user_input[i] != user_input[user_input.Length - 1 - i])
                {
                    Console.WriteLine("String is not a palindrmoe");
                    return;
                }
            };

            Console.WriteLine("String is a palindrome");
        }
    }
}
