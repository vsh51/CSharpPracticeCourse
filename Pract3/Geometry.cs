using System;

namespace Pract3
{
    struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double DistanceTo(Point other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    struct Rectangle
    {
        public Point X { get; set; }
        public Point Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(Point x, Point y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public double Area()
        {
            return Width * Height;
        }

        public double Perimeter()
        {
            return 2 * (Width + Height);
        }

        public override string ToString()
        {
            return $"Rectangle with vertices {X}, {Y}, width {Width} and height {Height}.";
        }
    }
}
