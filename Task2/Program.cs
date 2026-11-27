using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        string directory = AppContext.BaseDirectory;
        string csvPath = Path.Combine(directory, "monsters.csv");
        string jsonPath = Path.Combine(directory, "monsters.json");

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string[] lines = File.ReadAllLines(csvPath);
        List<Monster> monsters = new List<Monster>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(';');

            Monster monster = new Monster();
            monster.Name = parts[0];
            monster.Health = int.Parse(parts[1]);
            monster.Damage = int.Parse(parts[2]);
            monster.Type = parts[3];

            monsters.Add(monster);
        }

        string json = JsonSerializer.Serialize(monsters, options);
        File.WriteAllText(jsonPath, json);

        string loadedJson = File.ReadAllText(jsonPath);
        List<Monster> loadedMonsters = JsonSerializer.Deserialize<List<Monster>>(loadedJson);

        for (int i = 0; i < loadedMonsters.Count; i++)
        {
            Console.WriteLine("[" + loadedMonsters[i].Type + "] " + loadedMonsters[i].Name + " — HP: " + loadedMonsters[i].Health + ", DMG: " + loadedMonsters[i].Damage);
        }

        Monster maxHealth = loadedMonsters[0];
        Monster maxDamage = loadedMonsters[0];

        for (int i = 0; i < loadedMonsters.Count; i++)
        {
            if (loadedMonsters[i].Health > maxHealth.Health)
            {
                maxHealth = loadedMonsters[i];
            }

            if (loadedMonsters[i].Damage > maxDamage.Damage)
            {
                maxDamage = loadedMonsters[i];
            }
        }

        Console.WriteLine("Общее количество монстров: " + loadedMonsters.Count);
        Console.WriteLine("Наибольшее здоровье: " + maxHealth.Name + " — " + maxHealth.Health + " HP");
        Console.WriteLine("Наибольший урон: " + maxDamage.Name + " — " + maxDamage.Damage + " DMG");
    }
}

public class Monster
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int Damage { get; set; }
    public string Type { get; set; } = "";
}
