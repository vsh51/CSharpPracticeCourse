using System;
using System.Collections.Generic;
using System.Linq;

namespace Pract5
{
    internal class V1
    {
        public class Ticket
        {
            public UInt32 Id { get; }
            public double Price { get; }

            public enum TicketState
            {
                Available,
                Sold,
                Reserved
            }

            public TicketState State { get; set; } = TicketState.Available;

            public Ticket(UInt32 id, double price)
            {
                Id = id;
                Price = price;
            }
        }

        public class Session
        {
            public UInt32 Id { get; }
            public string Movie { get; set; }
            public DateTime Time { get; set; }

            private Logger _logger;
            public void SetLogger(Logger logger)
            {
                _logger = logger;
                _logger.SubToAvailableTicketsCountChanged(this);
                _logger.SubToReservedTicketsCountChanged(this);
                _logger.SubToSoldTicketsCountChanged(this);
            }

            private UInt32 _availableTicketsCount;
            private UInt32 _reservedTicketsCount;
            private UInt32 _soldTicketsCount;

            public EventHandler<UInt32> AvailableTicketsCountChanged;
            public EventHandler<UInt32> ReservedTicketsCountChanged;
            public EventHandler<UInt32> SoldTicketsCountChanged;

            public UInt32 AvailableTicketsCount {
                get => _availableTicketsCount;
                set {
                    _availableTicketsCount = value;
                    AvailableTicketsCountChanged?.Invoke(this, value);
                }
            }

            public UInt32 ReservedTicketsCount {
                get => _reservedTicketsCount;
                set
                {
                    _reservedTicketsCount = value;
                    ReservedTicketsCountChanged?.Invoke(this, value);
                }
            }

            public UInt32 SoldTicketsCount {
                get => _soldTicketsCount;
                set
                {
                    _soldTicketsCount = value;
                    SoldTicketsCountChanged?.Invoke(this, value);
                }
            }

            public Session(UInt32 id, string movie, DateTime time)
            {
                Id = id;
                Movie = movie;
                Time = time;
                AvailableTicketsCount = 0;
                ReservedTicketsCount = 0;
                SoldTicketsCount = 0;
            }
        }
        
        public class BoxOffice
        {
            public Dictionary<Session, List<Ticket>> Sessions { get; }

            private Logger _logger;
            public void SetLogger(Logger logger)
            {
                _logger = logger;
                _logger.SubToTicketSold(this);
                _logger.SubToTicketReserved(this);
            }

            public BoxOffice()
            {
                Sessions = new Dictionary<Session, List<Ticket>>();
            }

            public void AddSession(Session session)
            {
                Sessions.Add(session, new List<Ticket>());
            }

            public delegate double priceCalculation(double price);

            public void GenerateTicketsForSession(string movie, DateTime time, UInt32 count, double raw_price, priceCalculation calculator)
            {
                Session session = FindSession(movie, time);
                session.AvailableTicketsCount = count;

                for (UInt32 i = 0; i < count; i++)
                {
                    Sessions[session].Add(new Ticket(i, calculator(raw_price)));
                }
            }

            public Action<Ticket> OnTicketSold;
            public void SellTicketFor(string movie, DateTime time)
            {
                Session session = FindSession(movie, time);
                Ticket ticket = FindAvailableTicketFor(session);

                ticket.State = Ticket.TicketState.Sold;
                session.SoldTicketsCount++;
                session.AvailableTicketsCount--;
                OnTicketSold?.Invoke(ticket);
            }

            public Action<Ticket> OnTicketReserved;
            public void ReserveTicketFor(string movie, DateTime time)
            {
                Session session = FindSession(movie, time);
                Ticket ticket = FindAvailableTicketFor(session);

                ticket.State = Ticket.TicketState.Reserved;
                session.ReservedTicketsCount++;
                session.AvailableTicketsCount--;
                OnTicketReserved?.Invoke(ticket);
            }

            private Session FindSession(string movie, DateTime time)
            {
                Session session = Sessions.Keys.FirstOrDefault(s => s.Movie == movie && s.Time == time);
                return session ?? throw new Exception("Session not found");
            }

            private Ticket FindAvailableTicketFor(Session session)
            {
                Ticket ticket = Sessions[session].FirstOrDefault(t => t.State == Ticket.TicketState.Available);
                return ticket ?? throw new Exception("Ticket not found");
            }
        }

        public class Logger
        {
            public void SubToAvailableTicketsCountChanged(Session session)
            {
                session.AvailableTicketsCountChanged += (sender, count) =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Session {session.Id}: available tickets count changed to {count}");
                };
            }

            public void SubToReservedTicketsCountChanged(Session session)
            {
                session.ReservedTicketsCountChanged += (sender, count) =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Session {session.Id}: reserved tickets count changed to {count}");
                };
            }

            public void SubToSoldTicketsCountChanged(Session session)
            {
                session.SoldTicketsCountChanged += (sender, count) =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Session {session.Id}: sold tickets count changed to {count}");
                };
            }

            public void SubToTicketSold(BoxOffice boxOffice)
            {
                boxOffice.OnTicketSold += ticket =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Ticket {ticket.Id}: sold for {ticket.Price}$");
                };
            }

            public void SubToTicketReserved(BoxOffice boxOffice)
            {
                boxOffice.OnTicketReserved += ticket =>
                {
                    Console.WriteLine($"[{DateTime.Now}] Ticket {ticket.Id}: reserved for {ticket.Price}$");
                };
            }
        }
    }
}
