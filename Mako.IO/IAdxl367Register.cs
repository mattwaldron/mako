namespace Mako.IO;

public interface IAdxl367Register
{
    byte[] ReadRegisters(byte address, int nregisters);

    byte ReadRegister(byte address);

    void WriteRegisters(byte address, byte[] values);

    void WriteRegister(byte address, byte value);
}
