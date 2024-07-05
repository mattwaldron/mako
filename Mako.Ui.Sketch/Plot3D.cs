using HelixToolkit.Wpf;
using System.DirectoryServices;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Mako.Ui.Sketch;

public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

internal class Plot3D
{
    public static HelixViewport3D Create()
    {
        var view = new HelixViewport3D();
        var lights = new Model3DGroup();
        lights.Children.Add(new AmbientLight());
        
    }
    public static HelixViewport3D AddPoint(this HelixViewport3D plot, Point3D p)
    {
        
    }
}
