using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        string directory = AppContext.BaseDirectory;
        string logPath = Path.Combine(directory, "actions.log");

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        bool running = true;

        while (running)
        {
            Console.WriteLine("=== Управление профилями ===");
            Console.WriteLine("1. Создать профиль");
            Console.WriteLine("2. Записать результат игры");
            Console.WriteLine("3. Показать профиль");
            Console.WriteLine("4. Показать лог действий");
            Console.WriteLine("5. Выход");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Никнейм: ");
                string nickname = Console.ReadLine();
                string profilePath = Path.Combine(directory, "profile_" + nickname + ".json");

                if (File.Exists(profilePath))
                {
                    Console.WriteLine("Профиль уже существует!");
                }
                else
                {
                    PlayerProfile profile = new PlayerProfile();
                    profile.Nickname = nickname;
                    profile.GamesPlayed = 0;
                    profile.Wins = 0;
                    profile.WinRate = 0;

                    string json = JsonSerializer.Serialize(profile, options);
                    File.WriteAllText(profilePath, json);

                    string time = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                    File.AppendAllText(logPath, "[" + time + "] Создан профиль: " + nickname + "\n");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Никнейм: ");
                string nickname = Console.ReadLine();
                string profilePath = Path.Combine(directory, "profile_" + nickname + ".json");

                if (File.Exists(profilePath))
                {
                    Console.Write("Результат (win/loss): ");
                    string result = Console.ReadLine();

                    if (result == "win" || result == "loss")
                    {
                        string json = File.ReadAllText(profilePath);
                        PlayerProfile profile = JsonSerializer.Deserialize<PlayerProfile>(json);

                        profile.GamesPlayed++;

                        if (result == "win")
                        {
                            profile.Wins++;
                        }

                        profile.WinRate = profile.Wins * 100.0 / profile.GamesPlayed;

                        string updatedJson = JsonSerializer.Serialize(profile, options);
                        File.WriteAllText(profilePath, updatedJson);

                        string time = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                        File.AppendAllText(logPath, "[" + time + "] " + nickname + ": " + result + " (Всего игр: " + profile.GamesPlayed + ", Побед: " + profile.Wins + ", WinRate: " + profile.WinRate.ToString("F1") + "%)\n");
                    }
                }
                else
                {
                    Console.WriteLine("Профиль не найден");
                }
            }
            else if (choice == "3")
            {
                Console.Write("Никнейм: ");
                string nickname = Console.ReadLine();
                string profilePath = Path.Combine(directory, "profile_" + nickname + ".json");

                if (File.Exists(profilePath))
                {
                    string json = File.ReadAllText(profilePath);
                    PlayerProfile profile = JsonSerializer.Deserialize<PlayerProfile>(json);

                    Console.WriteLine("Никнейм: " + profile.Nickname);
                    Console.WriteLine("Всего игр: " + profile.GamesPlayed);
                    Console.WriteLine("Побед: " + profile.Wins);
                    Console.WriteLine("WinRate: " + profile.WinRate.ToString("F1") + "%");
                }
                else
                {
                    Console.WriteLine("Профиль не найден");
                }
            }
            else if (choice == "4")
            {
                if (File.Exists(logPath))
                {
                    string log = File.ReadAllText(logPath);
                    Console.WriteLine(log);
                }
            }
            else if (choice == "5")
            {
                running = false;
            }
        }
    }
}

public class PlayerProfile
{
    public string Nickname { get; set; } = "";
    public int GamesPlayed { get; set; }
    public int Wins { get; set; }
    public double WinRate { get; set; }
}
