using HelixToolkit.Wpf;
using Mako.Ui.Sketch;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;


Sketch.Create(win =>
{
    double roll = 0;
    double pitch = 0;
    double yaw = 0;

/* win.ControlPanel.AddButton("Sin", () =>
{
    var sine = new List<Point>();
    for (var i = 0; i <360; i++)
    {
        sine.Add(new Point(i, Math.Sin(2 * Math.PI * i / 360)));
    }
    win.Log(Plot.Create().AddLine(sine));
});*/

    var teapot = new CubeVisual3D();

    var view = Plot3D.Create().AddObject(teapot);
    view.ZoomExtents();

    void Update()
    {
        teapot.Rotate(pitch, yaw, roll);
    }

    win.ControlPanel.AddButton("Roll+", () =>
    {
        roll += 5;
        Update();
    });
    win.ControlPanel.AddButton("Roll-", () =>
    {
        roll -= 5;
        Update();
    });
    win.ControlPanel.AddButton("Yaw+", () =>
    {
        yaw += 5;
        Update();
    });
    win.ControlPanel.AddButton("Yaw-", () =>
    {
        yaw -= 5;
        Update();
    });
    win.ControlPanel.AddButton("Pitch+", () =>
    {
        pitch += 5;
        Update();
    });
    win.ControlPanel.AddButton("Pitch-", () =>
    {
        pitch -= 5;
        Update();
    });


    view.Height = 400;
    win.Log(view);
});
