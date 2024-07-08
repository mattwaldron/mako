using HelixToolkit.Wpf;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Mako.Ui.Sketch;

/*public class Point3D
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
}*/

public static class Plot3D
{
    public static HelixViewport3D Create()
    {
        var view = new HelixViewport3D();
        view.Children.Add(new DefaultLights());
        return view;
    }

    public static HelixViewport3D AddObject(this HelixViewport3D plot, Visual3D obj)
    {
        /*if (color != null)
        {
            mesh.Material = MaterialHelper.CreateMaterial(color.Value);
        }*/
        obj.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0));

        plot.Children.Add(obj);
        // plot.ZoomExtents();
        return plot;
    }

    private static Vector3D rollAxis = new Vector3D(1, 0, 0);
    private static Vector3D pitchAxis = new Vector3D(0, 1, 0);
    private static Vector3D yawAxis = new Vector3D(0, 0, 1);

    public static ModelVisual3D Rotate(this ModelVisual3D model, double pitch, double yaw, double roll)
    {
        var pitchRot = new RotateTransform3D(new AxisAngleRotation3D(pitchAxis, pitch));
        var yawRot = new RotateTransform3D(new AxisAngleRotation3D(yawAxis, yaw));
        var rollRot = new RotateTransform3D(new AxisAngleRotation3D(rollAxis, roll));
        
        model.Transform = Transform3DHelper.CombineTransform(
            Transform3DHelper.CombineTransform(
                rollRot, yawRot),
                pitchRot);
        return model;
    }

}
