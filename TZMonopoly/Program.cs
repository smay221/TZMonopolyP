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

    public List<Box> boxes { get; set; }


}

public class Box
{
    public string bestBeforeDate { get; set; }

}

public class MonopolyMain
{
    public  void makeNewPallet()
    {
        Pallet newPalet = new Pallet(); 
        Console.WriteLine("Введите ширину паллета");
        newPalet.width = Double.Parse(Console.ReadLine());
        Console.WriteLine("Введите высоту паллета");
        newPalet.height = Double.Parse(Console.ReadLine());
        Console.WriteLine("Введите глубину паллета");
        newPalet.depth = Double.Parse(Console.ReadLine());
        Console.WriteLine("Введите вес паллета");
        newPalet.weight = Double.Parse(Console.ReadLine());

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "PalletsList.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<Pallet> palletsList = JsonConvert.DeserializeObject<List<Pallet>>(json);

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
    public  double getPalletWeight(int palletID)
    {
        double weight = 0;


        return weight;
    }

    public static void Main(string[] args)
    {
        
    }
}