using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Spectre.Console;
using CliTest.TinyStupidGame;

namespace CliTest.cmds;

[Description("Its time to go ...")]
internal sealed class Beam : Command<Beam.Settings>
{
    private readonly TheGame _game;

    public Beam(TheGame inj)
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
        AnsiConsole.MarkupLineInterpolated($"{_game.Travel(settings.TalkALot)}");
        return 0;
    }
}
