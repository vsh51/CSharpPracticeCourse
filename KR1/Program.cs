using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR1
{
    /*
        Library
     */
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public UInt64 ISBN { get; set; }
        public bool Availability { get; set; }
        public UInt32 IsBeingUsedFor { get; set; }

        public Book(string title, string author, int year, UInt64 isbn, bool availability)
        {
            Title = title;
            Author = author;
            Year = year;
            ISBN = isbn;
            Availability = availability;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, Year: {Year}, ISBN: {ISBN}, Availability: {Availability}";
        }
    }

    public class User
    {
        public UInt32 ID { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public User(UInt32 id, string name)
        {
            ID = id;
            Name = name;
            Books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            Books.Remove(book);
        }

        public Book FindBook(string title)
        {
            foreach (Book book in Books)
            {
                if (book.Title == title)
                {
                    return book;
                }
            }
            return null;
        }
    }

    public class Library
    {
        public delegate void GiveBook(Book book, User user);
        public delegate void ReturnBook(Book book, User user);

        // Delegate for fine calculation based on days
        public delegate double FineCalculation(UInt32 days);

        public Action<User, Book, UInt32, double> BookOverdue;

        public static double DefaultFineCalculation(UInt32 days)
        {
            if (days <= 10)
            {
                return 0;
            }
            else
            {
                return 0.5 * days;
            }
        }

        public event GiveBook GiveBookEvent;
        public event ReturnBook ReturnBookEvent;

        public List<Book> Books { get; set; }
        public Library()
        {
            Books = new List<Book>();
        }
        public void AddBook(Book book)
        {
            Books.Add(book);
        }
        public void RemoveBook(Book book)
        {
            Books.Remove(book);
        }

        public Book FindBook(string title)
        {
            foreach (Book book in Books)
            {
                if (book.Title == title)
                {
                    return book;
                }
            }
            return null;
        }

        public void GiveBookToUser(Book book, User user)
        {
            if (book.Availability)
            {
                book.Availability = false;
                user.AddBook(book);
                GiveBookEvent?.Invoke(book, user);
            }
        }

        public void ReturnBookFromUser(Book book, User user, FineCalculation calculator)
        {
            if (!book.Availability)
            {
                book.Availability = true;
                user.RemoveBook(book);
                ReturnBookEvent?.Invoke(book, user);

                // Calculate fine
                double fine = calculator(book.IsBeingUsedFor);
                if (fine > 0)
                {
                    BookOverdue?.Invoke(user, book, book.IsBeingUsedFor, fine);
                }
            }
        }

        public void SortBooks(Func<Book, Book, int> sort)
        {
            Books.Sort((book1, book2) => sort(book1, book2));
        }

        public void NotifyBookGiven(GiveBook giveBook)
        {
            GiveBookEvent += giveBook;
        }

        public void NotifyBookReturned(ReturnBook returnBook)
        {
            ReturnBookEvent += returnBook;
        }

        public void NotifyBookOverdue(Action<User, Book, UInt32, double> bookOverdue)
        {
            BookOverdue += bookOverdue;
        }

        public void CleanNotifications()
        {
            GiveBookEvent = null;
            ReturnBookEvent = null;
            BookOverdue = null;
        }

        public override string ToString()
        {
            string result = "";
            foreach (Book book in Books)
            {
                result += book + "\n";
            }
            return result;
        }
    }

    class Logger
    {
        public void LogGiveBook(Book book, User user)
        {
            Console.WriteLine($"Book {book.Title} given to user {user.Name}");
        }
        public void LogReturnBook(Book book, User user)
        {
            Console.WriteLine($"Book {book.Title} returned from user {user.Name}");
        }
        public void LogOverdue(User user, Book book, UInt32 days, double fine)
        {
            Console.WriteLine($"Book {book.Title} is overdue for {days} days. Fine: {fine}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Book houndOfTheBaskervilles = new Book("Hound of the Baskervilles",
                "Arthur Conan Doyle", 1902, 9780141395486, true);
            Book sherlockHolmes = new Book("Sherlock Holmes",
                "Arthur Conan Doyle", 1892, 9780141395487, true);
            Book codeDaVinci = new Book("Code Da Vinci",
                "Dan Brown", 2003, 9780141395488, true);
            Book inferno = new Book("Inferno",
                "Dan Brown", 2013, 9780141395489, true);
            Book angelsAndDemons = new Book("Angels and Demons",
                "Dan Brown", 2000, 9780141395490, true);

            User user1 = new User(1, "John Doe");
            User user2 = new User(2, "Jane Doe");

            Library library = new Library();
            library.AddBook(houndOfTheBaskervilles);
            library.AddBook(sherlockHolmes);
            library.AddBook(codeDaVinci);
            library.AddBook(inferno);
            library.AddBook(angelsAndDemons);

            Logger logger = new Logger();
            library.NotifyBookGiven(logger.LogGiveBook);
            library.NotifyBookReturned(logger.LogReturnBook);
            library.NotifyBookOverdue(logger.LogOverdue);

            library.GiveBookToUser(houndOfTheBaskervilles, user1);
            library.GiveBookToUser(sherlockHolmes, user1);
            library.GiveBookToUser(codeDaVinci, user2);
            library.GiveBookToUser(inferno, user2);
            library.GiveBookToUser(angelsAndDemons, user2);

            Random random = new Random();

            foreach (Book book in library.Books)
            {
                book.IsBeingUsedFor = (uint)random.Next(8, 20);
            }

            library.ReturnBookFromUser(houndOfTheBaskervilles, user1, Library.DefaultFineCalculation);
            library.ReturnBookFromUser(sherlockHolmes, user1, Library.DefaultFineCalculation);
            library.ReturnBookFromUser(codeDaVinci, user2, Library.DefaultFineCalculation);
            library.ReturnBookFromUser(inferno, user2, Library.DefaultFineCalculation);
            library.ReturnBookFromUser(angelsAndDemons, user2, Library.DefaultFineCalculation);

            Console.WriteLine();
            Console.WriteLine("Sorted by year:");
            library.SortBooks((book1, book2) => book1.Year - book2.Year);
            Console.WriteLine(library.ToString());

            Console.WriteLine("Sorted by title:");
            library.SortBooks((book1, book2) => book1.Title.CompareTo(book2.Title));
            Console.WriteLine(library.ToString());

            library.CleanNotifications();
        }
    }
}
