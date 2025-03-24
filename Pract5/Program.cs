using System;

namespace Pract5
{
    internal class Program
    {
        public static double WeekEndPriceRaise(double raw_price)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return raw_price * 1.2;
            }
            else return raw_price;
        }

        static void Main(string[] args)
        {
            V1.Logger logger = new V1.Logger();

            V1.BoxOffice boxOffice = new V1.BoxOffice();
            V1.Session session = new V1.Session(1, "Matrix", DateTime.Parse("2025-03-21 14:30"));

            boxOffice.SetLogger(logger);
            session.SetLogger(logger);

            boxOffice.AddSession(session);
            boxOffice.GenerateTicketsForSession("Matrix", DateTime.Parse("2025-03-21 14:30"), 10, 3.2, WeekEndPriceRaise);

            boxOffice.SellTicketFor("Matrix", DateTime.Parse("2025-03-21 14:30"));
            boxOffice.ReserveTicketFor("Matrix", DateTime.Parse("2025-03-21 14:30"));

        }
    }
}
