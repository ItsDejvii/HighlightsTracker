using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HighlightsTracker.Models;
using Kotlin.Properties;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HighlightsTracker.ViewModels
{
    public partial class PeopleViewModel : ObservableObject
    {
        public ObservableCollection<Person> People { get; set; } = [];
        
        [ObservableProperty]
        public Stopwatch timer = new();

        [ObservableProperty]
        public string nameInput = "", timerText = "00:00:00";

        [ObservableProperty]
        public bool isRunning = false;

        public PeopleViewModel(){}

        [RelayCommand]
        private void RemovePerson(Person person)
        {
            if (person != null)
            {
                People.Remove(person);
            }
        }

        [RelayCommand]
        private void AddPerson(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            People.Add(new Person(name));
        }

        [RelayCommand]
        private void AddPersonTime(Person person)
        {
            People.First(p => p == person).PersonTimes.Add(new(person, new(Timer.Elapsed.Hours, Timer.Elapsed.Minutes, Timer.Elapsed.Seconds)));
        }

        [RelayCommand]
        private void RemovePersonTime(PersonTime pt)
        {
            People.First(p => p.PersonTimes.Contains(pt)).PersonTimes.Remove(pt);
        }

        [RelayCommand]
        private void CopyToClipboard()
        {
            string names = string.Join(", ", People.Select(p => p.Name));
            Clipboard.SetTextAsync(names);
        }

        [RelayCommand]
        private void ClearTimes()
        {
            for (int i = 0; i < People.Count; i++)
            {
                People[i].PersonTimes.Clear();
            }
        }
    }
}
