using System.ComponentModel.Design;
using ConsoleAppFramework;
using TinyStupidGame;

namespace CliCaf;

public class Program
{
    private Commands Cmd { get; set; }

    public Program(TheGame theGame)
    {
        Cmd = new Commands(theGame);
    }

    public static void Main(string[] args)
    {
        Program p = new Program(new TheGame());
        p.Run(args);
    }

    private void Run(string[] args)
    {
        var app = ConsoleApp.Create();
        app.Add("list", Cmd.ListCmd);
        app.Add("select", Cmd.SelectCmd);
        app.Add("look", Cmd.LookCmd);
        //// The sync version
        app.Add("go", Cmd.BeamCmd);
        //// The async version
        app.Add("beam", Cmd.BeamAsync);

        if (args.Length == 0)
        {
            var runShell = true;
            app.Add("exit", () => runShell = false);
            Console.Write("Cli> ");

            // This can be used if you really want a async readLine() ! (see : https://stackoverflow.com/a/79005703 )
            using var inputReader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);

            while (runShell)
            {
                var readLineT = inputReader.ReadLineAsync();
                while (!readLineT.IsCompleted)
                {
                    // Avoid heavy polling for async result.
                    Task.Delay(1000).Wait();
                }
                if (readLineT.IsCompleted)
                {
                    if (!string.IsNullOrEmpty(readLineT.Result))
                    {
                        var parameters = readLineT.Result.Trim().Split(" ");
                        _ = app.RunAsync(parameters);
                    }

                    Console.Write("Cli> ");
                }
            }
        }
        else
        {
            _ = app.RunAsync(args);
        }
    }
}
