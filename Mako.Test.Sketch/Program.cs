using HelixToolkit.Wpf;
using Mako.Ui.Sketch;
using Mako.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;


IntPtr mpsse;
Adxl367 adxl;
bool running = true;

MpsseSpi.Init_libMPSSE();
MpsseSpi.SPI_OpenChannel(0, out mpsse);

var config = new MpsseSpi.ChannelConfig
{
    ClockRate = 1_000_000,
    LatencyTimer = 1,
    configOptions = (int)MpsseSpi.SPI_CONFIG_OPTION.CS_ACTIVELOW
};
MpsseSpi.SPI_InitChannel(mpsse, out config);

adxl = new Adxl367(new Adxl367RegisterMpsse(mpsse));

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

    var teapot = new Teapot();

    var view = Plot3D.Create().AddObject(teapot);
    view.Camera.Position = new Point3D(3.2, 3.2, 3.2);
    view.Camera.UpDirection = new Vector3D(0.2, 0.2, 0.9);
    view.Camera.LookDirection = new Vector3D(-3.2, -3.2, -3.2);

    view.Height = 400;
    win.Log(view);

    win.Closing += (s, e) => { running = false; };

    var thr = new Thread(() =>
    {
        adxl.StartMeasure();
        adxl.UpdateRange(2);
        adxl.SetRate(4);
        while (running)
        {
            if (adxl.DataReady())
            {
                var (x, y, z) = adxl.Sample();
                teapot.Dispatcher.Invoke(() => teapot.Tilt(new Vector3D(x, y, z)));
            }
            else
            {
                Thread.Sleep(50);
            }
        }
    });
    thr.SetApartmentState(ApartmentState.STA);
    thr.Start();

    /*
    void Update()
    {
        teapot.Rotate(yaw, pitch, roll);
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
    win.ControlPanel.AddButton("Reset", () =>
    {
        yaw = 0; pitch = 0; roll = 0;
        Update();
    });
    */

    
});
