using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalMeetingApp
{
    public class MeetingFilter
    {
        private readonly IRepository repository;

        public MeetingFilter(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Meeting> FilterByCategory()
        {
            Console.WriteLine("1.CodeMonkey, 2.Hub, 3.Short, 4.Teambuilding");
            var filterBy = Console.ReadLine();//readline => integer => meeting category
            var filterMeetingCategory = MeetingCategory.TeamBuilding;
            return this.repository.MeetingByCategory(filterMeetingCategory);
        }
        public IEnumerable<Meeting> FilterByDescription()
        {
            Console.WriteLine("Enter searching phrase");
            var filterBy = Console.ReadLine();
            return this.repository.MeetingByDescription(filterBy);
        }
        public IEnumerable<Meeting> FilterByResponsiblePerson()
        {
            Console.WriteLine("Enter Name");
            var firstName = Console.ReadLine();
            var person = new Person();
            person.FirstName = firstName;
            var lastName = Console.ReadLine();
            person.LastName = lastName;
            return this.repository.MeetingByResponsiblePerson(person);
        }

        public IEnumerable<Meeting> FilterByType()
        {
            Console.WriteLine("Live = 1, InPerson = 2");
            var filterBy = Console.ReadLine();//readline => integer => meeting category
            var filterMeetingByType = MeetingType.TeamBuilding;
            return this.repository.MeetingByType(filterMeetingByType);
        }
        public IEnumerable<Meeting> FilterByDate()
        {
            Console.WriteLine("Enter date from in format MM/dd/yyyy");
            var fromDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter date from in format MM/dd/yyyy");
            var toDate = DateTime.Parse(Console.ReadLine());
            return this.repository.MeetingByDate(fromDate, toDate);
        }
    }
}
