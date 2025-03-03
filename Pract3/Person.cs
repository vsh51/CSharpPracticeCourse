using System;

namespace Pract3
{
    enum Gender {Male = 1, Female};
    class Person
    {

        public Person(string name, int age, Gender gender)
        {
            Name = name;
            Age = age;
            Gender = gender;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public void changeAge(int newAge)
        {
            this.Age = newAge;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}) is a {2}.)", Name, Age, this.Gender.ToString().ToLower());
        }
    }
}
