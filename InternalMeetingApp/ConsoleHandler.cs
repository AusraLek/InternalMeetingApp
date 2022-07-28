namespace InternalMeetingApp
{
    public class ConsoleHandler:IConsoleHandler
    {
        public string AskForString(string text)
        {
            string value = null;
            while (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine(text);
                value = Console.ReadLine();
            }
            return value;
        }

        public int AskForInt(string text)
        {
            while (true)
            {
                if (int.TryParse(AskForString(text), out var value))
                {
                    return value;
                }
            }
        }

        public DateTime AskForDate(string text)
        {
            while (true)
            {
                if (DateTime.TryParse(AskForString(text), out var value))
                {
                    return value;
                }
            }
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
