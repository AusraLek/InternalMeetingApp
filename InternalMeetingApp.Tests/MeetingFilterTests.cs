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
   public class MeetingFilterTests
    {
        private readonly MeetingFilter meetingFilter;
        private readonly Mock<IRepository> repository;
        private readonly Mock<IConsoleHandler> consoleHandler;

        public MeetingFilterTests()
        {
            this.repository = new Mock<IRepository>();
            this.consoleHandler = new Mock<IConsoleHandler>();
            this.meetingFilter = new MeetingFilter(this.repository.Object, this.consoleHandler.Object);
        }

        [TestMethod]
        [DataRow(1, MeetingCategory.CodeMonkey)]
        [DataRow(2, MeetingCategory.Hub)]
        [DataRow(3, MeetingCategory.Short)]
        [DataRow(4, MeetingCategory.TeamBuilding)]
        public void FilterByCategory(int input, MeetingCategory expectedCategory)
        {
            // Arange
            var actualCategory = (MeetingCategory)0;
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(input);
            this.repository
                .Setup(mock => mock.MeetingByCategory(It.IsAny<MeetingCategory>()))
                .Callback<MeetingCategory>(category => actualCategory = category)
                .Returns(new List<Meeting>());

            // Act
            this.meetingFilter.FilterByCategory();

            // Assert
            this.repository
                .Verify(mock => mock.MeetingByCategory(It.IsAny<MeetingCategory>()), Times.Once);
            actualCategory
                .Should()
                .Be(expectedCategory);
        }

        [TestMethod]
        [DataRow("Ausra")]
        public void FilterByDescription(string input)
        {
            // Arange
            this.consoleHandler
                .Setup(mock => mock.AskForString(It.IsAny<string>()))
                .Returns(input);
            string actualDescription = null;
            this.repository
                .Setup(mock => mock.MeetingByDescription(It.IsAny<string>()))
                .Callback<string>(phrase => actualDescription = phrase);

            // Act
            this.meetingFilter.FilterByDescription();

            // Assert
            this.repository
                .Verify(mock => mock.MeetingByDescription(It.IsAny<string>()), Times.Once);
            actualDescription
                .Should()
                .Be(input);
        }

        [TestMethod]
        [DataRow("Ausra", "Lekaviciute")]
        public void FilterByResponsiblePerson(string firstName, string lastName)
        {
            // Arange
            this.consoleHandler
                .SetupSequence(mock => mock.AskForString(It.IsAny<string>()))
                .Returns(firstName)
                .Returns(lastName);

            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
            };
            Person actualResponsiblePerson = null;
            this.repository
                .Setup(mock => mock.MeetingByResponsiblePerson(It.IsAny<Person>()))
                .Callback<Person>(person => actualResponsiblePerson = person);

            // Act
            this.meetingFilter.FilterByResponsiblePerson();

            // Assert
            this.repository
                .Verify(mock => mock.MeetingByResponsiblePerson(It.IsAny<Person>()), Times.Once);
            actualResponsiblePerson
                .Should()
                .BeEquivalentTo(new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                });
        }

        [TestMethod]
        [DataRow(1, MeetingType.Live)]
        [DataRow(2, MeetingType.InPerson)]
        public void FilterByType(int input, MeetingType expectiedType)
        {
            // Arange
            var actualType = (MeetingType)0;
            this.consoleHandler
                .Setup(mock => mock.AskForInt(It.IsAny<string>()))
                .Returns(input);
            this.repository
                .Setup(mock => mock.MeetingByType(It.IsAny<MeetingType>()))
                .Callback<MeetingType>(type => actualType = type)
                .Returns(new List<Meeting>());

            // Act
            this.meetingFilter.FilterByType();

            // Assert
            this.repository
                .Verify(mock => mock.MeetingByType(It.IsAny<MeetingType>()), Times.Once);
            actualType
                .Should()
                .Be(expectiedType);
        }

        [TestMethod]
        public void FilterByDate()
        {
            // Arange
            var from = new DateTime(2022,07,07);
            var to = new DateTime(2022, 07, 08);
            var actualFrom = DateTime.MinValue;
            var actualTo = DateTime.MinValue;
            this.consoleHandler
                .SetupSequence(mock => mock.AskForDate(It.IsAny<String>()))
                .Returns(from)
                .Returns(to);
            this.repository
                .Setup(mock => mock.MeetingByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Callback<DateTime, DateTime>(
                (start, end) =>
                {
                    actualFrom = start;
                    actualTo = end;
                })
                .Returns(new List<Meeting>());

            // Act
            this.meetingFilter.FilterByDate();

            // Assert
            this.repository
                .Verify(mock => mock.MeetingByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            actualFrom
                .Should()
                .Be(from);
            actualTo
                .Should()
                .Be(to);
        }
   }
}
