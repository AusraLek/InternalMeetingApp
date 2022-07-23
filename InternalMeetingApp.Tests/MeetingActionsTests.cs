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
        private readonly Mock<IRepository> repository;

        public MeetingActionsTests()
        {
            this.repository = new Mock<IRepository>();
            var meetingFilter = new MeetingFilter(this.repository.Object);
            this.consoleHandler = new Mock<IConsoleHandler>();
            this.meetingActions = new MeetingActions(this.repository.Object, meetingFilter, this.consoleHandler.Object);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(429495)]
        public void DeleteMeeting(int index)
        {
            // Arrange
            var person = new Person
            {
                FirstName = "Ausra",
                LastName = "Lekaviciute"
            };
            this.repository
                .Setup(mock => mock.ListAll())
                .Returns(new List<Meeting>());
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(index + 1);

            // Act
            this.meetingActions.DeleteMeeting(person);

            // Assert
            this.repository
                .Verify(mock => mock.Delete(index, person), Times.Once);
            this.consoleHandler
                .Verify(mock => mock.Notify(It.IsAny<string>()), Times.Once);

            //this.repository.ListAll().Count().Should().Be(1);
            //Assert.AreEqual(0, this.repository.ListAll().Count());

        }

        [TestMethod]
        [DataRow("Ausra", "Lekaviciute")]
        [DataRow(null, null)]
        public void AddMeeting(string firstName, string lastName)
        {
            // Arrange
            Meeting actualMeeting = null;
            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName
            };
            this.consoleHandler
                .SetupSequence(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(4)
                .Returns(2);
            this.consoleHandler
                .SetupSequence(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("Test Name")
                .Returns("Test Description");
            this.repository
                .Setup(mock => mock.Add(It.IsAny<Meeting>()))
                .Callback<Meeting>(meeting => actualMeeting = meeting);

            // Act
            this.meetingActions.AddMeeting(person);

            // Assert
            actualMeeting
                .Should()
                .BeEquivalentTo(new Meeting
                {
                    Name = "Test Name",
                    Description = "Test Description",
                    Category = MeetingCategory.TeamBuilding,
                    Type = MeetingType.InPerson,
                    ResponsiblePerson = new Person
                    {
                        FirstName = firstName,
                        LastName = lastName
                    }
                });

            //Assert.AreEqual(1, this.repository.ListAll().Count());

        }
    }
}
