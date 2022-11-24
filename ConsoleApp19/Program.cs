namespace ConsoleApp19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь для открытия файла");
            string open_path = Console.ReadLine();
            Console.Clear();
            Converted figures = new Converted(open_path);
            figures.Edit();
            Console.Clear();
            Console.WriteLine("Введите путь для сохранения файла");
            string save_path = Console.ReadLine();
            figures.SaveFile(save_path);
            Console.Clear();
            Console.WriteLine("Готово!");
        }
    }
}
