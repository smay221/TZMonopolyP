using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;
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

                if (newPalet.bestBeforeDate > newBox.bestBeforeDate && newPalet.bestBeforeDate != DateTime.Parse("01/01/0001"))
                {
                    newPalet.bestBeforeDate = newBox.bestBeforeDate;
                }
                else if (newPalet.bestBeforeDate == DateTime.Parse("01/01/0001"))
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
        int groupCounter = 1;
        List<Pallet> currentGroupList = new List<Pallet> { };
        List<Pallet> palletsList = JsonConvert.DeserializeObject<List<Pallet>>(json);
        List<Pallet> sortedPalletsByDate = palletsList.OrderByDescending(pallet => pallet.bestBeforeDate).ToList();
        DateTime lastDateTime = sortedPalletsByDate[0].bestBeforeDate;
        List<Pallet> sortedPalletsByBest = new List<Pallet>();

        for (int j = 0; j < 3; j++)
        {
            sortedPalletsByBest.Add(sortedPalletsByDate[j]);
        }
        sortedPalletsByBest = sortedPalletsByBest.OrderByDescending(pallet => pallet.volume).Reverse().ToList();
        Console.WriteLine("\n3 паллета, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"**Паллет с ID: {sortedPalletsByBest[i].ID}\n Ширана - {sortedPalletsByBest[i].weight}\n Высота - {sortedPalletsByBest[i].height}" +$"\n Глубина - {sortedPalletsByBest[i].depth}\n Вес - {sortedPalletsByBest[i].weight}\n Объём - {sortedPalletsByBest[i].volume}\n Срок годности - {sortedPalletsByBest[i].bestBeforeDate}\n Коробки внутри:");
            foreach (Box box in sortedPalletsByBest[i].boxes)
            {
                Console.WriteLine($"***ID: {box.ID}\n  Ширана - {box.weight}\n  Высота - {box.height}" +
                $"\n  Глубина - {box.depth}\n  Вес - {box.weight}\n  Объём - {box.volume}\n  Срок годности - {box.bestBeforeDate}");
            }
        }

        sortedPalletsByDate.Reverse();


        var groupPalletsByDate = from pallet in sortedPalletsByDate
                                          group pallet by pallet.bestBeforeDate;
        foreach(var groupP in groupPalletsByDate)
        {
            List<Pallet> sourtedGroupP = groupP.OrderByDescending(pallet => pallet.weight).ToList();
            Console.WriteLine("\n" +"*Группа" + groupP.Key + "\n");
            foreach (var pallet in sourtedGroupP) 
            {
                Console.WriteLine($"**Паллет с ID: {pallet.ID}\n Ширана - {pallet.weight}\n Высота - {pallet.height}" +
                    $"\n Глубина - {pallet.depth}\n Вес - {pallet.weight}\n Объём - {pallet.volume}\n Срок годности - {pallet.bestBeforeDate}\n Коробки внутри:");
                foreach (Box box in pallet.boxes)
                {
                    Console.WriteLine($"***ID: {box.ID}\n  Ширана - {box.weight}\n  Высота - {box.height}" +
                    $"\n  Глубина - {box.depth}\n  Вес - {box.weight}\n  Объём - {box.volume}\n  Срок годности - {box.bestBeforeDate}");
                }

            }
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