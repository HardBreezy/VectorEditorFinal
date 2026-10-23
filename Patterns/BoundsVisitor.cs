using VectorEditor.Models;

namespace VectorEditor.Patterns;

/// <summary>Габаритный прямоугольник.</summary>
public readonly record struct Bounds(double MinX, double MinY, double MaxX, double MaxY)
{
    public double CenterX => (MinX + MaxX) / 2;
    public double CenterY => (MinY + MaxY) / 2;
    public double Width => MaxX - MinX;
    public double Height => MaxY - MinY;

    public static Bounds Union(Bounds a, Bounds b) => new(
        Math.Min(a.MinX, b.MinX), Math.Min(a.MinY, b.MinY),
        Math.Max(a.MaxX, b.MaxX), Math.Max(a.MaxY, b.MaxY));
}

/// <summary>
/// Новая операция над фигурами — габариты. Добавлена отдельным классом,
/// без единой правки Circle/Rectangle/Line/ShapeGroup.
/// </summary>
public sealed class BoundsVisitor : IShapeVisitor<Bounds?>
{
    public Bounds? Visit(Circle c) =>
        new Bounds(c.CenterX - c.Radius, c.CenterY - c.Radius, c.CenterX + c.Radius, c.CenterY + c.Radius);

    public Bounds? Visit(Rectangle r) =>
        new Bounds(r.X, r.Y, r.X + r.Width, r.Y + r.Height);

    public Bounds? Visit(Line l) =>
        new Bounds(Math.Min(l.X1, l.X2), Math.Min(l.Y1, l.Y2), Math.Max(l.X1, l.X2), Math.Max(l.Y1, l.Y2));

    public Bounds? Visit(ShapeGroup g)
    {
        Bounds? result = null;
        foreach (var child in g.Children)
        {
            var b = child.Accept(this);
            if (b is null) continue;
            result = result is null ? b : Bounds.Union(result.Value, b.Value);
        }
        return result; // пустая группа габаритов не имеет
    }
}
