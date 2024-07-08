using HelixToolkit.Wpf;
using Mako.Ui.Sketch;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;


Sketch.Create(win =>
{
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

    var thetaRot = new AxisAngleRotation3D(new Vector3D(0, 0, 1), theta);
    var psiRot = new AxisAngleRotation3D(new Vector3D(0, 1, 0), psi);
    var combinedRot = Transform3DHelper.CombineTransform(new RotateTransform3D(psiRot), new RotateTransform3D(thetaRot));
    // var rotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), theta);
    // var rot3D = new RotateTransform3D(rotation);
    var teapot = new CubeVisual3D()
    {
        Transform = combinedRot
    };

    var view = Plot3D.Create().AddObject(teapot);

    void Update()
    {
        thetaRot.Angle = theta;
        psiRot.Angle = psi;
        var combinedRot = Transform3DHelper.CombineTransform(new RotateTransform3D(psiRot), new RotateTransform3D(thetaRot));
        // rotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), theta);
        // rot3D = new RotateTransform3D(rotation);
        // rotation.Angle = theta;
        teapot.Transform = combinedRot;
    }

    win.ControlPanel.AddButton("Theta+", () =>
    {
        theta += 5;
        Update();
    });
    win.ControlPanel.AddButton("Theta-", () =>
    {
        theta -= 5;
        Update();
    });
    win.ControlPanel.AddButton("Psi+", () =>
    {
        psi += 5;
        Update();
    });
    win.ControlPanel.AddButton("Psi-", () =>
    {
        psi -= 5;
        Update();
    });

    
    view.Height = 400;
    win.Log(view);
});
