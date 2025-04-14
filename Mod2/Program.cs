using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod2
{
    /*
    Створення простого застосунку для керування замовленнями
    Ваша програма повинна містити наступні елементи:
    Створення інтерфейсу IOrder, який містить методи для додавання товарів, видалення товарів та отримання загальної вартості замовлення.
    Створення класу Order, який реалізує інтерфейс IOrder та містить методи для роботи з замовленнями.
    Побудова ієрархії класів для товарів: базовий клас Product, який містить загальні властивості, та похідні класи, наприклад, FoodProduct, ElectronicProduct тощо.
    Використання конструкторів для ініціалізації об'єктів класів та деструкторів для звільнення ресурсів.
    Визначення події для сповіщення про зміну статусу замовлення та організація взаємодії об'єктів через цю подію.
    Реалізація узагальненого класу для зберігання списку товарів у замовленні.
    Створення класів винятків для обробки помилок під час роботи з замовленнями.
    */

    public class OrderException : Exception
    {
        public OrderException(string message) : base(message) {}
    }

    public interface IMarketItem
    {
        string Name { get; set; }
        string Info();
        string ToString();
    }

    public abstract class Product : IMarketItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public IMarketItem Category { get; set; }
        public abstract string Info();
    }

    public class ElectnicsCategory : Product, IMarketItem
    {
        public ElectnicsCategory(string name)
        {
            Name = name;
        }

        public override string Info()
        {
            return $"Electronics Category: {Name}";
        }

        public override string ToString()
        {
            return $"Electronics Category: {Name}, Price: {Price}, IsAvailable: {IsAvailable}";
        }

        ~ElectnicsCategory()
        {
            Console.WriteLine($"Electronics Category {Name} is being destroyed.");
        }
    }

    public class ClothingCategory : Product, IMarketItem
    {
        public string Brand { get; set; }
        public string Size { get; set; }

        public ClothingCategory(string name, string brand, string size)
        {
            Name = name;
            Brand = brand;
            Size = size;
        }

        public override string Info()
        {
            return $"Clothing Category: {Name}, Brand: {Brand}, Size: {Size}";
        }

        public override string ToString()
        {
            return $"Clothing Category: {Name}, Brand: {Brand}, Size: {Size}, Price: {Price}, IsAvailable: {IsAvailable}";
        }
    }

    public class BooksCategory : Product, IMarketItem
    {
        public string Author { get; set; }
        public string Genre { get; set; }
        public UInt32 ISBN { get; set; }

        public BooksCategory(string name, string author, string genre)
        {
            Name = name;
            Author = author;
            Genre = genre;
        }

        public override string Info()
        {
            return $"Books Category: {Name}, Author: {Author}, Genre: {Genre}, ISBN: {ISBN}";
        }

        public override string ToString()
        {
            return $"Books Category: {Name}, Author: {Author}, Genre: {Genre}, ISBN: {ISBN}, Price: {Price}, IsAvailable: {IsAvailable}";
        }

        ~BooksCategory()
        {
                Console.WriteLine($"Books Category {Name} is being destroyed.");
        }
    }

    public class FoodCategory : Product, IMarketItem
    {
        public string ExpirationDate { get; set; }
        public string Ingredients { get; set; }

        public FoodCategory(string name, string expirationDate, string ingredients)
        {
            Name = name;
            ExpirationDate = expirationDate;
            Ingredients = ingredients;
        }

        public override string Info()
        {
            return $"Food Category: {Name}, Expiration Date: {ExpirationDate}, Ingredients: {Ingredients}";
        }

        public override string ToString()
        {
            return $"Food Category: {Name}, Expiration Date: {ExpirationDate}, Ingredients: {Ingredients}, Price: {Price}, IsAvailable: {IsAvailable}";
        }

        ~FoodCategory()
        {
            Console.WriteLine($"Food Category {Name} is being destroyed.");
        }
    }

    public interface IOrder
    {
        void AddProduct(Product product);
        void RemoveProduct(Product product);
        double CalculateTotalPrice();
        void Checkout();
    }

    public class SpecificProducts<T> where T : Product
        {
        private List<T> _products;

        public SpecificProducts()
        {
            _products = new List<T>();
        }

        public void AddProduct(T product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(T product)
        {
            _products.Remove(product);
        }

        public double CalculateTotalPrice()
        {
            return _products.Sum(p => p.Price);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _products.GetEnumerator();
        }

        public void Clear()
        {
            _products.Clear();
        }
    }

    public class Order : IOrder
    {
        private SpecificProducts<Product> _products;

            public event EventHandler OrderStatusChanged;

        public Order()
        {
            _products = new SpecificProducts<Product>();
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new OrderException("Product cannot be null.");
            }
            _products.AddProduct(product);
        }

        public void RemoveProduct(Product product)
        {
            _products.RemoveProduct(product);
        }

        public double CalculateTotalPrice()
        {
            return _products.CalculateTotalPrice();
            }

        public void NotifyOrderStatusChanged()
        {
            OrderStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        public SpecificProducts<Product> GetProducts()
        {
            return _products;
        }

        public void Checkout()
        {
            Console.WriteLine("Checkout. Total price: " + CalculateTotalPrice() + "\n");
            NotifyOrderStatusChanged();
            _products.Clear();
        }

        ~Order()
        {
            Console.WriteLine("Order is being destroyed.");
        }
    }

    public class Marketplace
    {
        private List<Product> _products;
        private List<Order> _orders;

        public Marketplace()
        {
            _products = new List<Product>();
            _orders = new List<Order>();
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            _products.Remove(product);
        }

        public List<Product> GetAvailableProducts()
        {
            return _products.Where(p => p.IsAvailable).ToList();
        }

        public List<Product> GetProductsByCategory(IMarketItem category)
        {
            return _products.Where(p => p.Category.Name == category.Name).ToList();
        }

        public List<Product> GetProductsByPriceRange(double minPrice, double maxPrice)
        {
            return _products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
        }

        private void SubscribeToOrderStatusChanged(Order order)
        {
            order.OrderStatusChanged += (sender, e) =>
            {
                foreach (Product prod in order.GetProducts())
                {
                    prod.IsAvailable = false;
                }
                _products.RemoveAll(p => !p.IsAvailable);
            };
        }

        public Order CreateOrder()
        {
            Order order = new Order();
            _orders.Add(order);
            SubscribeToOrderStatusChanged(order);
            return order;
        }

        public bool CheckOutOrder(Order order)
        {
            if (_orders.Contains(order))
            {
                order.Checkout();
                _orders.Remove(order);
                return true;
            }
            return false;
        }

        public void PrintProducts()
        {
            Console.WriteLine("Products:");
            foreach (Product product in _products)
            {
                Console.WriteLine(product?.ToString() );
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Marketplace marketplace = new Marketplace();

            Product electronics = new ElectnicsCategory("Laptop");
            electronics.Price = 1000;
            electronics.IsAvailable = true;
            marketplace.AddProduct(electronics);
            
            Product clothing = new ClothingCategory("T-Shirt", "Nike", "M");
            clothing.Price = 20;
            clothing.IsAvailable = true;
            marketplace.AddProduct(clothing);
            
            Product book = new BooksCategory("C# Programming", "John Doe", "Programming");
            book.Price = 30;
            book.IsAvailable = true;
            marketplace.AddProduct(book);
            
            Product food = new FoodCategory("Pizza", "2023-12-31", "Cheese, Tomato");
            food.Price = 10;
            food.IsAvailable = true;
            marketplace.AddProduct(food);
            
            marketplace.PrintProducts();
            // Create an order
            Order order = marketplace.CreateOrder();
            order.AddProduct(electronics);
            order.AddProduct(clothing);

            // Checkout the order
            marketplace.CheckOutOrder(order);
        }
    }
}
