using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalMeetingApp
{
    public class MeetingActions
    {
        private readonly Repository repository;
        private readonly MeetingFilter meetingFilter;

        public MeetingActions(Repository repository, MeetingFilter meetingFilter)
        {
            this.repository = repository;
            this.meetingFilter = meetingFilter;
        }

        public void AddMeeting(Person loginPerson)
        {
            var meeting = new Meeting();
            meeting.ResponsiblePerson = loginPerson;
            Console.WriteLine("Enter meeting name");
            meeting.Name = Console.ReadLine();

            Console.WriteLine("Enter description");
            meeting.Description = Console.ReadLine();

            Console.WriteLine("Enter number of category: 1.CodeMonkey, 2.Hub, 3.Short, 4.Teambuilding");
            var categoryIndex = Convert.ToInt32(Console.ReadLine());
            meeting.Category = (MeetingCategory)categoryIndex;

            Console.WriteLine("Enter number of type: 1.Live, 2.In person");
            var typeIndex = Convert.ToInt32(Console.ReadLine());
            meeting.Type = (MeetingType)typeIndex;

            Console.WriteLine("Enter start date in format MM/dd/yyyy HH:mm:ss");
            meeting.StartDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Enter end date in format MM/dd/yyyy HH:mm:ss");
            meeting.EndDate = DateTime.Parse(Console.ReadLine());

            this.repository.Add(meeting);
            Console.Clear();
            Console.WriteLine("Meeting successfully added!");
        }

        public void DeleteMeeting(Person loginPerson)
        {
            var deleteIndex = SelectMeeting();
            this.repository.Delete(deleteIndex, loginPerson);
            Console.Clear();
            Console.WriteLine("Meeting successfully deleted!");
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
            Console.Clear();
            Console.WriteLine("Person successfully added!");
        }

        public void DeletePerson()
        {
            var personToDelete = InputFullName();
            var atendeeMeetingIndex = SelectMeeting();
            this.repository.DeleteAtendee(atendeeMeetingIndex, personToDelete);
            Console.Clear();
            Console.WriteLine("Person successfully deleted!");
        }

        public void ListAllMeetings()
        {
            var meetings = this.repository.ListAll();
            PrintMeetings(meetings);
        }

        public void FilterMeetings()
        {
            Console.WriteLine("Filter by " +
           "1. Description " +
           "2. Responsible Person " +
           "3. Meeting Category " +
           "4. By Type " +
           "5. By Date ");

            var filternumber = Console.ReadLine();

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
                Console.WriteLine($"{index} {meet.Name} {meet.StartDate} - {meet.EndDate}");
                index++;
            }
            Console.WriteLine("Enter meeting ID:");
            var enteredIndex = int.Parse(Console.ReadLine());

            return enteredIndex - 1;
        }

        public Person InputFullName()
        {
            var person = new Person();
            Console.WriteLine("Enter full name");
            var name = Console.ReadLine();
            var nameSpliting = name.Split(' ');
            person.FirstName = nameSpliting[0];
            person.LastName = nameSpliting[1];
            return person;
        }

        public void PrintMeetings(IEnumerable<Meeting> meetings)
        {
            foreach (var meet in meetings)
            {
                Console.WriteLine($"Name: {meet.Name}");
                Console.WriteLine($"ResponsiblePerson: {meet.ResponsiblePerson.FirstName} {meet.ResponsiblePerson.LastName}");
                List<string> atendees = new List<string>();
                foreach (var atendee in meet.Atendees)
                {
                    atendees.Add($"{atendee.Person.FirstName} {atendee.Person.LastName}");
                }
                Console.WriteLine($"Atendees: {string.Join(", ", atendees)}");
                Console.WriteLine($"Description: {meet.Description}");
                Console.WriteLine($"Category: {meet.Category}");
                Console.WriteLine($"Type: {meet.Type}");
                Console.WriteLine($"StartDate: {meet.StartDate}");
                Console.WriteLine($"EndDate: {meet.EndDate}");
                Console.WriteLine();
            }
        }
    }
}
