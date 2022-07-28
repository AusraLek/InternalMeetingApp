using InternalMeetingApp;

var repository = new Repository();
var consoleHandler = new ConsoleHandler();
var meetingFilter = new MeetingFilter(repository, consoleHandler);
var meetingActions = new MeetingActions(repository,meetingFilter, consoleHandler);
var loginPerson = meetingActions.InputFullName();
var exit = false;

while (!exit)
{
    Console.WriteLine("Press number 1. Add meeting, 2. Delete a meeting, 3.Add a person to the meeting " + 
        "4. Remove a person from the meeting. 5. List all the meetings 6. Filter meetings by: 7. Exit");

    var action = Console.ReadLine();

    switch (action)
    {
        case "1":
            meetingActions.AddMeeting(loginPerson);
            break;
        case "2":
            meetingActions.DeleteMeeting(loginPerson);
            break;
        case "3":
            meetingActions.AddPersonToMeeting();
            break;
        case "4":
            meetingActions.DeletePerson();
            break;
        case "5":
            meetingActions.ListAllMeetings();
            break;
        case "6":
            meetingActions.FilterMeetings();
            break;
        case "7":
            exit = true;
            break;
    }

    repository.Save();
}


