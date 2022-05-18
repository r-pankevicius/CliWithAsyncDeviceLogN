using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

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

            Timer timer = new(1000);
            timer.Elapsed += async (sender, e) => await OnTimerCallback();
            timer.Start();

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
                    else if (consoleKey.Key == ConsoleKey.Backspace)
                    {
                        if (CommandLineBuffer.Count > 0)
                        {
                            // clear to remove last command line char (CommandLineBuffer will be truncated by 1 char)
                            ClearCommandLine();
                            CommandLineBuffer.RemoveAt(CommandLineBuffer.Count - 1);
                            WriteCurrentCommandLine();
                        }
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
            ClearCommandLine();
            string commandLine = GetCurrentCommandLine();
            Console.Write(commandLine);
        }

        /// <summary>
        /// Clears all input line with "carriage return".
        /// </summary>
        private static void ClearCommandLine()
        {
            // Don't leave mess, clear current command line with spaces
            Console.Write($"\r{new string(' ', GetCurrentCommandLine().Length)}\r");
        }

        private static string GetCurrentCommandLine() =>
            string.Concat(CommandPromptPrefix, LineBufferToString());

        private static string LineBufferToString() => string.Join("", CommandLineBuffer);

        private static void ExecuteCommand(string line, out bool shouldExit)
        {
            Console.WriteLine();
            Console.WriteLine("...Executing command {0}", line);
            shouldExit =
                "exit".Equals(line, StringComparison.OrdinalIgnoreCase) ||
                "quit".Equals(line, StringComparison.OrdinalIgnoreCase);
        }

        public static Task OnTimerCallback()
        {
            ClearCommandLine();
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("OnTimerCallback");
            Console.ForegroundColor = foregroundColor;
            WriteCurrentCommandLine();
            return Task.CompletedTask;
        }
    }
}