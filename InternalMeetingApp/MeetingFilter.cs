namespace InternalMeetingApp
{
    public class MeetingFilter : IMeetingFilter
    {
        private readonly IRepository repository;
        private readonly IConsoleHandler consoleHandler;

        public MeetingFilter(IRepository repository, IConsoleHandler consoleHandler)
        {
            this.repository = repository;
            this.consoleHandler = consoleHandler;
        }

        public IEnumerable<Meeting> FilterByCategory()
        {
            var filterBy = this.consoleHandler
                .AskForInt("1.CodeMonkey, 2.Hub, 3.Short, 4.Teambuilding");
            var filterMeetingCategory = (MeetingCategory)filterBy;
            return this.repository.MeetingByCategory(filterMeetingCategory);
        }

        public IEnumerable<Meeting> FilterByDescription()
        {
            var filterBy = this.consoleHandler
                .AskForString("Enter searching phrase");
            return this.repository.MeetingByDescription(filterBy);
        }

        public IEnumerable<Meeting> FilterByResponsiblePerson()
        {
            var firstName = this.consoleHandler
                .AskForString("Enter First Name");
            var lastName = this.consoleHandler
                .AskForString("Enter Last Name");
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
            };
            return this.repository.MeetingByResponsiblePerson(person);
        }

        public IEnumerable<Meeting> FilterByType()
        {
            var filterBy = this.consoleHandler
                .AskForInt("Live = 1, InPerson = 2");
            var filterMeetingByType = (MeetingType)filterBy;
            return this.repository.MeetingByType(filterMeetingByType);
        }

        public IEnumerable<Meeting> FilterByDate()
        {
            var fromDate = this.consoleHandler
                .AskForDate("Enter start date in format MM/dd/yyyy HH:mm:ss");
            var toDate = this.consoleHandler
                .AskForDate("Enter end date from in format MM/dd/yyyy HH:mm:ss");
            return this.repository.MeetingByDate(fromDate, toDate);
        }
    }
}
