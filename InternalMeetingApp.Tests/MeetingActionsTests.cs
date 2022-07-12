using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalMeetingApp.Tests
{
    [TestClass]
    public class MeetingActionsTests
    {
        private readonly Mock<IConsoleHandler> consoleHandler;
        private readonly MeetingActions meetingActions;
        private readonly Repository repository;

        public MeetingActionsTests()
        {
            this.repository = new Repository();
            var meetingFilter = new MeetingFilter(this.repository);
            this.consoleHandler = new Mock<IConsoleHandler>();
            this.meetingActions = new MeetingActions(this.repository, meetingFilter, this.consoleHandler.Object);
        }

        [TestMethod]
        public void DeleteMeeting()
        {
            var meeting = new Meeting();
            meeting.Name = "meeto neimas";
            var person = new Person();
            person.FirstName = "Ausra";
            person.LastName = "Lekaviciute";
            meeting.ResponsiblePerson = person;
            this.repository.Add(meeting);
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(1);

            this.meetingActions.DeleteMeeting(person);

            Assert.AreEqual(0, this.repository.ListAll().Count());

        }
    }
}
