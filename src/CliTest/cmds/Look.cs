using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Spectre.Console;
using CliTest.TinyStupidGame;

namespace CliTest.cmds;

[Description("Try to find out whats going on....")]
internal sealed class Look : Command<Look.Settings>
{
    private readonly TheGame _game;

    public Look(TheGame inj)
    {
        _game = inj;
    }

    public sealed class Settings : CommandSettings
    {
        [CommandOption("-v|--verbose")]
        [DefaultValue(false)]
        [Description("Make it talk more.")]
        public bool TalkALot { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.MarkupLineInterpolated($"{_game.WhereAmI(settings.TalkALot)}");
        return 0;
    }
}
