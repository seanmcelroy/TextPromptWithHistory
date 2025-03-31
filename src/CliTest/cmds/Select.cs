using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliTest.Cmds;

[Description("Choose one specific from list...")]
internal sealed class Select : Command<Select.Settings>
{
    private readonly TheGame _game;
    private readonly IScreen _output;

    public Select(TheGame inj, IScreen output)
    {
        _game = inj;
        _output = output;
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
            _output.WriteLine($"{_game.SelectTarget(settings.ItemName ?? " ", settings.TalkALot)}");
        }
        else if (settings.ItemType?.ToLower() == "ship")
        {
            _output.WriteLine($"{_game.SelectShip(settings.ItemName ?? " ", settings.TalkALot)}");
        }
        else if (string.IsNullOrEmpty(settings.ItemType))
        {
            _output.WriteLine($"What kind of things you want to select from?");
        }
        else
        {
            _output.WriteLine($"I don't know any {settings.ItemType}.");
        }

        return 0;
    }
}
