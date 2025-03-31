using System.Collections.ObjectModel;

namespace HighlightsTracker.Models
{
    public class Person(string name)
    {
        public string Name { get; set; } = name;
        public ObservableCollection<PersonTime> PersonTimes { get; set; } = [];
    }
}


