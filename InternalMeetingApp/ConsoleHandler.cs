using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalMeetingApp
{
    public class ConsoleHandler:IConsoleHandler
    {
        public string AskForString(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }

        public int AskForInt(string text)
        {
            return Convert.ToInt32(AskForString(text));
        }

        public DateTime AskForDate(string text)
        {
            return DateTime.Parse(AskForString(text));
        }

        public void Print(params string[] texts)
        {
            foreach (var text in texts)
            {
                Console.WriteLine(text);
            }
        }

        public void Notify(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
        }
    }
}
