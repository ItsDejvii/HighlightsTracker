using HighlightsTracker.Models;
using System.Text.Json;

namespace HighlightsTracker.Services
{
    public class StorageService
    {
        private static readonly string personsPath = Path.Combine(FileSystem.AppDataDirectory, "persons.json");
        private static readonly string eventsPath = Path.Combine(FileSystem.AppDataDirectory, "events.json");
        private static readonly string timerPath = Path.Combine(FileSystem.AppDataDirectory, "timer.txt");

        public static async Task SavePersonsAsync(List<Person> data)
        {
            var json = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(personsPath, json);
        }

        public static async Task SaveEventsAsync(List<Event> data)
        {
            var json = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(eventsPath, json);
        }

        public static async Task SaveTimerAsync(TimeSpan timerValue)
        {
            string timer = timerValue.ToString(@"hh\:mm\:ss");
            await File.WriteAllTextAsync(timerPath, timer);
        }

        public static async Task<TimeSpan> LoadTimerAsync()
        {
            if (!File.Exists(timerPath))
                return new(0,0,0);

            string? timer = await File.ReadAllTextAsync(timerPath);

            if (string.IsNullOrEmpty(timer))
                return new(0,0,0);

            string[] times = timer.Split(':');
            TimeSpan timerVal = new(int.Parse(times[0]), int.Parse(times[1]), int.Parse(times[2]));
            return timerVal;

        }

        public static async Task<List<Person>> LoadPersonsAsync()
        {
            if (!File.Exists(personsPath))
                return [];

            var json = await File.ReadAllTextAsync(personsPath);
            List<Person>? people = JsonSerializer.Deserialize<List<Person>>(json);

            return people ?? [];
        }

        public static async Task<List<Event>> LoadEventsAsync()
        {
            if (!File.Exists(eventsPath))
                return [];

            var json = await File.ReadAllTextAsync(eventsPath);
            List<Event>? events = JsonSerializer.Deserialize<List<Event>>(json);

            return events ?? [];
        }
    }
}
