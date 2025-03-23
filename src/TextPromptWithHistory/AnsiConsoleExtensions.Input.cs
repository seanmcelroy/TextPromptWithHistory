using Spectre.Console;

namespace TextPromptWithHistory;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static class AnsiConsoleExtensions
{
    internal static async Task<string> ReadLine(this IAnsiConsole console, Style? style, bool secret, char? mask, IEnumerable<string>? items = null, IEnumerable<string>? history = null, CancellationToken cancellationToken = default)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        style ??= Style.Plain;
        var text = string.Empty;
        var lineIndex = 0;

        var autocomplete = new List<string>(items ?? Enumerable.Empty<string>());
        var historyIndex = -1;
        string? prehistorySaved = null;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
            if (rawKey == null)
            {
                continue;
            }

            var key = rawKey.Value;
            if (key.Key == ConsoleKey.Enter)
            {
                return text;
            }

            if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
            {
                var autoCompleteDirection = key.Modifiers.HasFlag(ConsoleModifiers.Shift)
                    ? AutoCompleteDirection.Backward
                    : AutoCompleteDirection.Forward;
                var replace = AutoComplete(autocomplete, text, autoCompleteDirection);
                if (!string.IsNullOrEmpty(replace))
                {
                    // Render the suggestion
                    console.Write("\b \b".Repeat(text.Length), style);
                    console.Write(replace);
                    text = replace;
                    continue;
                }
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

            if (key.Key == ConsoleKey.Delete)
            {
                if (text.Length > lineIndex)
                {
                    char charToDelete = text[lineIndex];
                    string tail = text.Substring(lineIndex + 1);

                    text = text.Substring(0, lineIndex) + tail;

                    if (UnicodeCalculator.GetWidth(charToDelete) == 2)
                    {
                        tail += "  ";
                    }
                    else if (UnicodeCalculator.GetWidth(charToDelete) == 1)
                    {
                        tail += " ";
                    }

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

                    if (UnicodeCalculator.GetWidth(charToDelete) == 2)
                    {
                        console.Cursor.MoveLeft();
                        console.Cursor.MoveLeft();
                        tail += "  ";
                    }
                    else if (UnicodeCalculator.GetWidth(charToDelete) == 1)
                    {
                        console.Cursor.MoveLeft();
                        tail += " ";
                    }

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
                console.Write(secret ? output.Mask(mask) : output, style);
                console.Write(secret ? text.Substring(lineIndex).Mask(mask) : text.Substring(lineIndex), style);
                AnsiConsole.Cursor.MoveLeft(text.Substring(lineIndex).Length);
            }
        }
    }

    private static string AutoComplete(List<string> autocomplete, string text, AutoCompleteDirection autoCompleteDirection)
    {
        var found = autocomplete.Find(i => i == text);
        var replace = string.Empty;

        if (found == null)
        {
            // Get the closest match
            var next = autocomplete.Find(i => i.StartsWith(text, true, CultureInfo.InvariantCulture));
            if (next != null)
            {
                replace = next;
            }
            else if (string.IsNullOrEmpty(text))
            {
                // Use the first item
                replace = autocomplete[0];
            }
        }
        else
        {
            // Get the next match
            replace = GetAutocompleteValue(autoCompleteDirection, autocomplete, found);
        }

        return replace;
    }

    private static string GetAutocompleteValue(AutoCompleteDirection autoCompleteDirection, IList<string> autocomplete, string found)
    {
        var foundAutocompleteIndex = autocomplete.IndexOf(found);
        var index = autoCompleteDirection switch
        {
            AutoCompleteDirection.Forward => foundAutocompleteIndex + 1,
            AutoCompleteDirection.Backward => foundAutocompleteIndex - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(autoCompleteDirection), autoCompleteDirection, null),
        };

        if (index >= autocomplete.Count)
        {
            index = 0;
        }

        if (index < 0)
        {
            index = autocomplete.Count - 1;
        }

        return autocomplete[index];
    }

    private enum AutoCompleteDirection
    {
        Forward,
        Backward,
    }
}