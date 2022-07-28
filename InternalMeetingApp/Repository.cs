using System.Text.Json;

namespace InternalMeetingApp
{
    public class Repository : IRepository
    {
        private List<Meeting> Meetings { get; set; }

        public Repository()
        {
            Meetings = new List<Meeting>();
            string path = "repository.json";
            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                Meetings = JsonSerializer.Deserialize<List<Meeting>>(jsonString);
            }
        }

        public IEnumerable<Meeting> ListAll()
        {
            return this.Meetings;
        }
        public void Add(Meeting meeting)
        {
            Meetings.Add(meeting);
        }
        public void Delete(int index, Person person)
        {
            var responsiblePerson = this.Meetings[index].ResponsiblePerson;

            if (responsiblePerson.FirstName == person.FirstName && responsiblePerson.LastName == person.LastName)
            {
                this.Meetings.RemoveAt(index);
                Console.WriteLine("The meeting has been deleted");
            }
            else
            {
                Console.WriteLine("Only the responsible person can delete the meeting!");
            }
        }

        public void AddAtendee(int index, Atendee atendee)
        {
            var atendees = this.Meetings[index].Atendees;
            if (!atendees.Any(a => atendee.Person.FirstName == a.Person.FirstName && atendee.Person.LastName == a.Person.LastName))
            {
                Meetings[index].Atendees.Add(atendee);
            }
            IEnumerable<Meeting> meetsByDate = Meetings.Where(meeting => meeting.Name != Meetings[index].Name && (meeting.StartDate >= Meetings[index].StartDate || Meetings[index].EndDate >= meeting.EndDate));
            foreach (var meet in meetsByDate)
            {
                if (meet.Atendees.Any(a => atendee.Person.FirstName == a.Person.FirstName && atendee.Person.LastName == a.Person.LastName))
                {
                    Console.WriteLine("Person is already in a meeting which intersects with the one being added");
                }
            }
        }

        public void DeleteAtendee(int index, Person atendee)
        {
            if (Meetings[index].ResponsiblePerson.FirstName != atendee.FirstName && Meetings[index].ResponsiblePerson.LastName != atendee.LastName)
            {
                Meetings[index].Atendees.RemoveAll(a => a.Person.FirstName == atendee.FirstName && a.Person.LastName == atendee.LastName);
            }
        }

        public void Save()
        {
            string fileName = "repository.json";
            string jsonString = JsonSerializer.Serialize(Meetings);
            File.WriteAllText(fileName, jsonString);
        }

        public IEnumerable<Meeting> MeetingByDescription(string description)
        {
            return this.Meetings.Where(meeting => meeting.Description.Contains(description));
        }

        public IEnumerable<Meeting> MeetingByCategory(MeetingCategory meetingCategory)
        {
            return this.Meetings.Where(meeting => meeting.Category == meetingCategory);
        }

        public IEnumerable<Meeting> MeetingByResponsiblePerson(Person person)
        {
            return this.Meetings.Where(meeting => meeting.ResponsiblePerson.FirstName == person.FirstName && meeting.ResponsiblePerson.LastName == person.LastName);
        }
        public IEnumerable<Meeting> MeetingByType(MeetingType meetingType)
        {
            return this.Meetings.Where(meeting => meeting.Type == meetingType);
        }

        public IEnumerable<Meeting> MeetingByDate(DateTime startDate, DateTime endDate)
        {
            return this.Meetings.Where(meeting => this.MeetingIsInRange(meeting.StartDate, meeting.EndDate, startDate, endDate));
        }

        private bool MeetingIsInRange(DateTime meetingStart, DateTime meetingEnd, DateTime rangeStart, DateTime rangeEnd)
        {
            if (meetingStart <= rangeStart && meetingEnd >= rangeEnd)
            {
                return true;
            }

            if (meetingStart >= rangeStart && meetingEnd <= rangeEnd)
            {
                return true;
            }

            if (meetingStart >= rangeStart && meetingEnd >= rangeEnd)
            {
                return true;
            }

            if (meetingStart <= rangeStart && meetingEnd <= rangeEnd)
            {
                return true;
            }

            return false;
        }
    }
}
