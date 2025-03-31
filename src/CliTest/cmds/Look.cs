using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TinyStupidGame;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace CliTest.Cmds;

[Description("Try to find out whats going on....")]
internal sealed class Look : Command<Look.Settings>
{
    private readonly TheGame _game;
    private readonly IScreen _output;

    public Look(TheGame inj, IScreen content)
    {
        _game = inj;
        _output = content;
    }

    public sealed class Settings : BaseSettings
    {
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var res = _game.WhereAmI(settings.TalkALot);

        _output.WriteLine(res);

        //_content["bottom"].Update(
        //    new Panel(new Markup(res))
        //        .Expand());
        //AnsiConsole.Write(_content);
        //AnsiConsole.Cursor.SetPosition(10, 10);
        return 0;
    }
}
