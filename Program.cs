using ConsoleApp62.Services;
using ConsoleApp62.models;

var service = new ChampionshipService();

while (true)
{
    Console.Clear();
    Console.WriteLine("Управление Чемпионатом");
    Console.WriteLine("1. Показать разницу голов");
    Console.WriteLine("2. Полная информация о матчах");
    Console.WriteLine("3. Матчи по дате");
    Console.WriteLine("4. Матчи конкретной команды");
    Console.WriteLine("5. Игроки забившие гол по дате");
    Console.WriteLine("6. Добавить новый матч");
    Console.WriteLine("7. Удалить матч");
    Console.WriteLine("0. Выход");
    Console.Write("\nВыберите действие: ");

    switch (Console.ReadLine())
    {
        case "1":
            service.ShowGoalDifference();
            break;
        case "2":
            service.ShowFullMatchInfo();
            break;
        case "3":
            Console.Write("Введите дату (гггг-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime d)) service.ShowMatchesByDate(d);
            break;
        case "4":
            Console.Write("Введите название команды: ");
            service.ShowMatchesByTeam(Console.ReadLine() ?? "");
            break;
        case "5":
            Console.Write("Введите дату: ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime d2)) service.ShowScorersByDate(d2);
            break;
        case "6":
            service.AddMatch(1, 2, 0, 0, DateTime.Now);
            break;
        case "7":
            Console.Write("Команда 1: "); string t1 = Console.ReadLine();
            Console.Write("Команда 2: "); string t2 = Console.ReadLine();
            Console.Write("Дата (гггг-мм-дд): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime d3)) service.DeleteMatch(t1, t2, d3);
            break;
        case "0":
            return;
    }
    Console.WriteLine("\nНажмите любую клавишу...");
    Console.ReadKey();
}