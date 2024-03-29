﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace InternalMeetingApp.Tests
{
    [TestClass]
    public class MeetingActionsTests
    {
        private readonly Mock<IConsoleHandler> consoleHandler;
        private readonly MeetingActions meetingActions;
        private readonly Mock<IRepository> repository;
        private readonly Mock<IMeetingFilter> meetingFilter;

        public MeetingActionsTests()
        {
            this.repository = new Mock<IRepository>();
            this.meetingFilter = new Mock<IMeetingFilter>();
            this.consoleHandler = new Mock<IConsoleHandler>();
            this.meetingActions = new MeetingActions(this.repository.Object, this.meetingFilter.Object, this.consoleHandler.Object);
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
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(429495)]
        public void DeletePerson(int index)
        {
            // Arrange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("Full Name");
            this.repository
                .Setup(mock => mock.ListAll())
                .Returns(new List<Meeting>());
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(index + 1);

            // Act
            this.meetingActions.DeletePerson();

            // Assert
            this.repository
                .Verify(mock => mock.DeleteAtendee(index, It.IsAny<Person>()), Times.Once);
            this.consoleHandler
                .Verify(mock => mock.Notify(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ListAllMeetings()
        {
            // Arange
            var meetings = new List<Meeting> { new Meeting(), new Meeting() };
            this.repository
                .Setup(mock => mock.ListAll())
                .Returns(meetings);

            // Act
            this.meetingActions.ListAllMeetings();

            // Assert
            this.consoleHandler
                .Verify(mock => mock.Print(It.IsAny<string[]>()), Times.Exactly(2));
        }

        [TestMethod]
        public void SelectMeeting()
        {
            // Arange
            var meetsList = new List<Meeting>();
            var meet1 = new Meeting();
            var meet2 = new Meeting();
            meetsList.Add(meet1);
            meetsList.Add(meet2);
            this.repository
                .Setup(mock => mock.ListAll())
                .Returns(meetsList);
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(2);

            // Act
            var result = this.meetingActions.SelectMeeting();

            //Assert
            result.Should().Be(1);
            this.consoleHandler
                .Verify(mock => mock.Print(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        [DataRow("Ausra Lekaviciute")]
        public void InputFullName(string name)
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns(name);

            //Act
            var result = this.meetingActions.InputFullName();

            //Assert
            result
                .Should()
                .BeEquivalentTo(new Person
                {
                    FirstName = "Ausra",
                    LastName = "Lekaviciute"
                });
        }

        [TestMethod]
        public void FilterMeetings_ByDescription()
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("1");

            // Act
            this.meetingActions.FilterMeetings();

            // Assert
            this.meetingFilter
                .Verify(mock => mock.FilterByDescription(), Times.Once);
        }

        [TestMethod]
        public void FilterMeetings_ByResponsiblePerson()
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("2");

            // Act
            this.meetingActions.FilterMeetings();

            // Assert
            this.meetingFilter
                .Verify(mock => mock.FilterByResponsiblePerson(), Times.Once);
        }

        [TestMethod]
        public void FilterMeetings_ByCategory()
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("3");

            // Act
            this.meetingActions.FilterMeetings();

            // Assert
            this.meetingFilter
                .Verify(mock => mock.FilterByCategory(), Times.Once);
        }

        [TestMethod]
        public void FilterMeetings_ByType()
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("4");

            // Act
            this.meetingActions.FilterMeetings();

            // Assert
            this.meetingFilter
                .Verify(mock => mock.FilterByType(), Times.Once);
        }

        [TestMethod]
        public void FilterMeetings_ByDate()
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("5");

            // Act
            this.meetingActions.FilterMeetings();

            // Assert
            this.meetingFilter
                .Verify(mock => mock.FilterByDate(), Times.Once);
        }

        [TestMethod]
        public void FilterMeetings_ThrowsException()
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns("s");

            // Act
            Action filter = () => this.meetingActions.FilterMeetings();

            // Assert
            filter
                .Should()
                .Throw<Exception>();
        }
    }   
}
