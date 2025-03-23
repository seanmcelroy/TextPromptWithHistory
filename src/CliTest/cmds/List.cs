using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Spectre.Console;
using CliTest.TinyStupidGame;

namespace CliTest.cmds;

[Description("See whats on the menu ...")]
internal sealed class List : Command<List.Settings>
{
    private readonly TheGame _game;

    public List(TheGame inj)
    {
        _game = inj;
    }

    public sealed class Settings : CommandSettings
    {
        [CommandOption("-v|--verbose")]
        [DefaultValue(false)]
        [Description("Make it talk more more")]
        public bool TalkALot { get; init; }

        [CommandArgument(0, "[searchItem]")]
        [DefaultValue(null)]
        [Description("What items you want to explore?")]
        public string? ItemType { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.ItemType?.ToLower() == "planets")
        {
            AnsiConsole.MarkupLineInterpolated($"{_game.ListPlanets(settings.TalkALot)}");
        }
        else if (settings.ItemType?.ToLower() == "ships")
        {
            AnsiConsole.MarkupLineInterpolated($"{_game.ListShips(settings.TalkALot)}");
        }
        else if (string.IsNullOrEmpty(settings.ItemType))
        {
            AnsiConsole.MarkupLineInterpolated($"What kind of things you are interessted in?");
        } else
        {
            AnsiConsole.MarkupLineInterpolated($"I don't know much about {settings.ItemType}.");
        }
        return 0;
    }
}


