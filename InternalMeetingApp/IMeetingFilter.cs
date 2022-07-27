
namespace InternalMeetingApp
{
    public interface IMeetingFilter
    {
        IEnumerable<Meeting> FilterByCategory();
        IEnumerable<Meeting> FilterByDate();
        IEnumerable<Meeting> FilterByDescription();
        IEnumerable<Meeting> FilterByResponsiblePerson();
        IEnumerable<Meeting> FilterByType();
    }
}