using VectorEditor.Patterns;

namespace VectorEditor.Models;

/// <summary>
/// Единый интерфейс фигуры (Composite: Component).
/// Одиночная фигура и группа обрабатываются одинаково.
/// </summary>
public interface IShape
{
    string Name { get; }

    /// <summary>Сдвигает фигуру на (dx, dy).</summary>
    void Move(double dx, double dy);

    /// <summary>Масштабирует фигуру в factor раз относительно точки (pivotX, pivotY).</summary>
    void Scale(double factor, double pivotX, double pivotY);

    /// <summary>Принимает посетителя (Visitor): так добавляются новые операции без правки фигур.</summary>
    T Accept<T>(IShapeVisitor<T> visitor);
}
