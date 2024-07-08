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

    public static ModelVisual3D Rotate(this ModelVisual3D model, double yaw, double pitch, double roll)
    {
        var pitchRot = new RotateTransform3D(new AxisAngleRotation3D(pitchAxis, pitch));
        var yawRot = new RotateTransform3D(new AxisAngleRotation3D(yawAxis, yaw));
        var rollRot = new RotateTransform3D(new AxisAngleRotation3D(rollAxis, roll));
        
        model.Transform = Transform3DHelper.CombineTransform(
            rollRot, 
            Transform3DHelper.CombineTransform(
                pitchRot,
                yawRot));
        return model;
    }

    private static Vector3D tiltFromDefault = new Vector3D(0, 0, 1);

    public static ModelVisual3D Tilt(this ModelVisual3D model, Vector3D toward)
    {
        return model.Tilt(toward, tiltFromDefault);
    }

    public static ModelVisual3D Tilt(this ModelVisual3D model, Vector3D toward, Vector3D from)
    {
        var rotationAxis = new Vector3D(toward.Y * from.Z - toward.Z * from.Y,
                                        toward.Z * from.X - toward.X * from.Z,
                                        toward.X * from.Y - toward.Y * from.X);

        var angle = Math.Asin(rotationAxis.Length / (toward.Length * from.Length));
        angle = angle * 180 / Math.PI;
        if ((toward.X * from.X + toward.Y*from.Y + toward.Z*from.Z) < 0)
        {
            angle = 180 - angle;
        }
        model.Transform = new RotateTransform3D(new AxisAngleRotation3D(rotationAxis, angle));
        return model;
    }


}
