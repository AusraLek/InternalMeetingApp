using InternalMeetingApp;

var repository = new Repository();
var loginPerson = InputFullName();
var exit = false;

while (!exit)
{
    Console.WriteLine("Press number 1. Add meeting, 2. Delete a meeting, 3.Add a person to the meeting " + 
        "4. Remove a person from the meeting. 5. List all the meetings 6. Filter meetings by: 7. Exit");

    var action = Console.ReadLine();

    if (action == "1")
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

        repository.Add(meeting);
        Console.Clear();
        Console.WriteLine("Meeting successfully added!");
    }

    if (action == "2")
    {
        var deleteIndex = SelectMeeting(repository);
        repository.Delete(deleteIndex, loginPerson);
        Console.Clear();
        Console.WriteLine("Meeting successfully deleted!");
    }

    if (action == "3")
    {
        var addPersonDate = DateTime.Now;
        var personToAdd = InputFullName();

        var atendee = new Atendee();
        atendee.Person = personToAdd;
        atendee.AdditionTime = addPersonDate;
        var atendeeMeetingIndex = SelectMeeting(repository);
        repository.AddAtendee(atendeeMeetingIndex, atendee);
        Console.Clear();
        Console.WriteLine("Person successfully added!");
    }

    if (action == "4")
    {
        var personToDelete = InputFullName();
        var atendeeMeetingIndex = SelectMeeting(repository);
        repository.DeleteAtendee(atendeeMeetingIndex, personToDelete);
        Console.Clear();
        Console.WriteLine("Person successfully deleted!");
    }

    if (action == "5")
    {
        var meetings = repository.ListAll();
        PrintMeetings(meetings);
    }

    if (action == "6") //filter meetings
    {
        Console.WriteLine("Filter by " +
            "1. Description " +
            "2. Responsible Person " +
            "3. Meeting Category " +
            "4. By Type " +
            "5. By Date ");

        var filternumber = Console.ReadLine();

        switch (filternumber)
        {
            case "1":
                PrintMeetings(FilterByDescription(repository));
                break;
            case "2":
                PrintMeetings(FilterByResponsiblePerson(repository));
                break;
            case "3":
                PrintMeetings(FilterByCategory(repository));
                break;
            case "4":
                PrintMeetings(FilterByType(repository));
                break;
            case "5":
                PrintMeetings(FilterByDate(repository));
                break;
        }
    }

    repository.Save();

    if (action == "7")
    {
        exit = true;
    }
}

 static int SelectMeeting(Repository repository)
{
    int index = 1;
    var meetings = repository.ListAll();

    foreach (var meet in meetings)
    {
        Console.WriteLine($"{index} {meet.Name} {meet.StartDate} - {meet.EndDate}");
        index++;
    }
    Console.WriteLine("Enter meeting ID:");
    var enteredIndex = int.Parse(Console.ReadLine());

    return enteredIndex - 1;
}

static Person InputFullName()
{
    var person = new Person();
    Console.WriteLine("Enter full name");
    var name = Console.ReadLine();
    var nameSpliting = name.Split(' ');
    person.FirstName = nameSpliting[0];
    person.LastName = nameSpliting[1];
    return person;
}

static void PrintMeetings(IEnumerable<Meeting> meetings)
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

static IEnumerable<Meeting> FilterByCategory(Repository repository)
{
    Console.WriteLine("1.CodeMonkey, 2.Hub, 3.Short, 4.Teambuilding");
    var filterBy = Console.ReadLine();//readline => integer => meeting category
    var filterMeetingCategory = MeetingCategory.TeamBuilding;
    return repository.MeetingByCategory(filterMeetingCategory);
}
static IEnumerable<Meeting> FilterByDescription(Repository repository)
{
    Console.WriteLine("Enter searching phrase");
    var filterBy = Console.ReadLine();
    return repository.MeetingByDescription(filterBy);
}
static IEnumerable<Meeting> FilterByResponsiblePerson(Repository repository)
{
    Console.WriteLine("Enter Name");
    var firstName = Console.ReadLine();
    var person = new Person();
    person.FirstName = firstName;
    var lastName = Console.ReadLine();
    person.LastName = lastName;
    return repository.MeetingByResponsiblePerson(person);
}

static IEnumerable<Meeting> FilterByType(Repository repository)
{
    Console.WriteLine("Live = 1, InPerson = 2");
    var filterBy = Console.ReadLine();//readline => integer => meeting category
    var filterMeetingByType = MeetingType.TeamBuilding;
    return repository.MeetingByType(filterMeetingByType);
}
static IEnumerable<Meeting> FilterByDate(Repository repository)
{
    Console.WriteLine("Enter date from in format MM/dd/yyyy");
    var fromDate = DateTime.Parse(Console.ReadLine());
    Console.WriteLine("Enter date from in format MM/dd/yyyy");
    var toDate = DateTime.Parse(Console.ReadLine());
    return repository.MeetingByDate(fromDate, toDate);
}