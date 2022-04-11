namespace CliWithAsyncDeviceLogN
{
    internal static class Program
    {
        private static readonly List<char> LineBuffer = new();

        public static void Main()
        {
            using var _ = new Timer(OnTimerCallback, 1, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            bool shouldExit = false;

            while (!shouldExit)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo consoleKey = Console.ReadKey(true);
                    if (consoleKey.Key == ConsoleKey.Enter)
                    {
                        string line = LineBufferToString();
                        LineBuffer.Clear();
                        Console.WriteLine(line);
                        ExecuteCommand(line, out shouldExit);
                    }
                    else if (consoleKey.KeyChar != 0)
                    {
                        LineBuffer.Add(consoleKey.KeyChar);
                        Console.Write("\r");
                        Console.Write(LineBufferToString());
                    }
                }
            }
        }

        private static string LineBufferToString() => string.Join("", LineBuffer);

        private static void ExecuteCommand(string line, out bool shouldExit)
        {
            Console.WriteLine("...Executing command {0}", line);
            shouldExit =
                "exit".Equals(line, StringComparison.OrdinalIgnoreCase) ||
                "quit".Equals(line, StringComparison.OrdinalIgnoreCase);
        }

        public static void OnTimerCallback(object? state)
        {
            Console.Write("\r"); // clear all input line with "carriage return"
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("OnTimerCallback");
            Console.ForegroundColor = foregroundColor;
            Console.Write("\r");
            Console.Write(LineBufferToString());
        }
    }
}
