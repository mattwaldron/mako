namespace Mako.IO;

public class Adxl367 : IAdxl367Register
{
    private IAdxl367Register _interface;
    private int _range = 2;
    private static Dictionary<int, double> _rangeToSensitivity = new()
    {
        { 2, 0.00025},
        { 4, 0.0005 },
        { 8, 0.001 }
    };

    public Adxl367(IAdxl367Register iface)
    {
        _interface = iface;
    }

    public (double x, double y, double z) Sample()
    {
        var bytes = _interface.ReadRegisters(0x0E, 6);
        var uints = new UInt16[3];
        uints[0] = (UInt16)((bytes[0] << 8) | (bytes[1]));
        uints[1] = (UInt16)((bytes[2] << 8) | (bytes[3]));
        uints[2] = (UInt16)((bytes[4] << 8) | (bytes[5]));
        var gs = uints.Select(b => (Int16)b).Select(sb => (double)sb).Select(d => d / 4 * _rangeToSensitivity[_range]).ToList();

        return (gs[0], gs[1], gs[2]);
    }

    public void StartMeasure()
    {
        _interface.WriteRegister(0x2D, 2);
    }

    public void Reset()
    {
        _interface.WriteRegister(0x1F, 2);
    }

    public void UpdateRange(int r = 0)
    {
        var filterCtl = _interface.ReadRegister(0x2C);
        if (r != 0)
        {
            var filterCtlInt = (int)filterCtl;
            filterCtlInt &= ~(byte)0xC0;
            if (r >= 8) { filterCtlInt |= 0x80; }
            else if (r >= 4) { filterCtlInt |= 0x40; }
            filterCtl = (byte)filterCtlInt;
            _interface.WriteRegister(0x2C, filterCtl);
        }
       
        var range = filterCtl >> 6;
        switch (range)
        {
            case 0: _range = 2; break;
            case 1: _range = 4; break;
            case 2: _range = 8; break;
            default: _range = 2; break;
        }
    }

    public void SetRate(int r = 0)
    {
        r &= 0x07;
        var filterCtl = _interface.ReadRegister(0x2C);
        filterCtl &= (byte)0xF8;
        filterCtl |= (byte)r;
        _interface.WriteRegister(0x2C, filterCtl);
    }

    public bool DataReady()
    {
        var status = _interface.ReadRegister(0x0B);
        return (status & 0x1) != 0;
    }

    public byte[] ReadRegisters(byte address, int nregisters)
    {
        return _interface.ReadRegisters(address, nregisters);
    }

    public byte ReadRegister(byte address)
    {
        return _interface.ReadRegister(address);
    }

    public void WriteRegisters(byte address, byte[] values)
    {
        _interface.WriteRegisters(address, values);
    }

    public void WriteRegister(byte address, byte value)
    {
        _interface.WriteRegister(address, value);
    }
}
