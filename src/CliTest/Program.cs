using CliTest.Cmds;
using CliTest.TinyStupidGame;
using Microsoft.Extensions.DependencyInjection;
using ShellExtension;
using Spectre.Console.Cli;

namespace CliTest;

internal static class Program
{
    public static void Main(string[] args)
    {
        var registrations = new ServiceCollection();
        registrations.AddSingleton<TheGame>(new TheGame());
        var registrar = new TypeRegistrar(registrations);
        registrar.Build();

        ICommandApp app = new CommandApp(registrar);
        app.Configure(config =>
        {
            config.AddCommand<Look>("look");
            config.AddCommand<List>("list");
            config.AddCommand<Select>("select");
            config.AddCommand<Beam>("beam");
#if DEBUG
            config.PropagateExceptions();
            config.ValidateExamples();
#endif
        });
        if (args.Length > 0)
        {
            // This is useless in this example but can be used for other real world cli apps to execute a single command..
            app.Run(args);
        }
        else
        {
            app.RunShell("tsg>");
        }
    }
}
