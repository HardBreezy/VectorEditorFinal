namespace VectorEditor.Models;

internal static class ScaleGuard
{
    public static void Check(double factor)
    {
        if (factor <= 0 || double.IsNaN(factor) || double.IsInfinity(factor))
            throw new ArgumentOutOfRangeException(nameof(factor), "Коэффициент масштаба должен быть больше 0");
    }
}
