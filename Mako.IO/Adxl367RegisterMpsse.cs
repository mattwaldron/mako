namespace Mako.IO;

public class Adxl367RegisterMpsse : IAdxl367Register
{
    private IntPtr _mpsse;
    public Adxl367RegisterMpsse(IntPtr mpsse)
    {
        _mpsse = mpsse;
    }

    public byte[] ReadRegisters(byte address, int nregisters = 1)
    {
        var send = new byte[nregisters + 2];
        send[0] = 0x0B;
        send[1] = address;
        var rcvd = new byte[nregisters + 2];
        MpsseSpi.SPI_ReadWrite(_mpsse, rcvd, send, nregisters + 2, out var ntransferred, (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_ENABLE | (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_DISABLE);
        return rcvd.Skip(2).ToArray();
    }

    public byte ReadRegister(byte address)
    {
        var send = new byte[3];
        send[0] = 0x0B;
        send[1] = address;
        var rcvd = new byte[3];
        MpsseSpi.SPI_ReadWrite(_mpsse, rcvd, send, 3, out var ntransferred, (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_ENABLE | (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_DISABLE);
        return rcvd[2];
    }

    public void WriteRegisters(byte address, byte[] values)
    {
        var send = new byte[values.Length + 2];
        send[0] = 0x0A;
        send[1] = address;
        for (var i = 0; i < values.Length; i++)
        {
            send[i + 2] = values[i];
        }
        var rcvd = new byte[send.Length];
        MpsseSpi.SPI_ReadWrite(_mpsse, rcvd, send, send.Length, out var ntransferred, (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_ENABLE | (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_DISABLE);
    }

    public void WriteRegister(byte address, byte value)
    {
        var send = new byte[3];
        send[0] = 0x0A;
        send[1] = address;
        send[2] = value;
        var rcvd = new byte[send.Length];
        MpsseSpi.SPI_ReadWrite(_mpsse, rcvd, send, send.Length, out var ntransferred, (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_ENABLE | (int)MpsseSpi.SPI_TRANSFER_OPTIONS.CHIPSELECT_DISABLE);
    }
}
