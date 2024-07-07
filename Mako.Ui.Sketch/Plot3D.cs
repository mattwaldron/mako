using HelixToolkit.Wpf;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
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

public static class Plot3D
{
    public static HelixViewport3D Create()
    {
        var view = new HelixViewport3D();
        view.Children.Add(new DefaultLights());
        return view;
    }

    public static HelixViewport3D AddMesh(this HelixViewport3D plot, MeshElement3D mesh, Color? color = null)
    {
        if (color != null)
        {
            mesh.Material = MaterialHelper.CreateMaterial(color.Value);
        }

        plot.Children.Add(mesh);
        return plot;
    }

    public static MeshElement3D Rotate(this MeshElement3D mesh, double theta, double psi)
    {
        mesh.Content.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), theta));
        /*mesh.Transform = Transform3DHelper.CombineTransform(
                new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), theta)),
                new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), psi))
            );*/
        return mesh;
    }

}
