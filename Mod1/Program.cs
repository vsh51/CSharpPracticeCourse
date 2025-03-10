using System;
using System.Collections.Generic;

/*
    Був на парі
 */

namespace Mod1
{
    class Program
    {
        public interface IEmployee
        {
            string Name { get; set; }
            string Position { get; set; }
            decimal Salary { get; set; }

            void Work();
            void GetInfo();
        }

        public class Manager : IEmployee
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public decimal Salary { get; set; }

            public int Subordinates { get; set; }

            private List<IEmployee> SubordinatesList;

            public void Subordinate(IEmployee employee)
            {
                SubordinatesList.Add(employee);
            }

            public string SubodinatesToString()
            {
                string subordinates = "";
                foreach (IEmployee employee in SubordinatesList)
                {
                    subordinates += employee.Name + ", ";
                }
                return subordinates;
            }

            public void RemoveSubordinatesWithName(string name)
            {
                foreach (IEmployee employee in SubordinatesList)
                {
                    if (employee.Name == name)
                    {
                        SubordinatesList.Remove(employee);
                    }
                }
            }

            public Manager(string name, string position, decimal salary)
            {
                SubordinatesList = new List<IEmployee>();
                Name = name;
                Position = position;
                Salary = salary;
            }
            public void Work()
            {
                Console.WriteLine("Manager is working...");
            }
            public void GetInfo()
            {
                Console.WriteLine($"Name: {Name}," +
                    $"Position: {Position}," +
                    $"Salary: {Salary}," +
                    $"Employees: {SubodinatesToString()}"
                );
            }
        }

        public class Developer : IEmployee
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public decimal Salary { get; set; }

            private List<string> ProgrammingLanguages;
            public Developer(string name, string position, decimal salary)
            {
                ProgrammingLanguages = new List<string>();
                Name = name;
                Position = position;
                Salary = salary;
            }
            public void Work()
            {
                Console.WriteLine("Developer is working...");
            }
            public void GetInfo()
            {
                Console.WriteLine($"Name: {Name}," +
                    $"Position: {Position}," +
                    $"Salary: {Salary}," +
                    $"Programming Languages: {ProgrammingLanguagesToString()}"
                );
            }

            public void AddProgrammingLanguage(string language)
            {
                ProgrammingLanguages.Add(language);
            }

            public string ProgrammingLanguagesToString()
            {
                string languages = "";
                foreach (string language in ProgrammingLanguages)
                {
                    languages += language + ", ";
                }
                return languages;
            }

            public void RemoveProgrammingLanguage(string language)
            {
                ProgrammingLanguages.Remove(language);
            }
        }

        public class Designer : IEmployee
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public decimal Salary { get; set; }

            public string DesignType { get; set; }

            public Designer(string name, string position, decimal salary)
            {
                Name = name;
                Position = position;
                Salary = salary;
            }

            public void Work()
            {
                Console.WriteLine("Designer is working...");
            }

            public void GetInfo()
            {
                Console.WriteLine($"Name: {Name}," +
                    $"Position: {Position}," +
                    $"Salary: {Salary}," +
                    $"Design Type: {DesignType}"
                );
            }
        }
        static void Main(string[] args)
        {
            List<IEmployee> employees = new List<IEmployee>();

            Manager manager = new Manager("John", "Senior Manager", 1000);
            IEmployee developer = new Developer("Alex", "Middle Developer", 500);
            IEmployee designer = new Designer("Anna", "Middle Designer", 700);

            manager.Subordinate(developer);
            manager.Subordinate(designer);
            manager.Subordinate(developer);
            manager.Subordinate(new Manager("Mike", "Junior Manager", 100));

            employees.Add(manager);
            employees.Add(developer);
            employees.Add(designer);

            foreach (IEmployee employee in employees)
            {
                employee.GetInfo();
                employee.Work();
            }
        }
    }
}