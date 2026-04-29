using Microsoft.EntityFrameworkCore;
using ConsoleApp62.Data;
using ConsoleApp62.models;

namespace ConsoleApp62.Services;

public class ChampionshipService
{
    private readonly FootballDbContext _db;

    public ChampionshipService() => _db = new FootballDbContext();

    public void ShowGoalDifference()
    {
        var stats = _db.Teams.Select(t => new {
            t.Name,
            Scored = _db.Matches.Where(m => m.HomeTeamId == t.Id).Sum(m => m.HomeGoals) +
                     _db.Matches.Where(m => m.AwayTeamId == t.Id).Sum(m => m.AwayGoals),
            Conceded = _db.Matches.Where(m => m.HomeTeamId == t.Id).Sum(m => m.AwayGoals) +
                       _db.Matches.Where(m => m.AwayTeamId == t.Id).Sum(m => m.HomeGoals)
        }).ToList();

        foreach (var s in stats)
            Console.WriteLine($"{s.Name,-15} | Разница: {s.Scored - s.Conceded} (Заб: {s.Scored}, Проп: {s.Conceded})");
    }

    public void ShowFullMatchInfo()
    {
        var matches = _db.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Goals).ThenInclude(g => g.Player)
            .ToList();

        foreach (var m in matches)
        {
            Console.WriteLine($"\n[{m.MatchDate:dd.MM.yyyy}] {m.HomeTeam.Name} {m.HomeGoals} : {m.AwayGoals} {m.AwayTeam.Name}");
            if (m.Goals.Any())
                Console.WriteLine("   Голы: " + string.Join(", ", m.Goals.Select(g => $"{g.Player.FullName} ({g.MinuteScored}')")));
            else
                Console.WriteLine("   Голов в этом матче не зафиксировано.");
        }
    }

    public void ShowMatchesByDate(DateTime date)
    {
        var matches = _db.Matches
            .Include(m => m.HomeTeam).Include(m => m.AwayTeam)
            .Where(m => m.MatchDate.Date == date.Date).ToList();

        if (!matches.Any()) Console.WriteLine("Матчей на эту дату нет.");
        foreach (var m in matches)
            Console.WriteLine($"{m.HomeTeam.Name} {m.HomeGoals}:{m.AwayGoals} {m.AwayTeam.Name}");
    }

    public void ShowMatchesByTeam(string teamName)
    {
        var matches = _db.Matches
            .Include(m => m.HomeTeam).Include(m => m.AwayTeam)
            .Where(m => m.HomeTeam.Name == teamName || m.AwayTeam.Name == teamName).ToList();

        foreach (var m in matches)
            Console.WriteLine($"[{m.MatchDate:d}] {m.HomeTeam.Name} {m.HomeGoals}:{m.AwayGoals} {m.AwayTeam.Name}");
    }

    public void ShowScorersByDate(DateTime date)
    {
        var scorers = _db.MatchGoals
            .Include(g => g.Player)
            .Include(g => g.Match)
            .Where(g => g.Match.MatchDate.Date == date.Date)
            .Select(g => g.Player.FullName)
            .Distinct().ToList();

        Console.WriteLine(scorers.Any() ? "Забили: " + string.Join(", ", scorers) : "В эту дату голов не было.");
    }

    public void AddMatch(int homeId, int awayId, int homeG, int awayG, DateTime date)
    {
        bool exists = _db.Matches.Any(m => m.HomeTeamId == homeId && m.AwayTeamId == awayId && m.MatchDate == date);
        if (exists)
        {
            Console.WriteLine("Такой матч уже есть в базе!");
            return;
        }

        var match = new Match { HomeTeamId = homeId, AwayTeamId = awayId, HomeGoals = homeG, AwayGoals = awayG, MatchDate = date };
        _db.Matches.Add(match);
        _db.SaveChanges();
        Console.WriteLine("Матч успешно добавлен.");
    }

    public void UpdateMatch(int id, int newHomeGoals, int newAwayGoals, DateTime newDate)
    {
        var match = _db.Matches.Find(id);
        if (match == null) { Console.WriteLine("Матч не найден."); return; }

        match.HomeGoals = newHomeGoals;
        match.AwayGoals = newAwayGoals;
        match.MatchDate = newDate;
        _db.SaveChanges();
        Console.WriteLine("Матч обновлен.");
    }

    public void DeleteMatch(string team1, string team2, DateTime date)
    {
        var match = _db.Matches.FirstOrDefault(m =>
            (m.HomeTeam.Name == team1 && m.AwayTeam.Name == team2 && m.MatchDate.Date == date.Date));

        if (match == null) { Console.WriteLine("Матч не найден."); return; }

        Console.Write($"Удалить матч {team1}-{team2} от {date:d}? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            _db.Matches.Remove(match);
            _db.SaveChanges();
            Console.WriteLine("Матч удален.");
        }
    }
}
