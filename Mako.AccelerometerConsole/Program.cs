using Mako.IO;
using Microsoft.VisualBasic;

IntPtr mpsse;
Adxl367 adxl;

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

var xlInfo = adxl.ReadRegisters(0, 8);
Console.WriteLine(BitConverter.ToString(xlInfo));

adxl.StartMeasure();
adxl.UpdateRange(2);
adxl.SetRate(4);

// var fctl = adxl.ReadRegister(0x2C);
// Console.WriteLine($"FILTER_CTL = {fctl:X2}");

double xm = 0, ym = 0, zm = 0, xn = 0, yn = 0, zn = 0;
int count = 0;

while (true)
{
    if (Console.KeyAvailable)
    {
        break;
        Console.ReadKey();
        Console.WriteLine($"{xn:F5}, {xm:F5}, {yn:F5}, {ym:F5}, {zn:F5}, {zm:F5}");
        xm = 0; ym = 0; zm = 0; xn = 0; yn = 0; zn = 0;
        if (count++ > 8) break;
    }
    if (adxl.DataReady())
    {
        var (x, y, z) = adxl.Sample();
        if (x < xn) xn = x;
        if (x > xm) xm = x;
        if (y < yn) yn = y;
        if (y > ym) ym = y;
        if (z < zn) zn = z;
        if (z > zm) zm = z; 
        Console.WriteLine($"|{x,9:F5}|{y,9:F5}|{z,9:F5}|");
    }
    else
    {
        Thread.Sleep(50);
    }
}

MpsseSpi.SPI_CloseChannel(mpsse);
MpsseSpi.Cleanup_libMPSSE();