using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace CliTest.TinyStupidGame;
internal class TheGame
{
    private readonly List<string> _planets = new() { "Earth", "Jupiter", "Mars", "Venus" };
    private readonly List<string> _planetDict = new() { "mostly harmless.", "a gassy giant with lot of moons.", " the red one.", "Aphrodite, Inanna, thank god its Frigg, ... " };
    private readonly List<string> _ships = new() { "Orion", "Voyager", "Crudy", "Megablast", "MyCruiser" };

    private int _yourPositionIdx = 0;
    private Dictionary<int, int> _shipPosition = new()
    {
        { 0, 0 },
        { 1, 0 },
        { 2, 0 },
        { 3, 3 },
        { 4, 2 },
    };

    private string? _selectedTarget = null;
    private string? _selectedShip = null;


    public TheGame()
    {
        AnsiConsole.WriteLine($"TheGame constructed.");
    }

    internal string ListPlanets(bool talkALot)
    {
        string retVal = "";
        for (int i = 0; i < _planets.Count; i++)
        {
            retVal += FormatSelected(_planets[i], _selectedTarget);
            if (talkALot)
            {
                retVal += ", " + _planetDict[i];
            }

            retVal += Environment.NewLine;
        }

        return retVal;
    }

    internal string ListShips(bool talkALot)
    {
        string retVal;
        if (talkALot)
        {
            retVal = "This ships are known of (in the solar system): ";
            foreach (var ship in _ships)
            {
                retVal += Environment.NewLine + FormatSelected(ship, _selectedShip);
            }
        }
        else
        {
            retVal = "This are seen ships: ";
            var localshipsIdx = _shipPosition.Where(x => x.Value == _yourPositionIdx).Select(x => x.Key);
            foreach (var idx in localshipsIdx)
            {
                retVal += Environment.NewLine + FormatSelected(_ships[idx], _selectedShip);
            }
        }

        return retVal;
    }

    private string FormatSelected(string name, string? selected)
    {
        string retVal = "[";
        if (name.Equals(selected))
        {
            retVal += "*";
        } else
        {
            retVal += " ";
        }
        retVal += "] " + name;
        return retVal;
    }

    internal string SelectShip(string itemName, bool talkALot)
    {
        string retVal = "";
        if (_ships.Contains(itemName))
        {
            _selectedShip = itemName;
            retVal += talkALot ? "Your boarding pass for " + itemName + " is ready." : "done.";
        }
        else
        {
            _selectedShip = null;
            retVal += talkALot ? "I dont now any ship named " + itemName: "not done.";
        }

        return retVal;
    }

    internal string SelectTarget(string itemName, bool talkALot)
    {
        string retVal = "";
        if (_planets.Contains(itemName))
        {
            _selectedTarget = itemName;
            retVal += talkALot ? itemName + " selected as next voyage." : "done.";
        }
        else
        {
            _selectedTarget = null;
            retVal += talkALot ? itemName + " confuses me." : "not done.";
        }

        return retVal;
    }

    internal string WhereAmI(bool talkALot)
    {
        string retVal = "You are on " + _planets[_yourPositionIdx];
        if (talkALot)
        {
            var localshipsIdx = _shipPosition.Where(x => x.Value == _yourPositionIdx).Select(x => x.Key);
            retVal += Environment.NewLine + "You can see ";
            foreach (var idx in localshipsIdx)
            {
                retVal += _ships[idx] + " ";
            }

            retVal += "in orbit.";

            if (!string.IsNullOrEmpty(_selectedTarget))
            {
                retVal += Environment.NewLine + "You can hardly wait to leave for " + _selectedTarget + ".";
            }
            if (!string.IsNullOrEmpty(_selectedShip))
            {
                retVal += Environment.NewLine + _selectedShip + " really anticipates your boarding.";
            }
        }

        return retVal;
    }

    internal string Travel(bool talkALot)
    {
        string retVal = " ";
        if (string.IsNullOrEmpty(_selectedTarget) ||
            string.IsNullOrEmpty(_selectedShip))
        {
            if (talkALot)
            {
                retVal = "Maybe you should consider a desired destination. Also some thoughts about how to get there would help....";
            }
            else
            {
                retVal = "You can't.";
            }
        }
        else
        {
            if (_planets[_yourPositionIdx] == _selectedTarget)
            {
                retVal = talkALot ? "I think to stay at home and keep your live boring you better type 'exit' ..." : "That's ridiculous.";
            }
            else
            {
                _yourPositionIdx = _planets.FindIndex(x => x.Equals(_selectedTarget));
                int shipIdx = _ships.FindIndex(x => x.Equals(_selectedShip));
                _shipPosition[shipIdx] = _yourPositionIdx;

                AnsiConsole.Status()
                .Start(talkALot ? $"You enter orbit to board {_selectedShip}..." : $".", ctx =>
                {
                    // Simulate some work
                    AnsiConsole.MarkupLine(talkALot ? $"Enter {_selectedShip}..." : $".");
                    Thread.Sleep(1000);

                    // Update the status and spinner
                    ctx.Status(talkALot ? "Accelerating..." : $".");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    // Simulate some work
                    AnsiConsole.MarkupLine(talkALot ? $"Heading of towards {_selectedTarget}..." : $".");
                    Thread.Sleep(2000);
                });

                _selectedTarget = null;
                retVal = talkALot ? "After an exiting journey finally you reached your desired target." : "arrived";
            }
        }

        return retVal;
    }
}
