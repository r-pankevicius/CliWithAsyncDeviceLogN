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
        private static readonly CommandsBuffer CommandsBuffer = new(maxSize: 30);

        public static async Task Main()
        {
            Console.WriteLine(
                "Use commands exit or quit to quit the program. (But Ctrl+C is more reliable.)");

            Timer timer = new(1000);
            timer.Elapsed += async (sender, e) => await OnTimerCallback();
            timer.Start();

            await RunAsync();
        }

        /// <summary>
        /// Runs command line interface until exit/quit command is entered.
        /// </summary>
        public static async Task RunAsync()
        {
            WriteCurrentCommandLine();

            bool shouldExit = false;

            while (!shouldExit)
            {
                if (Console.KeyAvailable)
                {
                    // intercept: true will not display the key entered
                    ConsoleKeyInfo consoleKey = Console.ReadKey(intercept: true);
                    shouldExit = await ProcessKeyAsync(consoleKey);
                }
            }
        }

        /// <summary>
        /// Processes the key typed on the console.
        /// </summary>
        /// <param name="consoleKey">Console key pressed</param>
        /// <returns>true if terminal should exit, false if not.</returns>
        private static async Task<bool> ProcessKeyAsync(ConsoleKeyInfo consoleKey)
        {
            static void OnPreviousOrNext(string previousOrNext)
            {
                if (!string.IsNullOrEmpty(previousOrNext))
                {
                    ClearCommandLine();
                    CommandLineBuffer.Clear();
                    CommandLineBuffer.AddRange(previousOrNext);
                    WriteCurrentCommandLine();
                }
            }

            if (consoleKey.Key == ConsoleKey.Enter)
            {
                string commandLine = GetAndClearCommandLine();
                return await ExecuteCommandAsync(commandLine);
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
            else if (consoleKey.Key == ConsoleKey.UpArrow)
            {
                OnPreviousOrNext(CommandsBuffer.Previous());
            }
            else if (consoleKey.Key == ConsoleKey.DownArrow)
            {
                OnPreviousOrNext(CommandsBuffer.Next());
            }
            else if (consoleKey.KeyChar != 0)
            {
                AddCommandLineChar(consoleKey.KeyChar);
            }

            return false;
        }

        private static string GetAndClearCommandLine()
        {
            string line = LineBufferToString();
            CommandLineBuffer.Clear();
            return line;
        }

        private static void AddCommandLineChar(char ch)
        {
            CommandLineBuffer.Add(ch);
            WriteCurrentCommandLine();
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

        /// <summary>
        /// Executes command line <paramref name="commandLine"/>.
        /// </summary>
        /// <param name="commandLine">Command line, what user has entered</param>
        /// <returns>true if should exit (i.e. user typed exit command). false otherwise.</returns>
        private static Task<bool> ExecuteCommandAsync(string commandLine)
        {
            Console.WriteLine();

            // Simulate execution
            Console.WriteLine("...Executing command {0}", commandLine);
            CommandsBuffer.Add(commandLine);

            bool shouldExit =
                "exit".Equals(commandLine, StringComparison.OrdinalIgnoreCase) ||
                "quit".Equals(commandLine, StringComparison.OrdinalIgnoreCase);
            return Task.FromResult(shouldExit);
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