using System.ComponentModel;
using Spectre.Console.Cli;

namespace CliTest.Cmds;
internal class BaseSettings : CommandSettings
{
    [CommandOption("-v|--verbose")]
    [DefaultValue(false)]
    [Description("Make it talk more.")]
    public bool TalkALot { get; init; }
}
