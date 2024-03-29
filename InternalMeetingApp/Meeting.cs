﻿namespace InternalMeetingApp
{
    public class Meeting
    {
        public string Name { get; set; }
        public Person ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MeetingCategory Category { get; set; }
        public MeetingType Type { get; set; }
        public List<Atendee> Atendees { get; set; }

        public Meeting()
        {
            this.Atendees = new List<Atendee>();
            ResponsiblePerson = new Person();
        }
    }
}