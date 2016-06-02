using System;

namespace Restaurant
{
    public class ConsoleHorn : IHorn
    {
        public void Whisper(string message)
        {
            UsingColor(ConsoleColor.Gray, () => Console.WriteLine(message));
        }

        public void Say(string message)
        {
            UsingColor(ConsoleColor.White, () => Console.WriteLine(message));
        }

        public void Note(string message)
        {
            UsingColor(ConsoleColor.Green, () => Console.WriteLine(message));
        }

        private void UsingColor(ConsoleColor color, Action action)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = previousColor;
        }
    }
}
