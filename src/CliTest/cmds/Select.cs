using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using CliTest.TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliTest.Cmds;

[Description("Choose one specific from list...")]
internal sealed class Select : Command<Select.Settings>
{
    private readonly TheGame _game;

    public Select(TheGame inj)
    {
        _game = inj;
    }

    public sealed class Settings : BaseSettings
    {
        [CommandArgument(0, "[itemType]")]
        [Description("What items you want to select from?")]
        public string? ItemType { get; set; }

        [CommandArgument(1, "[itemName]")]
        [Description("Name the one you want to select.")]
        public string? ItemName { get; set; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.ItemType?.ToLower() == "planet")
        {
            AnsiConsole.MarkupLineInterpolated($"{_game.SelectTarget(settings.ItemName ?? " ", settings.TalkALot)}");
        }
        else if (settings.ItemType?.ToLower() == "ship")
        {
            AnsiConsole.MarkupLineInterpolated($"{_game.SelectShip(settings.ItemName ?? " ", settings.TalkALot)}");
        }
        else if (string.IsNullOrEmpty(settings.ItemType))
        {
            AnsiConsole.MarkupLineInterpolated($"What kind of things you want to select from?");
        }
        else
        {
            AnsiConsole.MarkupLineInterpolated($"I don't know any {settings.ItemType}.");
        }

        return 0;
    }
}
