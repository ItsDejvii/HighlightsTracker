using Android.Service.Autofill;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HighlightsTracker.Models;
using HighlightsTracker.Services;
using Kotlin.Properties;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace HighlightsTracker.ViewModels
{
    public partial class PeopleViewModel : ObservableObject
    {
        public PeopleViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {

            TimerTimespan = await StorageService.LoadTimerAsync();
            TimerText = TimerTimespan.ToString(@"hh\:mm\:ss");

            List<Person> savedPeople = await StorageService.LoadPersonsAsync();
            for (int i = 0; i < savedPeople.Count; i++)
            {
                People.Add(savedPeople[i]);
            }

            List<Event> savedEvents = await StorageService.LoadEventsAsync();
            for (int i = 0; i < savedEvents.Count; i++)
            {
                Events.Add(savedEvents[i]);
            }

        }
        public ObservableCollection<Person> People { get; set; } = [];
        public ObservableCollection<Event> Events { get; set; } = [];

        public TimeSpan TimerTimespan;

        [ObservableProperty]
        public Stopwatch timer = new();

        [ObservableProperty]
        public string nameInput = "", timerText = "00:00:00";

        [ObservableProperty]
        public bool isRunning = false;

        [RelayCommand]
        private async Task RemovePerson(Person person)
        {
            if (person != null)
            {
                People.Remove(person);
                await StorageService.SavePersonsAsync(People.ToList());
            }
        }

        [RelayCommand]
        private async Task AddPerson(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            People.Add(new Person(name));
            await StorageService.SavePersonsAsync(People.ToList());
        }

        [RelayCommand]
        private async Task AddEvent(PersonEvent personEvent)
        {
            TimeSpan stopwatchTimespan = new(Timer.Elapsed.Hours, Timer.Elapsed.Minutes, Timer.Elapsed.Seconds);
            stopwatchTimespan += TimerTimespan;
            Events.Add(new(personEvent.Person.Name, stopwatchTimespan, personEvent.Event.ToString()));
            await StorageService.SaveEventsAsync(Events.ToList());
        }

        [RelayCommand]
        private async Task RemoveEvent(Event e)
        {
            Events.Remove(e);
            await StorageService.SaveEventsAsync(Events.ToList());
        }

        [RelayCommand]
        private async Task ExportToCSV()
        {
            List<string[]> csvData = [];
            for (int i = 0; i < Events.Count; i++)
            {
                string[] data = { Events[i].Name, Events[i].Time.ToString(@"hh\:mm\:ss"), Events[i].Title };
                csvData.Add(data);
            }

            FileSaverResult result = await SaveCsvFile(csvData, "events.csv");

            if (result.IsSuccessful)
            {
                await Toast.Make("File successfully saved.").Show();
            }
            else
            {
                await Toast.Make("Failed to save file.").Show();
            }

        }

        private async Task<FileSaverResult> SaveCsvFile(List<string[]> data, string fileName)
        {
            try
            {
                StringBuilder csvContent = new();

                foreach (string[] row in data)
                {
                    string escapedRow = string.Join(",", row.Select(field => $"\"{field.Replace("\"", "\"\"")}\""));
                    csvContent.AppendLine(escapedRow);
                }

                byte[] bytes = Encoding.UTF8.GetBytes(csvContent.ToString());

                using var memoryStream = new MemoryStream(bytes);

                return await FileSaver.Default.SaveAsync(fileName, memoryStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving CSV: {ex.Message}");
                return new FileSaverResult(fileName, ex);
            }
        }

        [RelayCommand]
        private async Task ClearEvents()
        {
            Events.Clear();

            await StorageService.SaveEventsAsync(Events.ToList());
        }
    }
}
