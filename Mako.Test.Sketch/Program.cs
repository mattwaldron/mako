using HelixToolkit.Wpf;
using Mako.Ui.Sketch;
using System.Windows.Media.Media3D;

Sketch.Create(win =>
{
    var teapot = new CubeVisual3D();
    double theta = 0;
    double psi = 0;

    /* win.ControlPanel.AddButton("Sin", () =>
    {
        var sine = new List<Point>();
        for (var i = 0; i <360; i++)
        {
            sine.Add(new Point(i, Math.Sin(2 * Math.PI * i / 360)));
        }
        win.Log(Plot.Create().AddLine(sine));
    });*/

    var view = Plot3D.Create().AddMesh(teapot);

    void Update()
    {
        teapot.Rotate(theta, psi);
    }

    win.ControlPanel.AddButton("Theta+", () =>
    {
        theta += 0.03;
        Update();
    });
    win.ControlPanel.AddButton("Theta-", () =>
    {
        theta -= 0.03;
    });
    win.ControlPanel.AddButton("Psi+", () =>
    {
        psi += 0.03;
    });
    win.ControlPanel.AddButton("Psi-", () =>
    {
        psi -= 0.03;
    });

    
    view.Height = 400;
    win.Log(view);
    teapot.Rotate(theta, psi);
});
