using VectorEditor.Patterns;

namespace VectorEditor.Models;

/// <summary>Прямоугольник (Composite: Leaf). X, Y — левый верхний угол.</summary>
public sealed class Rectangle : IShape
{
    public string Name { get; }
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Width { get; private set; }
    public double Height { get; private set; }

    public Rectangle(string name, double x, double y, double width, double height)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Не задано имя фигуры", nameof(name));
        if (width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Ширина и высота должны быть больше 0");

        Name = name;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public void Move(double dx, double dy)
    {
        X += dx;
        Y += dy;
    }

    public void Scale(double factor, double pivotX, double pivotY)
    {
        ScaleGuard.Check(factor);
        X = pivotX + (X - pivotX) * factor;
        Y = pivotY + (Y - pivotY) * factor;
        Width *= factor;
        Height *= factor;
    }

    public T Accept<T>(IShapeVisitor<T> visitor) => visitor.Visit(this);
}
