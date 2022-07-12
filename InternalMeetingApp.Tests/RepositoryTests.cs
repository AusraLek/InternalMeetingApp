using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternalMeetingApp;
using System.Linq;

namespace InternalMeetingApp.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void AddMeeting()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            meeting.Name = "Meeto pavadinimas";

            // Act
            repository.Add(meeting);

            // Assert
            Assert.AreEqual(1, repository.ListAll().Count());
        }

        [TestMethod]
        public void DeleteMeetingWhenResponsiblePersonMatch()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            meeting.Name = "meeto neimas";
            var person = new Person();
            person.FirstName = "Ausra";
            person.LastName = "Lekaviciute";
            meeting.ResponsiblePerson = person;
            repository.Add(meeting);

            // Act
            repository.Delete(0, person);

            // Assert
            Assert.AreEqual(0, repository.ListAll().Count());
        }

        [TestMethod]
        public void ListAllMeetings()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            meeting.Name = "meeto neimas";
            repository.Add(meeting);
            var meeting2 = new Meeting();
            meeting2.Name = "kitas meeto neimas";
            repository.Add(meeting2);

            // Act
            var result = repository.ListAll();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void AddAtendee()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            repository.Add(meeting);
            var atendee = new Atendee();
            atendee.Person.FirstName = "Ausra";
            atendee.Person.LastName = "Lekaviciute";

            // Act
            repository.AddAtendee(0, atendee);

            // Assert
            var result = repository.ListAll();
            Assert.AreEqual(1, result.First().Atendees.Count);
        }

        [TestMethod]
        public void DeleteMeetingWhenPersonNotMatch()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            meeting.Name = "meeto neimas";
            repository.Add(meeting);
            var person = new Person();
            person.FirstName = "Ausra";
            person.LastName = "Lekaviciute";
            meeting.ResponsiblePerson.FirstName = "Nesusra";
            meeting.ResponsiblePerson.LastName = "Nelekaviciute";

            // Act
            repository.Delete(0, person);

            // Assert
            Assert.AreEqual(1, repository.ListAll().Count());
        }

        [TestMethod]
        public void AddAtendeeTwoTimes()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            repository.Add(meeting);
            var atendee = new Atendee();
            atendee.Person.FirstName = "Ausra";
            atendee.Person.LastName = "Lekaviciute";

            // Act
            repository.AddAtendee(0, atendee);
            repository.AddAtendee(0, atendee);

            // Assert
            var result = repository.ListAll();
            Assert.AreEqual(1, result.First().Atendees.Count);
        }

        [TestMethod]
        public void DeleteAtendee()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            var testAtendee = new Atendee();
            testAtendee.Person.FirstName = "Ausra";
            testAtendee.Person.LastName = "Lekaviciute";
            meeting.Atendees.Add(testAtendee);
            repository.Add(meeting);
            var atendee = new Person();
            atendee.FirstName = "Ausra";
            atendee.LastName = "Lekaviciute";

            // Act
            repository.DeleteAtendee(0, atendee);

            // Assert
            var result = repository.ListAll();
            Assert.AreEqual(0, result.First().Atendees.Count);
        }

        [TestMethod]
        public void DeleteAtendeeWhenNotExist()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            var testAtendee = new Atendee();
            testAtendee.Person.FirstName = "Ausra";
            testAtendee.Person.LastName = "Lekaviciute";
            meeting.Atendees.Add(testAtendee);
            repository.Add(meeting);
            var atendee = new Person();
            atendee.FirstName = "Vardenis";
            atendee.LastName = "Pavardenis";

            // Act
            repository.DeleteAtendee(0, atendee);

            // Assert
            var result = repository.ListAll();
            Assert.AreEqual(1, result.First().Atendees.Count);
        }

        [TestMethod]
        public void DeleteAtendeeWhenResponsiblePerson()
        {
            // Arrange
            var repository = new Repository();
            var meeting = new Meeting();
            meeting.ResponsiblePerson.FirstName = "Ausra";
            meeting.ResponsiblePerson.LastName = "Lekaviciute";
            var testAtendee = new Atendee();
            testAtendee.Person.FirstName = "Ausra";
            testAtendee.Person.LastName = "Lekaviciute";
            meeting.Atendees.Add(testAtendee);
            repository.Add(meeting);
            var atendee = new Person();
            atendee.FirstName = "Ausra";
            atendee.LastName = "Lekaviciute";

            // Act
            repository.DeleteAtendee(0, atendee);

            // Assert
            var result = repository.ListAll();
            Assert.AreEqual(1, result.First().Atendees.Count);
        }


    }
}