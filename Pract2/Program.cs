using System;

namespace Pract2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Type surname: ");
            string surname = Console.ReadLine();
            Console.WriteLine("Type mail: ");
            string mail = Console.ReadLine();

            Client cl1 = new Client(name, surname, mail);

            Console.WriteLine(cl1);
        }
    }
}
