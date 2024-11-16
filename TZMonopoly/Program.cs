using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

public class Pallet
{
    public int ID { get; set; }
    public double width { get; set; }
    public double height { get; set; }
    public double depth { get; set; }
    public double weight { get; set; }
    public double volume { get; set; }
    public DateTime bestBeforeDate { get; set; }
    public List<Box> boxes { get; set; } = new List<Box>();


}

public class Box
{
    public int ID { get; set; }
    public double width { get; set; }
    public double height { get; set; }
    public double depth { get; set; }
    public double weight { get; set; }
    public double volume { get; set; }
    public DateTime bestBeforeDate { get; set; }

}

public class MonopolyMain
{
    public static void MakeNewPallet()
    {
        Pallet newPalet = new Pallet();
        try
        {
            Console.WriteLine("Введите ширину паллета");
            newPalet.width = Double.Parse(Console.ReadLine());
            Console.WriteLine("Введите высоту паллета");
            newPalet.height = Double.Parse(Console.ReadLine());
            Console.WriteLine("Введите глубину паллета");
            newPalet.depth = Double.Parse(Console.ReadLine());
        }

        catch 
        {
            Console.WriteLine("Данные введены некорректно");
            return;
        }
        newPalet.weight = 30;

        int boxIdCounter = 0;

        while (true)
        {
            Console.WriteLine("Что вы хотите сделать с коробкой?(Удалить/Добавить/Продолжить)");
            string isContinue = Console.ReadLine();

            if (isContinue.ToLower() == "добавить")
            {
                Box newBox = MakeNewBox(newPalet);
                newBox.ID = boxIdCounter;
                boxIdCounter++;
                newPalet.boxes.Add(newBox);
                newPalet.weight += newBox.weight;
                newPalet.volume += newBox.volume;

                if (newPalet.bestBeforeDate != null && newPalet.bestBeforeDate > newBox.bestBeforeDate)
                {
                    newPalet.bestBeforeDate = newBox.bestBeforeDate;
                }

                else if (newPalet.bestBeforeDate == null)
                {
                    newPalet.bestBeforeDate = newBox.bestBeforeDate;
                }
            } 
            else if (isContinue.ToLower() == "продолжить")
            {
                break;
            }
            else if (isContinue.ToLower() == "удалить")
            {
                Console.WriteLine("Введите ID коробки которую хотите удалить");
                foreach (Box box in newPalet.boxes)
                {
                    Console.WriteLine($"ID: {box.ID}\n Ширана - {box.weight}\n Высота - {box.height}" +
                    $"\n Глубина{box.depth}\n Вес - {box.weight}\n Объём - {box.volume}\n Срок годности - {box.bestBeforeDate}\n");
                }
                try
                {
                    List<Box> newBoxes = new List<Box>();
                    int selectedId = int.Parse(Console.ReadLine());
                    foreach (Box box in newPalet.boxes)
                    {
                        if (box.ID != selectedId)
                        {
                            newBoxes.Add(box);
                        }
                    }
                    newPalet.boxes = newBoxes;
                
                }
                catch 
                {
                    Console.WriteLine("ID введён некорректно");
                    continue;
                }
               
            }
            else
            {
                Console.WriteLine("Команда не распознана");
            }
        }
        newPalet.volume += newPalet.weight * newPalet.height * newPalet.depth;


        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "PalletsList.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<Pallet> palletsList = JsonConvert.DeserializeObject<List<Pallet>>(json);

            newPalet.ID = palletsList[palletsList.Count - 1].ID + 1;
            palletsList.Add(newPalet);

            string updatedJson = JsonConvert.SerializeObject(palletsList, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }
        else
        {
            List<Pallet> palletsList = new List<Pallet>();
            palletsList.Add(newPalet);

            string json = JsonConvert.SerializeObject(palletsList, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

    public static Box MakeNewBox(Pallet newPallet)
    {
        Box newBox = new Box();
        while (true)
        {
            try
            {
                Console.WriteLine("Введите ширину коробки");
                newBox.width = Double.Parse(Console.ReadLine());
            }

            catch
            {
                Console.WriteLine("Данные введены некорректно");
                continue;
            }

            if (newBox.width > newPallet.width)
            {
                Console.WriteLine("Ширина коробки не может превышать ширину паллета.");
            }
            else
            {
                break;
            }
        }
        while (true)
        {
            try
            {
                Console.WriteLine("Введите высоту коробки");
                newBox.height = Double.Parse(Console.ReadLine());

            }
            catch
            {
                Console.WriteLine("Данные введены некорректно");
                continue;
            }
            break;
        }
        while (true)
        {
            try
            {
                Console.WriteLine("Введите глубину коробки");
                newBox.depth = Double.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Данные введены некорректно");
                continue;
            }
            if (newBox.depth > newPallet.depth)
            {
                Console.WriteLine("Ширина коробки не может превышать ширину паллета.");
            }
            else
            {
                break;
            }
        }


        while (true)
        {
            try
            {
                Console.WriteLine("Введите вес коробки");
                newBox.weight = Double.Parse(Console.ReadLine());

            }
            catch
            {
                Console.WriteLine("Данные введены некорректно");
                continue;
            }
            break;
        }
        while (true)
        {
            try
            {
                Console.WriteLine("До какого числа годна продукция(введите в формате mm/dd/yyyy)");
                newBox.bestBeforeDate = DateTime.Parse(Console.ReadLine());

            }
            catch
            {
                Console.WriteLine("Данные введены некорректно");
                continue;
            }
            break;
        }
        newBox.volume = newBox.width * newBox.height * newBox.depth;



        return newBox;
    }

    public static void DeleteSelectedPallet(int selectedID)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "PalletsList.json");
        string json = File.ReadAllText(filePath);
        List<Pallet> palletsList = JsonConvert.DeserializeObject<List<Pallet>>(json);
        List<Pallet> newPalletList = new List<Pallet>();
        foreach (Pallet pallet in palletsList)
        {
            if(pallet.ID != selectedID)
            {
                newPalletList.Add(pallet);
            }
        }
        string updatedJson = JsonConvert.SerializeObject(newPalletList, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);

    }

    public static void ReadAllPallets()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "PalletsList.json");
        string json = File.ReadAllText(filePath);
        List<Pallet> palletsList = JsonConvert.DeserializeObject<List<Pallet>>(json);
        List<Pallet> sortedPallets = palletsList.OrderByDescending(pallet => pallet.bestBeforeDate).ToList();
        foreach (Pallet pallet in sortedPallets)
        {
            Console.WriteLine("*****Все Паллеты*****");
            Console.WriteLine($"Паллет с ID: {pallet.ID}\nШирана - {pallet.weight}\nВысота - {pallet.height}" +
                $"\nГлубина - {pallet.depth}\nВес - {pallet.weight}\nОбъём - {pallet.volume}\nСрок годности - {pallet.bestBeforeDate}\nКоробки внутри:");
            foreach (Box box in pallet.boxes)
            {
                Console.WriteLine($"ID: {box.ID}\n Ширана - {box.weight}\n Высота - {box.height}" +
                $"\n Глубина - {box.depth}\n Вес - {box.weight}\n Объём - {box.volume}\n Срок годности - {box.bestBeforeDate}\n");
            }
            Console.WriteLine("*****3 паллета с наибольшим сроком годности, отсортированные по возрастанию объема*****");
            //Console.WriteLine()
        }

    }



    public static void Main(string[] args)
    {
        while(true)
        {
            Console.WriteLine("Возможные команды:\nСоздать новый палет - 1\nУдалить палет - 2\nСчитать все палеты - 3");
            string readedCommand = Console.ReadLine();
            if (readedCommand == "1")
            {
                MakeNewPallet();
            }
            if (readedCommand == "2") 
            {
                Console.WriteLine("Введите ID паллета который вы хотите удалить");
                try
                {
                    DeleteSelectedPallet(int.Parse(Console.ReadLine()));
                }
                catch
                {
                    Console.WriteLine("ID введён некорректно");
                }
            }
            if(readedCommand == "3")
            {
                ReadAllPallets();
            }
        }
    }
}