using System;

namespace Pract3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Person> people = new List<Person>();
            people.Add(new Person("Aarav Sharma", 28, Gender.Male));
            people.Add(new Person("Priya Verma", 24, Gender.Female));
            people.Add(new Person("Rohan Iyer", 32, Gender.Male));

            foreach (Person person in people)
            {
                Console.WriteLine(person);
                person.changeAge(1);
            }

            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }

            Console.WriteLine();
            Rectangle rectangle = new Rectangle(new Point(0, 0), new Point(1, 1), 2, 3);
            Console.WriteLine(rectangle);
        }
    }
}
