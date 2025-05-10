using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.Design;


/*
    Вам потрiбно створити консольну програму на C#, яка моделює систему унiверситету.
    Програма повинна працювати з двома колекцiями:
    • Students — список студентiв;
    • Courses — список курсiв.
    Клас Student:
    • int Id
    • string Name
    • int Age
    • int CourseId
    • double Grade
    Клас Course:
    • int Id
    • string Title
    • string Instructor
 */

namespace Pract7
{
    [Serializable]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int CourseId { get; set; }
        public double Grade { get; set; }

        public Student(int id, string name, int age, int courseId, double grade)
        {
            Id = id;
            Name = name;
            Age = age;
            CourseId = courseId;
            Grade = grade;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Age: {Age}, CourseId: {CourseId}, Grade: {Grade}";
        }
    }

    [Serializable]
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructor { get; set; }

        public Course(int id, string title, string instructor)
        {
            Id = id;
            Title = title;
            Instructor = instructor;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Instructor: {Instructor}";
        }
    }

    [Serializable]
    public class Students : List<Student>
    {
        public void AddStudent(Student student)
        {
            this.Add(student);
        }

        public void RemoveStudent(int id)
        {
            var student = this.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                this.Remove(student);
            }
        }
    }

    [Serializable]
    public class Courses : List<Course>
    {
        public void AddCourse(Course course)
        {
            this.Add(course);
        }

        public void RemoveCourse(int id)
        {
            var course = this.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                this.Remove(course);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(binPath, @"..\.."));
           
            string studentsFilePath = Path.Combine(projectRoot, "students.json");
            string coursesFilePath = Path.Combine(projectRoot, "courses.json");

            // Deserialize students from JSON file
            string loadedStudentsJson = File.ReadAllText(studentsFilePath);
            Students students = JsonSerializer.Deserialize<Students>(loadedStudentsJson);

            // Deserialize courses from JSON file
            string loadedCoursesJson = File.ReadAllText(coursesFilePath);
            Courses courses = JsonSerializer.Deserialize<Courses>(loadedCoursesJson);

            // Part 2 Task 1
            var grade_grater_80 = from student in students      // вибір студента 
                                  where student.Grade > 80      // обмеження по балах
                                  select student;
            Console.WriteLine("Students with grade greater than 80:");
            grade_grater_80.ToList().ForEach(s => Console.WriteLine(s.ToString()));
            Console.WriteLine("--------------------------\n");


            // Part 2 Task 2
            var sorted_b_grade_desc = from student in students          // вибір студента
                                      orderby student.Grade descending  // сортування по балах
                                      select student;
            Console.WriteLine("Students sorted by grade in descending order:");
            sorted_b_grade_desc.ToList().ForEach(s => Console.WriteLine(s.ToString()));
            Console.WriteLine("--------------------------\n");


            // Part 2 Task 3
            var programming_students = from student in students                                     // вибір студента
                                       join course in courses on student.CourseId equals course.Id  // об'єднання студента з курсом
                                       where course.Title == "Programming"                          // обмеження по назві курсу
                                       select student;
            Console.WriteLine("Students enrolled in Programming course:");
            programming_students.ToList().ForEach(s => Console.WriteLine(s.ToString()));
            Console.WriteLine("--------------------------\n");


            // Part 2 Task 4
            var grouped_by_age = from student in students                       // вибір студента
                                 group student by student.Age into ageGroup     // групування по віку
                                 select ageGroup;                               // беремо згруповані дані
            grouped_by_age.ToList().ForEach(g =>
            {
                Console.WriteLine($"Age group: {g.Key}");
                g.ToList().ForEach(s => Console.WriteLine(s.Name));
                Console.WriteLine();
            });
            Console.WriteLine("--------------------------\n");


            // Part 2 Task 5
            var joined = from student in students                                       // вибір студента
                         join course in courses on student.CourseId equals course.Id    // об'єднання студента з курсом
                         select new                                                     // створення нового об'єкту який буде поміщено в генератор
                         {
                             StudentName = student.Name,
                             CourseTitle = course.Title,
                             Instructor = course.Instructor
                         };
            Console.WriteLine("Joined data of students and courses:");
            joined.ToList().ForEach(j => Console.WriteLine($"Student: {j.StudentName}, Course: {j.CourseTitle}, Instructor: {j.Instructor}"));
            Console.WriteLine("--------------------------\n");


            // Part 2 Task 6
            var avg_grade_all = (from student in students
                                 select student.Grade).Average();                   // обчислення середнього бала через агрегативну функцію
            Console.WriteLine($"Average grade of all students: {avg_grade_all}");
            Console.WriteLine("--------------------------\n");

            // Part 2 Task 7
            var any_student_below_60 = students.Any(s => s.Grade < 60);             // перевірка наявності студента з балом менше 60
            Console.WriteLine($"Is there any student with grade below 60? {any_student_below_60}");
            Console.WriteLine("--------------------------\n");

            // Part 2 Task 8
            // Print course titile with max students enrolled
            var course_w_max_students = (from student in students                                       // вибір студента
                                         join course in courses on student.CourseId equals course.Id    // об'єднання студента з курсом
                                         group student by course into courseGroup                       // групування по курсу
                                         select new                                                     // створення нового об'єкту який буде поміщено в генератор
                                         {
                                             Course = courseGroup.Key,
                                             Title = courseGroup.Key.Title,
                                             StudentCount = courseGroup.Count()
                                         }).ToList();

            Console.WriteLine("Course with maximum students enrolled:");
            var maxCourse = course_w_max_students.Max(s => s.StudentCount);
            Console.WriteLine($"{course_w_max_students.FirstOrDefault(s => s.StudentCount == maxCourse).Title}, Students Enrolled: {maxCourse}");
            Console.WriteLine("--------------------------\n");

            // Part 2 Task 9
            var top_3_students = (from student in students           // вибір студента
                                  orderby student.Grade descending   // сортування по балах
                                  select student).Take(3);           // вибір трьох студентів з найвищими балами

            Console.WriteLine("Top 3 students by grade:");
            top_3_students.ToList().ForEach(s => Console.WriteLine($"Name: {s.Name}, Grade: {s.Grade}"));
            Console.WriteLine("--------------------------\n\n\n");


            // Part 3
            var new_tb = (from student in students                                      // вибір студента
                          join course in courses on student.CourseId equals course.Id   // об'єднання студента з його курсом
                          select new
                          {
                              StudentName = student.Name,
                              Course = course.Title,
                              Performance = student.Grade >= 90 ? "Exc" :               // перевірка на оцінку
                                            student.Grade >= 75 ? "Good" :              // перевірка на оцінку
                                            student.Grade >= 60 ? "Average" :           // перевірка на оцінку
                                                                  "Fail"                // перевірка на оцінку
                          }).ToList();
            Console.WriteLine("Student performance table:");
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("|\tName\t\t|\tCourse\t\t|\tPerformance\t|");
            Console.WriteLine("-------------------------------------------------------------------------");
            foreach (var item in new_tb)
            {
                Console.WriteLine($"|\t{item.StudentName}\t|\t{item.Course}\t|\t{item.Performance}\t\t|");
                Console.WriteLine("-------------------------------------------------------------------------");
            }
        }
    }
}
