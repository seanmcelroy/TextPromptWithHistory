using Spectre.Console;
using TextPromptWithHistory;

internal static class Program
{
    private static int Main()
    {
        AnsiConsole.MarkupLine("[bold fuchsia]spectre.console textprompt-history test harness[/]");

        List<string> history = [];

        while (true)
        {
            var input = AnsiConsole.Prompt(new TextPromptWithHistory<string>("[green]>[/]").AddHistory(history));
            if (!string.IsNullOrWhiteSpace(input))
            {
                history.Add(input);
            }
        }
    }
}