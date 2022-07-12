using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalMeetingApp
{
    public class Atendee
    {
        public Person Person { get; set; }
        public DateTime AdditionTime { get; set; }

        public Atendee()
        {
            Person = new Person();
        }
    }
}
