using CliTest.Cmds;
using TinyStupidGame;
using Microsoft.Extensions.DependencyInjection;
using ShellExtension;
using Spectre.Console.Cli;
using Spectre.Console;

namespace CliTest;

internal static class Program
{
    public static void Main(string[] args)
    {
        // The Textual user Interface which is used from Commands to output the results.
        SpectreScreen screen = new SpectreScreen();

        var registrations = new ServiceCollection();
        registrations.AddSingleton<TheGame>(new TheGame());
        registrations.AddSingleton<IScreen>(screen);
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
            // Create the layout and sets the cursor to input position.
            screen.Refresh();

            // This is used if you really want a async readLine() !!!!!
            using var inputReader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);

            while (true)
            {
                AnsiConsole.MarkupInterpolated($">");

                // var lineT = Console.In.ReadLineAsync();   // This is not really async!!!! see: https://stackoverflow.com/a/79005703 
                var lineT = inputReader.ReadLineAsync();
                while (!lineT.IsCompletedSuccessfully)
                {
                    // avoid heavy cpu load when polling for input line.
                    Task.Delay(100).Wait();
                }

                if (lineT.IsCompletedSuccessfully)
                {
                    var line = lineT.Result;
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Trim().StartsWith("exit", StringComparison.CurrentCultureIgnoreCase))
                        {
                            break;
                        }

                        if (line.Trim().StartsWith("clr", StringComparison.CurrentCultureIgnoreCase))
                        {
                            screen.Clear();
                            continue;
                        }

                        if (new List<string>() { "-?", "?", "help", "-help", "--help" }.Contains(line.Trim().ToLower()))
                        {
                            line = "-h";
                            screen.WriteLine($"Type 'exit' to exit this shell.");
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

                            app.Run(largs);
                        }
                        catch (Exception cpe)
                        {
                            AnsiConsole.WriteException(cpe);
                        }
                    }
                    else
                    {
                        screen.Refresh();
                    }
                }
            }
        }
    }

    // app.RunShell("tsg>");
}


