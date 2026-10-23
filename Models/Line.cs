using VectorEditor.Patterns;

namespace VectorEditor.Models;

/// <summary>Линия (Composite: Leaf).</summary>
public sealed class Line : IShape
{
    public string Name { get; }
    public double X1 { get; private set; }
    public double Y1 { get; private set; }
    public double X2 { get; private set; }
    public double Y2 { get; private set; }

    public Line(string name, double x1, double y1, double x2, double y2)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Не задано имя фигуры", nameof(name));

        Name = name;
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public void Move(double dx, double dy)
    {
        X1 += dx;
        Y1 += dy;
        X2 += dx;
        Y2 += dy;
    }

    public void Scale(double factor, double pivotX, double pivotY)
    {
        ScaleGuard.Check(factor);
        X1 = pivotX + (X1 - pivotX) * factor;
        Y1 = pivotY + (Y1 - pivotY) * factor;
        X2 = pivotX + (X2 - pivotX) * factor;
        Y2 = pivotY + (Y2 - pivotY) * factor;
    }

    public T Accept<T>(IShapeVisitor<T> visitor) => visitor.Visit(this);
}
