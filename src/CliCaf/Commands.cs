using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using ConsoleAppFramework;
using TinyStupidGame;

namespace CliCaf;
public class Commands
{
    public TheGame Game { get; set; } = new TheGame();

    public Commands(TheGame game)
    {
        Game = game;
    }

    /// <summary>
    ///     list of things.
    /// </summary>
    /// <param name="type">What thingys you want?</param>
    /// <param name="verbose">-v, if you want me to talk a lot.</param>
    /// <returns>async void.</returns>
    public void ListCmd([Argument] string type, bool verbose)
    {
        if (type.Equals("planets", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine(Game.ListPlanets(verbose));
        }
        else if (type.Equals("ships", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine(Game.ListShips(verbose));
        }
        else if (string.IsNullOrEmpty(type))
        {
            Console.WriteLine("What kind of things you are interessted in?");
        }
        else
        {
            Console.WriteLine($"I don't know much about {type}.");
        }

    }

    /// <summary>
    ///     select one out of a list of things.
    /// </summary>
    /// <param name="type">What thingies you want to select from?</param>
    /// <param name="name">Name the one you want to have.</param>
    /// <param name="verbose">-v, if you want me to talk a lot.</param>
    public void SelectCmd([Argument] string type, [Argument] string name, bool verbose)
    {
        if (string.Equals(type, "planet", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine(Game.SelectTarget(name ?? " ", verbose));
        }
        else if (string.Equals(type, "ship", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.WriteLine(Game.SelectShip(name ?? " ", verbose));
        }
        else if (string.IsNullOrEmpty(type))
        {
            Console.WriteLine("What kind of things you want to select from?");
        }
        else
        {
            Console.WriteLine("I don't know any {settings.ItemType}.");
        }
    }

    /// <summary>
    ///     Try to find out what this is all about.
    /// </summary>
    /// <param name="verbose">-v, if you want me to talk a lot.</param>
    public void LookCmd(bool verbose)
    {
        Console.WriteLine(Game.WhereAmI(verbose));
    }


    static string result = "";
    /// <summary>
    ///     Let's go to new adventures.
    /// </summary>
    /// <param name="verbose">-v, if you want me to talk a lot.</param>
    public void BeamCmd(bool verbose)
    {
        result = "";
        IProgress<string> progress = new Progress<string>(CollectProgress);
        var t = Game.TravelAsync(verbose, progress);
        t.Wait();
        Console.WriteLine(result);

    }

    private void CollectProgress(string obj)
    {
        result += obj;
    }


    private static void ReportProgress(string obj)
    {
        Console.Write(obj);
    }

    /// <summary>
    ///     Let's go to new adventures.
    /// </summary>
    /// <param name="verbose">-v, if you want me to talk a lot.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task BeamAsync(bool verbose)
    {
        IProgress<string> progress = new Progress<string>(ReportProgress);
        return Game.TravelAsync(verbose, progress);
    }
}
