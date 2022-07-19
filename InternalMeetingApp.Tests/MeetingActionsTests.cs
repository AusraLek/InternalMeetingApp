using FluentAssertions;
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
            // Arrange
            var meeting = new Meeting
            {
                Name = "meeto neimas",
            };
            var person = new Person
            {
                FirstName = "Ausra",
                LastName = "Lekaviciute"
            };

            meeting.ResponsiblePerson = person;
            this.repository.Add(meeting);
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(1);

            // Act
            this.meetingActions.DeleteMeeting(person);

            // Assert
            this.repository.ListAll().Should().HaveCount(1);

            //this.repository.ListAll().Count().Should().Be(1);
            //Assert.AreEqual(0, this.repository.ListAll().Count());

        }

        [TestMethod]
        public void AddMeeting()
        {
            // Arrange
            var repository = new Repository();
            var person = new Person
            {
                FirstName = "Ausra",
                LastName = "Lekaviciute"
            };
            this.consoleHandler
                .SetupSequence(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(4)
                .Returns(2);
            this.consoleHandler
                .SetupSequence(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("Test Name")
                .Returns("Test Description");

            // Act
            this.meetingActions.AddMeeting(person);

            // Assert
            this.repository.ListAll().Should().HaveCount(1);
            this.repository
                .ListAll()
                .First()
                .Should()
                .BeEquivalentTo(new Meeting
                {
                    Name = "Test Name",
                    Description = "Test Description",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.InPerson,
                    ResponsiblePerson = new Person
                    {
                        FirstName = "Ausra",
                        LastName = "Lekaviciute"
                    }
                });

            //Assert.AreEqual(1, this.repository.ListAll().Count());

        }


    }
}
