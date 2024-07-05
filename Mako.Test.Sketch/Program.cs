using Mako.Ui.Sketch;

Sketch.Create(win =>
{
    win.ControlPanel.AddButton("Sin", () =>
    {
        var sine = new List<Point>();
        for (var i = 0; i <360; i++)
        {
            sine.Add(new Point(i, Math.Sin(2 * Math.PI * i / 360)));
        }
        win.Log(Plot.Create().AddLine(sine));
    });
});
