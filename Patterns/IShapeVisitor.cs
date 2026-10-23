using VectorEditor.Models;

namespace VectorEditor.Patterns;

/// <summary>
/// Посетитель (Visitor): операция над деревом фигур.
/// Новая операция (формат экспорта, подсчёт и т.п.) — новый класс, фигуры не меняются.
/// </summary>
public interface IShapeVisitor<out T>
{
    T Visit(Circle circle);
    T Visit(Rectangle rectangle);
    T Visit(Line line);
    T Visit(ShapeGroup group);
}
