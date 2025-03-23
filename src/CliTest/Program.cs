using CliTest.Cmds;
using CliTest.TinyStupidGame;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using TextPromptWithHistory;

namespace CliTest;

internal class Program
{
    private readonly CommandApp _app;

    public static void Main(string[] args)
    {
        var registrations = new ServiceCollection();
        registrations.AddSingleton<TheGame>(new TheGame());
        var registrar = new TypeRegistrar(registrations);
        registrar.Build();

        var app = new CommandApp(registrar);
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

        Program p = new Program(app);
        p.RunShell("tsg>");
    }

    public Program(CommandApp app)
    {
        _app = app;
    }

    // This could also go into a CommandApp-Extension....
    private void RunShell(string prompt = ">", string exitCmd = "exit")
    {
        List<string> lineBuffer = new();
        while (true)
        {
            var line = AnsiConsole.Prompt(new TextPromptWithHistory<string>(prompt).AllowEmpty().AddHistory(lineBuffer));
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Trim().ToLower().StartsWith(exitCmd))
                {
                    break;
                }

                lineBuffer.Add(line);
                if (new List<string>() { "-?", "?", "help", "-help", "--help" }.Contains(line.Trim().ToLower()))
                {
                    line = "-h";
                }

                try
                {
                    string[] largs = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    if (largs.Length == 2)
                    {
                        if (largs[0].Equals("help", StringComparison.InvariantCultureIgnoreCase))
                        {
                            largs[0] = largs[1];
                            largs[1] = "-h";
                        }
                    }

                    _app.Run(largs);
                }
                catch (Exception cpe)
                {
                    AnsiConsole.WriteException(cpe);
                }
            }
        }
    }
}
