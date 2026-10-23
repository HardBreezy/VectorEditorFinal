using VectorEditor.Patterns;

namespace VectorEditor.Models;

/// <summary>
/// Группа фигур (Composite: Composite). Сама является фигурой,
/// поэтому группы могут вкладываться друг в друга.
/// </summary>
public sealed class ShapeGroup : IShape
{
    private readonly List<IShape> _children = new();

    public string Name { get; }
    public IReadOnlyList<IShape> Children => _children;

    public ShapeGroup(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Не задано имя группы", nameof(name));
        Name = name;
    }

    public ShapeGroup Add(IShape shape)
    {
        ArgumentNullException.ThrowIfNull(shape);

        if (ReferenceEquals(shape, this) || (shape is ShapeGroup g && g.Contains(this)))
            throw new InvalidOperationException($"Нельзя добавить «{shape.Name}» в «{Name}»: получится цикл");
        if (_children.Contains(shape))
            throw new InvalidOperationException($"Фигура «{shape.Name}» уже есть в группе «{Name}»");

        _children.Add(shape);
        return this;
    }

    public bool Remove(IShape shape) => _children.Remove(shape);

    /// <summary>Есть ли фигура в этой группе на любом уровне вложенности.</summary>
    public bool Contains(IShape shape)
    {
        foreach (var child in _children)
        {
            if (ReferenceEquals(child, shape)) return true;
            if (child is ShapeGroup group && group.Contains(shape)) return true;
        }
        return false;
    }

    // Операции делегируются детям: группа двигается и масштабируется как одна фигура.
    public void Move(double dx, double dy)
    {
        foreach (var child in _children)
            child.Move(dx, dy);
    }

    public void Scale(double factor, double pivotX, double pivotY)
    {
        ScaleGuard.Check(factor);
        foreach (var child in _children)
            child.Scale(factor, pivotX, pivotY);
    }

    public T Accept<T>(IShapeVisitor<T> visitor) => visitor.Visit(this);
}
