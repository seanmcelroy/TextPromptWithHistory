using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliTest.Cmds;

[Description("See whats on the menu ...")]
internal sealed class List : Command<List.Settings>
{
    private readonly TheGame _game;
    private readonly IScreen _output;

    public List(TheGame inj, IScreen scr)
    {
        _game = inj;
        _output = scr;
    }

    public sealed class Settings : BaseSettings
    {
        [CommandArgument(0, "[searchItem]")]
        [DefaultValue(null)]
        [Description("What items you want to explore?")]
        public string? ItemType { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.ItemType?.ToLower() == "planets")
        {
            _output.WriteLine(_game.ListPlanets(settings.TalkALot));
        }
        else if (settings.ItemType?.ToLower() == "ships")
        {
            _output.WriteLine(_game.ListShips(settings.TalkALot));
        }
        else if (string.IsNullOrEmpty(settings.ItemType))
        {
            _output.WriteLine("What kind of things you are interessted in?");
        }
        else
        {
            _output.WriteLine($"I don't know much about {settings.ItemType}.");
        }
        return 0;
    }
}