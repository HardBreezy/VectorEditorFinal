using System.Globalization;
using VectorEditor.Models;

namespace VectorEditor.Patterns;

/// <summary>Ещё одна операция: текстовая структура сцены в виде дерева (для показа в консоли).</summary>
public sealed class OutlineVisitor : IShapeVisitor<string>
{
    private int _depth;

    public string Visit(Circle c) =>
        $"{Pad}○ {c.Name}: центр ({N(c.CenterX)}; {N(c.CenterY)}), r={N(c.Radius)}";

    public string Visit(Rectangle r) =>
        $"{Pad}▭ {r.Name}: ({N(r.X)}; {N(r.Y)}), {N(r.Width)}×{N(r.Height)}";

    public string Visit(Line l) =>
        $"{Pad}╱ {l.Name}: ({N(l.X1)}; {N(l.Y1)}) → ({N(l.X2)}; {N(l.Y2)})";

    public string Visit(ShapeGroup g)
    {
        var header = $"{Pad}▾ группа «{g.Name}» ({g.Children.Count} шт.)";
        _depth++;
        var lines = g.Children.Select(child => child.Accept(this)).ToList();
        _depth--;
        return lines.Count == 0 ? header : header + "\n" + string.Join("\n", lines);
    }

    private string Pad => new(' ', _depth * 3);
    private static string N(double v) => v.ToString("0.##", CultureInfo.InvariantCulture);
}
