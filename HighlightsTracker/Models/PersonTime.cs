namespace HighlightsTracker.Models
{
    public class PersonTime(Person person, TimeSpan time)
    {
        public Person Person { get; set; } = person;
        public TimeSpan Time { get; set; } = time;
    }
}


