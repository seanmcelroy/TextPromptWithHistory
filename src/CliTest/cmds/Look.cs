using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using CliTest.TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliTest.Cmds;

[Description("Try to find out whats going on....")]
internal sealed class Look : Command<Look.Settings>
{
    private readonly TheGame _game;

    public Look(TheGame inj)
    {
        _game = inj;
    }

    public sealed class Settings : BaseSettings
    {
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.MarkupLineInterpolated($"{_game.WhereAmI(settings.TalkALot)}");
        return 0;
    }
}
