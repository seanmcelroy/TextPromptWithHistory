using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace CliTest;
public class SpectreScreen : IScreen
{
    private string content = string.Empty;
    private Layout rootLayout;

    public SpectreScreen()
    {
        rootLayout = new Layout("Root")
                .SplitRows(
                     new Layout("Top"),
                     new Layout(new Markup("")).Size(2));
    }


    public void Clear()
    {
        content = string.Empty;
        rootLayout["Top"].Update(new Panel(new Markup(Markup.Escape(content))).Expand());
        Refresh();
    }
    public void WriteLine(string text)
    {
        content += text + Environment.NewLine;
        rootLayout["Top"].Update(new Panel(new Markup(Markup.Escape(content))).Expand());
        Refresh();
    }

    internal void Refresh()
    {
        AnsiConsole.Write(rootLayout);
        int y = Console.CursorTop;
        AnsiConsole.Cursor.SetPosition(0, y);
    }
}
