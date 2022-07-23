namespace InternalMeetingApp
{
    public interface IRepository
    {
        void Add(Meeting meeting);
        void AddAtendee(int index, Atendee atendee);
        void Delete(int index, Person person);
        void DeleteAtendee(int index, Person atendee);
        IEnumerable<Meeting> ListAll();
        IEnumerable<Meeting> MeetingByCategory(MeetingCategory meetingCategory);
        IEnumerable<Meeting> MeetingByDate(DateTime startDate, DateTime endDate);
        IEnumerable<Meeting> MeetingByDescription(string description);
        IEnumerable<Meeting> MeetingByResponsiblePerson(Person person);
        IEnumerable<Meeting> MeetingByType(MeetingType meetingType);
        void Save();
    }
}