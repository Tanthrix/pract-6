using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp19
{
    public class Figure
    {
        public Figure() { }
        public Figure(string _name, int _width, int _height) { name = _name; width = _width; height = _height; }
        public string getStringed()
        {
            return $"{name}\n{width}\n{height}";
        }
        public string name;
        public int width;
        public int height;
    }
    public class Converted
    {
        public Converted(string _path)
        {
            path = _path;
            obj = Deserialize(_path);
            text = ToTxtView(obj);
        }
        public void Edit()
        {
            int max_pos = 1;
            int pos = 1;

            string[] lines = text.Split('\n');
            int edit = 0;
            while (max_pos != 0) {
                

                Console.Clear();
                Console.WriteLine($"File {path.Split('\\')[^1]}. F1 to save & exit");
                max_pos = 1;
                foreach (string line in lines)
                {
                    ++max_pos;
                    Console.WriteLine("   " + line);
                }
                if (edit != 0)
                {
                    Console.SetCursorPosition(3, pos);
                    lines[edit - 1] = Console.ReadLine();
                    edit = 0;
                }
                Console.SetCursorPosition(0, pos);

                Console.Write("->");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.F1:
                        max_pos = 0;
                        break;
                    case ConsoleKey.UpArrow:
                        if (pos == 1)
                            pos = max_pos;
                        else
                            pos--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (pos == max_pos)
                            pos = 1;
                        else
                            pos++;
                        break;
                    case ConsoleKey.Enter:
                        lines[pos - 1] = "";
                        edit = pos;
                        break;
                }
            }
            text = String.Join('\n', lines).Replace("\r", "");
            obj = ToList(text: text);
        }
        private string path;
        static private List<Figure> obj;
        private string text;
        private static List<Figure> ToList(string path = "", string text = "")
        {
            if (text == "")
                text = File.ReadAllText(path);
            List<string> lines = text.Split("\n").ToList();
            List<Figure> figures = new List<Figure>();
            lines.RemoveAll(x => x == "");
            for (int i = 0; i < lines.Count(); i += 3)
            {
                try
                {
                    string name = lines[i];
                    int width = Convert.ToInt32(lines[i + 1]);
                    int height = Convert.ToInt32(lines[i + 2]);
                    Figure figure = new Figure(name, width, height);
                    figures.Add(figure);
                }
                catch
                {
                    break;
                }
            }
            return figures;
        }
        private static List<Figure> Deserialize(string path)
        {
            List<Figure> result = new List<Figure>();
            switch (path.Split('.')[^1])
            {
                case "json":
                    result = JsonConvert.DeserializeObject<List<Figure>>(File.ReadAllText(path));
                    break;
                case "xml":
                    XmlSerializer xml = new XmlSerializer(typeof(List<Figure>));
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        result = (List<Figure>)xml.Deserialize(fs);
                    }
                    break;
                case "txt":
                    result = ToList(path);
                    break;
            }
            return result;
        }
        private static string ToTxtView(List<Figure> figures)
        {
            List<string> result = new List<string>();
            foreach (Figure figure in figures)
            {
                result.Add(figure.getStringed());
            }
            return String.Join('\n', result);
        }
        public void SaveFile(string save_path)
        {
            switch (save_path.Split('.')[^1])
            {
                case "json":
                    File.WriteAllText(save_path, JsonConvert.SerializeObject(obj));
                    break;
                case "xml":
                    XmlSerializer xml = new XmlSerializer(typeof(List<Figure>));
                    using (FileStream fs = new FileStream(save_path, FileMode.OpenOrCreate))
                    {
                        xml.Serialize(fs, obj);
                    }                    
                    break;
                case "txt":
                    File.WriteAllText(save_path, ToTxtView(obj));
                    break;
            }
        }
    }
}