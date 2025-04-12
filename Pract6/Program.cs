using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pract6
{
    public interface ICategory
    {
        string Name { get; set; }
        string ToString();
        string Info();
    }

    public abstract class ProductBase
    {
        public abstract string Title { get; set; }
        public abstract double Price { get; set; }
        public abstract bool IsAvailable { get; set; }
        public abstract ICategory Category { get; set; }
    }

    public class Product<TCategory> : ProductBase where TCategory : ICategory
    {
        public override string Title { get; set; }
        public override double Price { get; set; }
        public override bool IsAvailable { get; set; }

        public override ICategory Category { get; set; }

        public Product(string title, double price, bool isAvailable, TCategory category)
        {
            Title = title;
            Price = price;
            IsAvailable = isAvailable;
            Category = category;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Price: {Price}, IsAvailable: {IsAvailable}, Category: {Category.ToString()}";
        }
    }

    abstract public class Category : ICategory
    {
        public abstract string Name { get; set; }
        public abstract string Info();
        public override string ToString()
        {
            return Name;
        }
    }

    public class ElectnicsCategory : Category
    {
        public override string Name { get; set; }
        
        public ElectnicsCategory(string name)
        {
            Name = name;
        }

        public override string Info()
        {
            return $"Electronics Category: {Name}";
        }
    }

    public class ClothingCategory : Category
    {
        public string Brand { get; set; }
        public override string Name { get; set; }
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
    }

    public class BooksCategory : Category
    {
        public override string Name { get; set; }
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
    }

    public class FoodCategory : Category
    {
        public override string Name { get; set; }
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
    }

    public interface IOrder<TCategory> where TCategory : ICategory
    {
        void AddProduct(Product<TCategory> product);
        void RemoveProduct(Product<TCategory> product);
        double CalculateTotalPrice();
        void Checkout();
    }

    public class Order<TCategory> : IOrder<TCategory> where TCategory : ICategory
    {
        private List<Product<TCategory>> _products;

        public Order()
        {
            _products = new List<Product<TCategory>>();
        }

        public void AddProduct(Product<TCategory> product)
        {
            _products.Add(product);
        } 

        public void RemoveProduct(Product<TCategory> product)
        {
            _products.Remove(product);
        }

        public double CalculateTotalPrice()
        {
            return _products.Sum(p => p.Price);
        }

        public void Checkout()
        {
            Console.WriteLine("Checkout complete. Total price: " + CalculateTotalPrice() + "\n");
            foreach (Product<TCategory> product in _products)
            {
                product.IsAvailable = false;
            }
            _products.Clear();
        }
    }

    public class Marketplace
    {
        private List<ProductBase> _products;
        private List<Object> _orders;

        public Marketplace()
        {
            _products = new List<ProductBase>();
            _orders = new List<Object>();
        }

        public void AddProduct(ProductBase product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(ProductBase product)
        {
            _products.Remove(product);
        }

        public List<ProductBase> GetAvailableProducts()
        {
            return _products.Where(p => p.IsAvailable).ToList();
        }

        public List<ProductBase> GetProductsByCategory(ICategory category)
        {
            return _products.Where(p => p.Category.Name == category.Name).ToList();
        }

        public List<ProductBase> GetProductsByPriceRange(double minPrice, double maxPrice)
        {
            return _products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
        }

        public Order<TCategory> CreateOrder<TCategory>
        () where TCategory : ICategory
        {
            Order<TCategory> order = new Order<TCategory>();
            _orders.Add(order);
            return order;
        }

        public bool CheckOutOrder<TCategory>(Order<TCategory> order) where TCategory : ICategory
        {
            if (_orders.Contains(order))
            {
                order.Checkout();
                _orders.Remove(order);
                return true;
            }
            return false;
        }

        public void UpdateProductsOwnership()
        {
            _products.RemoveAll(p => !p.IsAvailable);
        }

        public void PrintProducts()
        {
            Console.WriteLine("Products:");
            foreach (var product in _products)
            {
                Console.WriteLine(product?.ToString());
            }
            Console.WriteLine();
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Marketplace marketplace = new Marketplace();

            ElectnicsCategory electronicsCategory = new ElectnicsCategory("Electronics");
            ClothingCategory clothingCategory = new ClothingCategory("Clothing", "Nike", "M");
            BooksCategory booksCategory = new BooksCategory("Books", "J.K. Rowling", "Fantasy");
            FoodCategory foodCategory = new FoodCategory("Food", "2023-12-31", "Ingredients");

            Product<ElectnicsCategory> phone = new Product<ElectnicsCategory>("Phone", 500, true, electronicsCategory);
            Product<ElectnicsCategory> laptop = new Product<ElectnicsCategory>("Laptop", 1000, true, electronicsCategory);
            Product<ClothingCategory> jacket = new Product<ClothingCategory>("Jacket", 50, true, clothingCategory);
            Product<ClothingCategory> tShirt = new Product<ClothingCategory>("T-Shirt", 20, true, clothingCategory);
            Product<BooksCategory> book1 = new Product<BooksCategory>("Book 1", 10, true, booksCategory);
            Product<BooksCategory> book = new Product<BooksCategory>("Harry Potter", 15, true, booksCategory);
            Product<FoodCategory> bread = new Product<FoodCategory>("Bread", 2, true, foodCategory);
            Product<FoodCategory> apple = new Product<FoodCategory>("Apple", 1, true, foodCategory);

            marketplace.AddProduct(laptop);
            marketplace.AddProduct(tShirt);
            marketplace.AddProduct(book);
            marketplace.AddProduct(apple);
            marketplace.AddProduct(phone);
            marketplace.AddProduct(jacket);
            marketplace.AddProduct(book1);
            marketplace.AddProduct(bread);
            
            marketplace.PrintProducts();

            Order<ElectnicsCategory> order = marketplace.CreateOrder<ElectnicsCategory>();
            order.AddProduct(phone);
            order.AddProduct(laptop);
            marketplace.CheckOutOrder(order);

            marketplace.PrintProducts();

            marketplace.UpdateProductsOwnership();

            marketplace.PrintProducts();
        }
    }
}
