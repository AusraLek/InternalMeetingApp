namespace InternalMeetingApp
{
    public class MeetingActions
    {
        private readonly IRepository repository;
        private readonly IMeetingFilter meetingFilter;
        private readonly IConsoleHandler consoleHandler;

        public MeetingActions(IRepository repository, IMeetingFilter meetingFilter, IConsoleHandler consoleHandler)
        {
            this.repository = repository;
            this.meetingFilter = meetingFilter;
            this.consoleHandler = consoleHandler;
        }

        public void AddMeeting(Person loginPerson)
        {
            var meeting = new Meeting();
            meeting.ResponsiblePerson = loginPerson;

            meeting.Name = this.consoleHandler.AskForString("Enter Meeting Name");

            meeting.Description = this.consoleHandler.AskForString("Enter description");

            var categoryIndex = this.consoleHandler.AskForInt("Enter number of category: " +
                "1.CodeMonkey, 2.Hub, 3.Short, 4.Teambuilding");
            meeting.Category = (MeetingCategory)categoryIndex;

            var typeIndex = this.consoleHandler.AskForInt("Enter number of type: 1.Live, 2.In person");
            meeting.Type = (MeetingType)typeIndex;

            meeting.StartDate = this.consoleHandler.AskForDate("Enter start date in format MM/dd/yyyy HH:mm:ss");
            meeting.EndDate = this.consoleHandler.AskForDate("Enter end date in format MM/dd/yyyy HH:mm:ss");

            this.repository.Add(meeting);
            this.consoleHandler.Notify("Meeting successfully added!");
        }

        public void DeleteMeeting(Person loginPerson)
        {
            var deleteIndex = SelectMeeting();
            this.repository.Delete(deleteIndex, loginPerson);
            this.consoleHandler.Notify("Meeting successfully deleted!");
        }

        public void AddPersonToMeeting()
        {
            var addPersonDate = DateTime.Now;
            var personToAdd = InputFullName();

            var atendee = new Atendee();
            atendee.Person = personToAdd;
            atendee.AdditionTime = addPersonDate;
            var atendeeMeetingIndex = SelectMeeting();
            this.repository.AddAtendee(atendeeMeetingIndex, atendee);
            this.consoleHandler.Notify("Person successfully added!");
        }

        public void DeletePerson()
        {
            var atendeeMeetingIndex = SelectMeeting();
            var personToDelete = InputFullName();
            this.repository.DeleteAtendee(atendeeMeetingIndex, personToDelete);
            this.consoleHandler.Notify("Person successfully deleted!");
        }

        public void ListAllMeetings()
        {
            var meetings = this.repository.ListAll();
            PrintMeetings(meetings);
        }

        public void FilterMeetings()
        {
           var filternumber = this.consoleHandler.AskForString("Filter by " +
           "1. Description " +
           "2. Responsible Person " +
           "3. Meeting Category " +
           "4. By Type " +
           "5. By Date ");

            var filteredMeetings = filternumber switch
            {
                "1" => this.meetingFilter.FilterByDescription(),
                "2" => this.meetingFilter.FilterByResponsiblePerson(),
                "3" => this.meetingFilter.FilterByCategory(),
                "4" => this.meetingFilter.FilterByType(),
                "5" => this.meetingFilter.FilterByDate(),
                _ => throw new Exception()
            };
            PrintMeetings(filteredMeetings);
        }

        public int SelectMeeting()
        {
            int index = 1;
            var meetings = this.repository.ListAll();

            foreach (var meet in meetings)
            {
                this.consoleHandler.Print($"{index} {meet.Name} {meet.StartDate} - {meet.EndDate}");
                index++;
            }
            var enteredIndex = this.consoleHandler.AskForInt("Enter meeting ID:");
            return enteredIndex - 1;
        }

        public Person InputFullName()
        {
            var person = new Person();
            var name = this.consoleHandler.AskForString("Enter full name");
            var nameSpliting = name.Split(' ');
            person.FirstName = nameSpliting[0];
            person.LastName = nameSpliting[1];
            return person;
        }

        private void PrintMeetings(IEnumerable<Meeting> meetings)
        {
            foreach (var meet in meetings)
            {
                List<string> atendees = new List<string>();
                foreach (var atendee in meet.Atendees)
                {
                    atendees.Add($"{atendee.Person.FirstName} {atendee.Person.LastName}");
                }

                this.consoleHandler.Print(
                    $"Name: {meet.Name}",
                    $"ResponsiblePerson: {meet.ResponsiblePerson.FirstName} {meet.ResponsiblePerson.LastName}",
                    $"Atendees: {string.Join(", ", atendees)}",
                    $"Description: {meet.Description}",
                    $"Category: {meet.Category}",
                    $"Type: {meet.Type}",
                    $"StartDate: {meet.StartDate}",
                    $"EndDate: {meet.EndDate}",
                    "");
            }
        }
    }
}
