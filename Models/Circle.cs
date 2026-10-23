using VectorEditor.Patterns;

namespace VectorEditor.Models;

/// <summary>Круг (Composite: Leaf).</summary>
public sealed class Circle : IShape
{
    public string Name { get; }
    public double CenterX { get; private set; }
    public double CenterY { get; private set; }
    public double Radius { get; private set; }

    public Circle(string name, double centerX, double centerY, double radius)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Не задано имя фигуры", nameof(name));
        if (radius <= 0)
            throw new ArgumentOutOfRangeException(nameof(radius), "Радиус должен быть больше 0");

        Name = name;
        CenterX = centerX;
        CenterY = centerY;
        Radius = radius;
    }

    public void Move(double dx, double dy)
    {
        CenterX += dx;
        CenterY += dy;
    }

    public void Scale(double factor, double pivotX, double pivotY)
    {
        ScaleGuard.Check(factor);
        CenterX = pivotX + (CenterX - pivotX) * factor;
        CenterY = pivotY + (CenterY - pivotY) * factor;
        Radius *= factor;
    }

    public T Accept<T>(IShapeVisitor<T> visitor) => visitor.Visit(this);
}
