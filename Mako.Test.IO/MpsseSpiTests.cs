using Mako.IO;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace Mako.Test.IO;

public class MpsseSpiTests
{
    private IntPtr mpsse;
    private Adxl367 adxl;
    [SetUp]
    public void Setup()
    {
        MpsseSpi.Init_libMPSSE();
        MpsseSpi.SPI_GetNumChannels(out var nchannels);
        MpsseSpi.SPI_OpenChannel(0, out mpsse);

        var config = new MpsseSpi.ChannelConfig
        {
            ClockRate = 1_000_000,
            LatencyTimer = 1,
            configOptions = (int)MpsseSpi.SPI_CONFIG_OPTION.CS_ACTIVELOW
        };
        MpsseSpi.SPI_InitChannel(mpsse, out config);

        adxl = new Adxl367(new Adxl367RegisterMpsse(mpsse));
    }

    [TearDown]
    public void Teardown()
    {
        MpsseSpi.SPI_CloseChannel(mpsse);
        MpsseSpi.Cleanup_libMPSSE();
    }

    [Test]
    public void ReadId()
    {
        var inp = new byte[8];
        var outp = new byte[8] { 11, 0, 0, 0, 0, 0, 0, 0, };
        MpsseSpi.SPI_ReadWrite(mpsse, inp, outp, 6, out var ntransferred, (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_ENABLE | (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_ENABLE);
        TestContext.Progress.WriteLine(Convert.ToHexString(inp));
    }

    [Test]
    public void Start()
    {
        adxl.StartMeasure();
    }

    [Test]
    public void Sample()
    {
        var (x, y, z) = adxl.Sample();
        TestContext.Progress.WriteLine($"{x}, {y}, {z}");
    }

    [Test]
    public void DataReady()
    {
        var dr = adxl.DataReady();
        TestContext.Progress.WriteLine(dr);
    }
}