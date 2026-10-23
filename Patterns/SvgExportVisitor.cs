using System.Globalization;
using System.Security;
using VectorEditor.Models;

namespace VectorEditor.Patterns;

/// <summary>Экспорт в SVG-подобный текст. Группа превращается в &lt;g&gt;.</summary>
public sealed class SvgExportVisitor : IShapeVisitor<string>
{
    private int _depth = 1;

    /// <summary>Экспортирует сцену целиком: корневой тег svg и вложенные фигуры.</summary>
    public string Export(IShape root, int width, int height)
    {
        _depth = 1;
        var body = root.Accept(this);
        // fill/stroke на корне наследуются всеми фигурами: контуры видны, заливки нет
        return $"<svg width=\"{width}\" height=\"{height}\" xmlns=\"http://www.w3.org/2000/svg\" " +
               $"fill=\"none\" stroke=\"#1b2430\" stroke-width=\"2\">\n{body}\n</svg>";
    }

    public string Visit(Circle c) =>
        $"{Indent}<circle id=\"{Esc(c.Name)}\" cx=\"{N(c.CenterX)}\" cy=\"{N(c.CenterY)}\" r=\"{N(c.Radius)}\" />";

    public string Visit(Rectangle r) =>
        $"{Indent}<rect id=\"{Esc(r.Name)}\" x=\"{N(r.X)}\" y=\"{N(r.Y)}\" width=\"{N(r.Width)}\" height=\"{N(r.Height)}\" />";

    public string Visit(Line l) =>
        $"{Indent}<line id=\"{Esc(l.Name)}\" x1=\"{N(l.X1)}\" y1=\"{N(l.Y1)}\" x2=\"{N(l.X2)}\" y2=\"{N(l.Y2)}\" />";

    public string Visit(ShapeGroup g)
    {
        var open = $"{Indent}<g id=\"{Esc(g.Name)}\">";
        _depth++;
        var children = g.Children.Select(child => child.Accept(this)).ToList();
        _depth--;
        var close = $"{Indent}</g>";

        return children.Count == 0
            ? $"{open}\n{close}"
            : $"{open}\n{string.Join("\n", children)}\n{close}";
    }

    private string Indent => new(' ', _depth * 2);
    private static string N(double v) => v.ToString("0.##", CultureInfo.InvariantCulture);
    private static string Esc(string s) => SecurityElement.Escape(s) ?? string.Empty;
}
