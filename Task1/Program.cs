using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string path = Path.Combine(AppContext.BaseDirectory, "journal.json");
        AdventureJournal journal;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            journal = JsonSerializer.Deserialize<AdventureJournal>(json);
        }
        else
        {
            Console.Write("Имя героя: ");
            string heroName = Console.ReadLine();

            journal = new AdventureJournal();
            journal.HeroName = heroName;
            journal.Quests = new List<QuestEntry>();

            File.WriteAllText(path, JsonSerializer.Serialize(journal, options));
        }

        bool running = true;

        while (running)
        {
            Console.WriteLine("=== Журнал приключений ===");
            Console.WriteLine("1. Добавить квест");
            Console.WriteLine("2. Завершить квест");
            Console.WriteLine("3. Провалить квест");
            Console.WriteLine("4. Показать все квесты");
            Console.WriteLine("5. Показать только активные квесты");
            Console.WriteLine("6. Выход");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Название: ");
                string name = Console.ReadLine();

                Console.Write("Описание: ");
                string description = Console.ReadLine();

                QuestEntry quest = new QuestEntry();
                quest.QuestName = name;
                quest.Description = description;
                quest.Status = "Active";

                journal.Quests.Add(quest);
                File.WriteAllText(path, JsonSerializer.Serialize(journal, options));
            }
            else if (choice == "2" || choice == "3")
            {
                string newStatus = "Completed";

                if (choice == "3")
                {
                    newStatus = "Failed";
                }

                int activeCount = 0;

                for (int i = 0; i < journal.Quests.Count; i++)
                {
                    if (journal.Quests[i].Status == "Active")
                    {
                        activeCount++;
                    }
                }

                if (activeCount == 0)
                {
                    Console.WriteLine("Активных квестов нет.");
                }
                else
                {
                    int activeNumber = 1;

                    for (int i = 0; i < journal.Quests.Count; i++)
                    {
                        if (journal.Quests[i].Status == "Active")
                        {
                            Console.WriteLine(activeNumber + ". " + journal.Quests[i].QuestName);
                            activeNumber++;
                        }
                    }

                    Console.Write("Номер: ");
                    int selectedNumber = int.Parse(Console.ReadLine());
                    activeNumber = 0;

                    for (int i = 0; i < journal.Quests.Count; i++)
                    {
                        if (journal.Quests[i].Status == "Active")
                        {
                            activeNumber++;

                            if (activeNumber == selectedNumber)
                            {
                                journal.Quests[i].Status = newStatus;
                            }
                        }
                    }

                    File.WriteAllText(path, JsonSerializer.Serialize(journal, options));
                }
            }
            else if (choice == "4")
            {
                for (int i = 0; i < journal.Quests.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + journal.Quests[i].QuestName + " [" + journal.Quests[i].Status + "] — " + journal.Quests[i].Description);
                }
            }
            else if (choice == "5")
            {
                int activeNumber = 1;

                for (int i = 0; i < journal.Quests.Count; i++)
                {
                    if (journal.Quests[i].Status == "Active")
                    {
                        Console.WriteLine(activeNumber + ". " + journal.Quests[i].QuestName + " — " + journal.Quests[i].Description);
                        activeNumber++;
                    }
                }
            }
            else if (choice == "6")
            {
                running = false;
            }
        }
    }
}

public class QuestEntry
{
    public string QuestName { get; set; } = "";
    public string Status { get; set; } = "";
    public string Description { get; set; } = "";
}

public class AdventureJournal
{
    public string HeroName { get; set; } = "";
    public List<QuestEntry> Quests { get; set; } = new List<QuestEntry>();
}
