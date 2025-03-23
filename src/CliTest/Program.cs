using CliTest.cmds;
using CliTest.TinyStupidGame;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using TextPromptWithHistory;

namespace CliTest;

internal class Program
{
    private readonly CommandApp _app;

    static void Main(string[] args)
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
        p.RunShell();
    }

    public Program(CommandApp app)
    {
        _app = app;
    }

    private void RunShell()
    {
        List<string> lineBuffer = new();
        string prompt = "tsg>";
        while (true)
        {
            var line = AnsiConsole.Prompt(new TextPromptWithHistory<String>(prompt).AllowEmpty().AddHistory(lineBuffer));
            if (!string.IsNullOrEmpty(line))
            {
                if (line.Trim().ToLower().StartsWith("exit"))
                {
                    break;
                }

                lineBuffer.Add(line);
                if ((new List<string>() { "-?", "?", "help", "-help", "--help" }).Contains(line.Trim().ToLower()))
                {
                    line = "-h";
                }
                try
                {
                    string[] largs = line.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    if (largs.Length == 2)
                    {
                        if (largs[0].ToLower() == "help")
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
