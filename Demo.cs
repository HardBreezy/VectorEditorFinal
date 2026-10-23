using System.Diagnostics;
using System.Text;
using VectorEditor.Models;
using VectorEditor.Patterns;

namespace VectorEditor;

/// <summary>Сценарии демо-стенда: по одному блоку на каждый паттерн и каждое требование задания.</summary>
public static class Demo
{
    public static void Run()
    {
        var scene = BuildScene(announce: true);
        var original = BuildScene(announce: false); // нетронутая копия: нужна для картинки «до»

        Section("Сценарий 1. Composite: сцена из фигур и вложенных групп");
        ShowScene(scene);

        ScenarioMoveAndScale(scene);
        ScenarioExport(scene);
        ScenarioBeforeAfter(original, scene);
    }

    // --- сценарий 1: построение сцены ---
    private static ShapeGroup BuildScene(bool announce)
    {
        var house = new ShapeGroup("Дом")
            .Add(new Rectangle("Стены", 100, 100, 80, 60))
            .Add(new Line("Крыша-лево", 100, 100, 140, 70))
            .Add(new Line("Крыша-право", 140, 70, 180, 100));

        var sun = new ShapeGroup("Солнце")
            .Add(new Circle("Диск", 220, 40, 15))
            .Add(new Line("Луч", 220, 15, 220, 5));

        var scene = new ShapeGroup("Сцена")
            .Add(house)
            .Add(sun)
            .Add(new Circle("Облако", 40, 40, 20));

        if (announce)
        {
            Console.WriteLine("  Собрана сцена: дом (прямоугольник + 2 линии), солнце (круг + линия) и отдельное облако.");
            Console.WriteLine("  Группы вложены в группу «Сцена», фигуры и группы используются через один интерфейс IShape.");
        }
        return scene;
    }

    // --- сценарий 2: перемещение и масштабирование группы ---
    private static void ScenarioMoveAndScale(ShapeGroup scene)
    {
        Section("Сценарий 2. Группа ведёт себя как одна фигура");

        var house = (ShapeGroup)scene.Children[0];

        Console.WriteLine("  Команда: Move(+50, +20) для группы «Дом»");
        house.Move(50, 20);
        ShowScene(house);

        var bounds = house.Accept(new BoundsVisitor())!.Value;
        Console.WriteLine($"\n  Команда: Scale(×2) для группы «Дом» относительно её центра ({bounds.CenterX:0.##}; {bounds.CenterY:0.##})");
        house.Scale(2, bounds.CenterX, bounds.CenterY);
        ShowScene(house);

        Console.WriteLine("\n  Команда: Move(-10, 0) для всей сцены (вложенные группы двигаются вместе)");
        scene.Move(-10, 0);
        ShowScene(scene);
    }

    // --- сценарий 3: экспорт в два формата ---
    private static void ScenarioExport(ShapeGroup scene)
    {
        Section("Сценарий 3. Visitor: экспорт сцены в два формата");

        Console.WriteLine("  Формат 1: SVG-подобный текст (SvgExportVisitor)\n");
        Console.WriteLine(new SvgExportVisitor().Export(scene, 400, 300));

        Console.WriteLine("\n  Формат 2: JSON (JsonExportVisitor)\n");
        Console.WriteLine(new JsonExportVisitor().Export(scene));
    }

    // --- сценарий 4: было / стало, картинки сохраняются и открываются автоматически ---
    private static void ScenarioBeforeAfter(ShapeGroup original, ShapeGroup scene)
    {
        Section("Сценарий 4. Было / стало");

        Console.WriteLine("  Дом сдвинут на (+50; +20) и увеличен ×2, затем вся сцена сдвинута на (−10; 0).\n");
        PrintSideBySide(original, scene);

        var beforePath = Path.GetFullPath("before.svg");
        var afterPath = Path.GetFullPath("after.svg");
        try
        {
            var exporter = new SvgExportVisitor();
            File.WriteAllText(beforePath, exporter.Export(original, 400, 300), new UTF8Encoding(false));
            File.WriteAllText(afterPath, exporter.Export(scene, 400, 300), new UTF8Encoding(false));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"\n  Не удалось сохранить картинки: {ex.Message}");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"  Картинка «до»:    {beforePath}");
        Console.WriteLine($"  Картинка «после»: {afterPath}");

        TryOpen(beforePath);
        TryOpen(afterPath);
    }

    /// <summary>Открывает файл в программе по умолчанию; если не получилось — просто сообщает.</summary>
    private static void TryOpen(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception)
        {
            Console.WriteLine($"  Не удалось открыть {Path.GetFileName(path)} автоматически ({ex.Message}). Откройте файл вручную.");
        }
    }

    /// <summary>Две колонки: слева дерево до изменений, справа после.</summary>
    private static void PrintSideBySide(IShape before, IShape after)
    {
        var left = before.Accept(new OutlineVisitor()).Split('\n');
        var right = after.Accept(new OutlineVisitor()).Split('\n');
        const int width = 50;

        Console.WriteLine("  " + "БЫЛО".PadRight(width) + "СТАЛО");
        Console.WriteLine("  " + new string('-', width - 2) + "  " + new string('-', width - 2));
        for (var i = 0; i < Math.Max(left.Length, right.Length); i++)
        {
            var l = i < left.Length ? left[i] : string.Empty;
            var r = i < right.Length ? right[i] : string.Empty;
            Console.WriteLine("  " + l.PadRight(width) + r);
        }
    }

    private static void ShowScene(IShape shape)
    {
        var text = shape.Accept(new OutlineVisitor());
        foreach (var line in text.Split('\n'))
            Console.WriteLine("  " + line);
    }

    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }
}
