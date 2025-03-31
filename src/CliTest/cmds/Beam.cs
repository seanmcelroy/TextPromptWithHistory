using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CliTest.Cmds;

[Description("Its time to go ...")]
internal sealed class Beam : Command<Beam.Settings>
{
    private readonly TheGame _game;
    private readonly IScreen _output;
    public Beam(TheGame inj, IScreen content)
    {
        _game = inj;
        _output = content;
    }

    public sealed class Settings : BaseSettings
    {
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        IProgress<string> progress = new Progress<string>(ReportProgress);
        _ = _game.TravelAsync(settings.TalkALot, progress);
        return 0;
    }

    private void ReportProgress(string message)
    {
        _output.WriteLine(message);
        // AnsiConsole.MarkupLineInterpolated($"{message}");
    }
}
