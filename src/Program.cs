namespace CliWithAsyncDeviceLogN
{
    internal static class Program
    {
        private const string CommandPromptPrefix = "$ ";

        private static readonly List<char> CommandLineBuffer = new();

        public static void Main()
        {
            Console.WriteLine("Use commands exit or quit to quit the program.");
            WriteCurrentCommandLine();

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
                        CommandLineBuffer.Clear();
                        ExecuteCommand(line, out shouldExit);
                    }
                    else if (consoleKey.KeyChar != 0)
                    {
                        CommandLineBuffer.Add(consoleKey.KeyChar);
                        WriteCurrentCommandLine();
                    }
                }
            }
        }

        private static void WriteCurrentCommandLine()
        {
            string commandLine = GetCurrentCommandLine();
            Console.Write("\r");
            Console.Write(commandLine);
        }

        private static string GetCurrentCommandLine() => string.Concat(CommandPromptPrefix, LineBufferToString());

        private static string LineBufferToString() => string.Join("", CommandLineBuffer);

        private static void ExecuteCommand(string line, out bool shouldExit)
        {
            Console.WriteLine();
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
