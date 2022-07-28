namespace InternalMeetingApp
{
    public class Atendee
    {
        public Person Person { get; set; }
        public DateTime AdditionTime { get; set; }

        public Atendee()
        {
            Person = new Person();
        }
    }
}
