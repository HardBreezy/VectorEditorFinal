using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using VectorEditor.Models;

namespace VectorEditor.Patterns;

/// <summary>Экспорт в JSON. Каждая фигура превращается в JSON-узел, группа — в узел с массивом children.</summary>
public sealed class JsonExportVisitor : IShapeVisitor<JsonNode>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // кириллица без \uXXXX
    };

    public string Export(IShape root) => root.Accept(this).ToJsonString(Options);

    public JsonNode Visit(Circle c) => new JsonObject
    {
        ["type"] = "circle",
        ["name"] = c.Name,
        ["cx"] = Round(c.CenterX),
        ["cy"] = Round(c.CenterY),
        ["r"] = Round(c.Radius)
    };

    public JsonNode Visit(Rectangle r) => new JsonObject
    {
        ["type"] = "rectangle",
        ["name"] = r.Name,
        ["x"] = Round(r.X),
        ["y"] = Round(r.Y),
        ["width"] = Round(r.Width),
        ["height"] = Round(r.Height)
    };

    public JsonNode Visit(Line l) => new JsonObject
    {
        ["type"] = "line",
        ["name"] = l.Name,
        ["x1"] = Round(l.X1),
        ["y1"] = Round(l.Y1),
        ["x2"] = Round(l.X2),
        ["y2"] = Round(l.Y2)
    };

    public JsonNode Visit(ShapeGroup g)
    {
        var children = new JsonArray();
        foreach (var child in g.Children)
            children.Add(child.Accept(this));

        return new JsonObject
        {
            ["type"] = "group",
            ["name"] = g.Name,
            ["children"] = children
        };
    }

    private static double Round(double v) => Math.Round(v, 2);
}
