namespace HighlightsTracker.Models
{
    public class Event(string name, TimeSpan time, string title)
    {
        public string Name { get; set; } = name;
        public TimeSpan Time { get; set; } = time;
        public string Title { get; set; } = title;
    }
}


