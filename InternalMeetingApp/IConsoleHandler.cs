using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalMeetingApp
{
    public interface IConsoleHandler
    {
        string AskForString(string text);
        int AskForInt(string text);
        DateTime AskForDate(string text);
        void Print(params string[] texts);
        void Notify(string text);
    }
}
