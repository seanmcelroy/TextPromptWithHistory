# TextPromptWithHistory

A .NET library that makes it easier to create even more useful `TextPrompt`s in `Spectre.Console` by adding shell history super powers to support REPL environments.

## Example

```text
Command (arrow up/down for history)> _
```

### Usage

```csharp

// Manage a list of history items to provide for scrolling with up/down arrow keys
List<string> history = [];

// Use TextPrompt in a loop to implement a command line shell
do
{
    // Provide the history of items using the .AddHistory() extension method
    string? input = AnsiConsole.Prompt(
        new TextPrompt<string>("Command (arrow up/down for history)>").AddHistory(history));

    // Logic to determine when to add an item to the history
    if (!string.IsNullOrWhiteSpace(input))
        history.Add(input);

} while (true);
```

A simple REPL app is included in the `ReplTest` directory.

Detailed instructions for using `Spectre.Console` are located on their project website, https://spectreconsole.net