using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using CliTest.TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliTest.Cmds;

[Description("Its time to go ...")]
internal sealed class Beam : Command<Beam.Settings>
{
    private readonly TheGame _game;

    public Beam(TheGame inj)
    {
        _game = inj;
    }

    public sealed class Settings : BaseSettings
    {
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.MarkupLineInterpolated($"{_game.Travel(settings.TalkALot)}");
        return 0;
    }
}
