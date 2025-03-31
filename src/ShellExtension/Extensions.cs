using Spectre.Console;
using Spectre.Console.Cli;

namespace ShellExtension;

/// <summary>
///     Exctension methods needed to provide a 'mini shell' to a confuigured Spectre.Console.Cli.CommandApp.
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Extension method for Spectre.Console.Cli to run the app in a 'mini shell'.
    /// </summary>
    /// <param name="app">The instance of the CommandApp.</param>
    /// <param name="prompt">the text prompt ro be shown.</param>
    /// <param name="exitCmd">the exit command to leave the shell.</param>
    public static void RunShell(this ICommandApp app, string prompt = ">", string exitCmd = "exit")
    {
        while (true)
        {
            AnsiConsole.MarkupInterpolated($"{prompt}");
            var lineT = Console.In.ReadLineAsync(); // To support Async commands this does not block here!
                                                    // The line editing is done by Console.In including a history list of typed lines!!!


            if (lineT.IsCompletedSuccessfully)
            {
                var line = lineT.Result;
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.Trim().ToLower().StartsWith(exitCmd))
                    {
                        break;
                    }

                    if (new List<string>() { "-?", "?", "help", "-help", "--help" }.Contains(line.Trim().ToLower()))
                    {
                        line = "-h";
                        AnsiConsole.MarkupLineInterpolated($"Type '{exitCmd}' to exit this shell.");
                    }

                    try
                    {
                        string[] largs = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                        if (largs.Length == 2)
                        {
                            if (largs[0].Equals("help", StringComparison.InvariantCultureIgnoreCase))
                            {
                                largs[0] = largs[1];
                                largs[1] = "-h";
                            }
                        }

                        app.Run(largs);
                    }
                    catch (Exception cpe)
                    {
                        AnsiConsole.WriteException(cpe);
                    }
                }
            }
            else
            {
                // avoid heavy cpu load when polling for input line.
                var t = Task.Delay(10);
                t.Wait();
            }
        }
    }

    private static string ReadLine(IEnumerable<string> history, CancellationToken cancellationToken = default)
    {
        IAnsiConsole console = AnsiConsole.Console;
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        Style style = Style.Plain;
        var text = string.Empty;
        var lineIndex = 0;

        //var autocomplete = new List<string>(items ?? Enumerable.Empty<string>());
        var autocomplete = new List<string>(Enumerable.Empty<string>());
        var historyIndex = -1;
        string? prehistorySaved = null;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var rawKey = console.Input.ReadKey(true); //, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

            if (rawKey == null)
            {
                continue;
            }

            var key = rawKey.Value;
            if (key.Key == ConsoleKey.Enter)
            {
                console.WriteLine();
                return text;
            }

            if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
            {
                //var autoCompleteDirection = key.Modifiers.HasFlag(ConsoleModifiers.Shift)
                //    ? AutoCompleteDirection.Backward
                //    : AutoCompleteDirection.Forward;
                //var replace = AutoComplete(autocomplete, text, autoCompleteDirection);
                //if (!string.IsNullOrEmpty(replace))
                //{
                //    // Render the suggestion
                //    console.Write("\b \b".Repeat(text.Length), style);
                //    console.Write(replace);
                //    text = replace;
                //    continue;
                //}
            }

            if (key.Key == ConsoleKey.LeftArrow)
            {
                if (lineIndex > 0)
                {
                    console.Cursor.MoveLeft();
                    lineIndex--;
                }

                continue;
            }

            if (key.Key == ConsoleKey.RightArrow)
            {
                if (text.Length > lineIndex)
                {
                    console.Cursor.MoveRight();
                    lineIndex++;
                }

                continue;
            }

            if (key.Key == ConsoleKey.Home)
            {
                if (lineIndex > 0)
                {
                    console.Cursor.MoveLeft(lineIndex);
                    lineIndex = 0;
                }

                continue;
            }

            if (key.Key == ConsoleKey.End)
            {
                if (text.Length > lineIndex)
                {
                    console.Cursor.MoveRight(text.Length - lineIndex);
                    lineIndex = text.Length;
                }

                continue;
            }

            if (key.Key == ConsoleKey.Delete)
            {
                if (text.Length > lineIndex)
                {
                    char charToDelete = text[lineIndex];
                    string tail = text.Substring(lineIndex + 1);

                    text = text.Substring(0, lineIndex) + tail;

                    //if (UnicodeCalculator.GetWidth(charToDelete) == 2)
                    //{
                    //    tail += "  ";
                    //}
                    //else if (UnicodeCalculator.GetWidth(charToDelete) == 1)
                    //{
                    tail += " ";
                    //}

                    console.Write(tail);
                    console.Cursor.MoveLeft(tail.Length);
                }

                continue;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (lineIndex > 0)
                {
                    char charToDelete = text[lineIndex - 1];
                    string tail = string.Empty;
                    if (lineIndex < text.Length)
                    {
                        tail = text.Substring(lineIndex);
                    }

                    text = text.Substring(0, lineIndex - 1) + tail;

                    //if (UnicodeCalculator.GetWidth(charToDelete) == 2)
                    //{
                    //    console.Cursor.MoveLeft();
                    //    console.Cursor.MoveLeft();
                    //    tail += "  ";
                    //}
                    //else if (UnicodeCalculator.GetWidth(charToDelete) == 1)
                    //{
                    console.Cursor.MoveLeft();
                    tail += " ";
                    //}

                    console.Write(tail);
                    console.Cursor.MoveLeft(tail.Length);
                    lineIndex--;
                }

                continue;
            }

            var historyCount = history?.Count() ?? 0;
            if (key.Key == ConsoleKey.UpArrow && historyCount > 0 && historyIndex < historyCount)
            {
                console.Cursor.MoveLeft(text.Length);
                console.Write(" ".Repeat(text.Length));
                console.Cursor.MoveLeft(text.Length);

                historyIndex++;
                if (historyIndex > historyCount)
                {
                    historyIndex = historyCount;
                }
                else if (historyIndex == 0)
                {
                    prehistorySaved = text;
                }

                var prev = history!.Reverse().Skip(historyIndex).Take(1).FirstOrDefault();
                if (prev != null)
                {
                    text = prev ?? string.Empty;
                    console.Write(text);
                }
                else
                {
                    text = string.Empty;
                }

                lineIndex = text.Length;
                continue;
            }

            if (key.Key == ConsoleKey.DownArrow && historyCount > 0 && historyIndex > -1)
            {
                // Erase what is there
                console.Cursor.MoveLeft(text.Length);
                console.Write(" ".Repeat(text.Length));
                console.Cursor.MoveLeft(text.Length);

                historyIndex--;

                if (historyIndex == -1)
                {
                    text = prehistorySaved ?? string.Empty;
                }
                else
                {
                    text = history!.Reverse().Skip(historyIndex).Take(1).FirstOrDefault() ?? string.Empty;
                }

                console.Write(text);
                lineIndex = text.Length;

                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                text = text.Insert(lineIndex, key.KeyChar.ToString());
                lineIndex++;

                // text += key.KeyChar.ToString();
                var output = key.KeyChar.ToString();
                console.Write(output, style);
                console.Write(text.Substring(lineIndex), style);
                AnsiConsole.Cursor.MoveLeft(text.Substring(lineIndex).Length);
            }
        }
    }

    internal static string Repeat(this string text, int count)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (count <= 0)
        {
            return string.Empty;
        }

        if (count == 1)
        {
            return text;
        }

        return string.Concat(Enumerable.Repeat(text, count));
    }
}

