namespace HighlightsTracker.Models
{
    public class PersonEvent(Person person, EventEnum e)
    {
        public Person Person { get; set; } = person;
        public EventEnum Event { get; set; } = e;
    }
}


