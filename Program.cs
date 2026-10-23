using System.Globalization;
using System.Text;
using VectorEditor;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
CultureInfo.CurrentCulture = new CultureInfo("ru-RU");

Demo.Run();

Console.WriteLine();
Console.WriteLine("Готово. Нажмите Enter для выхода.");
Console.ReadLine();
