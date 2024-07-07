using System;
using System.Runtime.InteropServices;

// Conversion of libmpsse_spi.h from FTDI, with a little help from https://github.com/DVDPT/libMPSSE-.Net-Wrapper
namespace Mako.IO;

public class MpsseSpi
{
    /* Bit definition of the transferOptions parameter in SPI_Read, SPI_Write & SPI_Transfer  */
    [Flags]
    public enum SPI_TRANSFER_OPTIONS : int
    {
        /* transferOptions-Bit0: If this bit is 0 then it means that the transfer size provided is in bytes */
        SIZE_IN_BYTES = 0x00000000,
        /* transferOptions-Bit0: If this bit is 1 then it means that the transfer size provided is in bytes */
        SIZE_IN_BITS = 0x00000001,
        /* transferOptions-Bit1: if BIT1 is 1 then CHIP_SELECT line will be enabled at start of transfer */
        CHIPSELECT_ENABLE = 0x00000002,
        /* transferOptions-Bit2: if BIT2 is 1 then CHIP_SELECT line will be disabled at end of transfer */
        CHIPSELECT_DISABLE = 0x00000004,
        /* transferOptions-Bit3: if BIT3 is 1 then LSB will be processed first */
        LSB_FIRST = 0x00000008
    }


    /* Bit definition of the Options member of configOptions structure */
    [Flags]
    public enum SPI_CONFIG_OPTION
    {
        MODE_MASK = 0x00000003,
        MODE0 = 0x00000000,
        MODE1 = 0x00000001,
        MODE2 = 0x00000002,
        MODE3 = 0x00000003,
        CS_MASK = 0x0000001C,		/* 111 00 */
        CS_DBUS3 = 0x00000000,		/* 000 00 */
        CS_DBUS4 = 0x00000004,		/* 001 00 */
        CS_DBUS5 = 0x00000008,		/* 010 00 */
        CS_DBUS6 = 0x0000000C,		/* 011 00 */
        CS_DBUS7 = 0x00000010,		/* 100 00 */
        CS_ACTIVEHIGH = 0x00000000,
        CS_ACTIVELOW = 0x00000020
    }


    /******************************************************************************/
    /*								Type defines								  */
    /******************************************************************************/

    /* Structure contains configuration information of the SPI channel. It is populated by the user
    application during initialization of the channel and then it is saved in a linked-list and used
    internally by other SPI functions during operation. The structure is removed from the list when
    the user application calls SPI_CloseChannel */
    public struct ChannelConfig
    {
        public int ClockRate;   /* SPI clock rate, value should be <= 30000000 */
        public byte LatencyTimer;   /* value in milliseconds, maximum value should be <= 255 */
        public int configOptions;   /* This member provides a way to enable/disable features
    specific to the protocol that are implemented in the chip
    BIT1-0=CPOL-CPHA:	00 - MODE0 - data captured on rising edge, propagated on falling
                        01 - MODE1 - data captured on falling edge, propagated on rising
                        10 - MODE2 - data captured on falling edge, propagated on rising
                        11 - MODE3 - data captured on rising edge, propagated on falling
    BIT4-BIT2: 000 - A/B/C/D_DBUS3=ChipSelect
             : 001 - A/B/C/D_DBUS4=ChipSelect
             : 010 - A/B/C/D_DBUS5=ChipSelect
             : 011 - A/B/C/D_DBUS6=ChipSelect
             : 100 - A/B/C/D_DBUS7=ChipSelect
    BIT5: ChipSelect is active high if this bit is 0
    BIT6 -BIT31		: Reserved
    */
        public int Pin; /* BIT7   -BIT0:   Initial direction of the pins	*/
        /* BIT15 -BIT8:   Initial values of the pins		*/
        /* BIT23 -BIT16: Final direction of the pins		*/
        /* BIT31 -BIT24: Final values of the pins		*/
        public ushort currentPinState;  /* BIT7   -BIT0:   Current direction of the pins	*/
        /* BIT15 -BIT8:   Current values of the pins	*/
    }

    /**  @noop FT_DEVICE_LIST_INFO_NODE
 * @par Summary
 * This structure is used for passing information about a device back from the FT_GetDeviceInfoList function.
 * @see FT_GetDeviceInfoList
 */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FT_DEVICE_LIST_INFO_NODE
    {
        public ulong Flags;
        public ulong Type;
        public ulong ID;
        public int LocId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SerialNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Description;
        public IntPtr ftHandle;
    }

    /*!
     * \brief This function initializes libMPSSE
     *
     * This function is called once when the library is loaded. It initializes all the modules in the
     * library. This function initializes all the variables and data structures that are required to be
     * initialized once only during loading.
     *
     * \param[in] none
     * \param[out] none
     * \return none
     * \sa
     * \note May individually call Ftdi_I2C_Module_Init, Ftdi_SPI_Module_Init, Ftdi_Mid_Module_Init,
     * Ftdi_Common_Module_Init, etc if required. This function should be called by the OS specific
     * function(eg: DllMain for windows) that is called by the OS automatically during startup.
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Init_libMPSSE();

    /*!
     * \brief Cleans up the module before unloading
     *
     * This function frees all the resources that were allocated during initialization. It should be called
     * by the OS to ensure that the module exits gracefully
     *
     * \param[in] none
     * \param[out] none
     * \return none
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static void Cleanup_libMPSSE();

    /*!
     * \brief Gets the number of SPI channels connected to the host
     *
     * This function gets the number of SPI channels that are connected to the host system
     *
     * \param[out] *numChannels Pointer to variable in which the no of channels will be returned
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note This function doesn't return the number of FTDI chips connected to the host system
     * \note FT2232D has 1 MPSSE port
     * \note FT2232H has 2 MPSSE ports
     * \note FT4232H has 4 ports but only 2 of them have MPSSEs
     * so call to this function will return 2 if a FT4232 is connected to it.
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_GetNumChannels(out int numChannels);

    /*!
     * \brief Provides information about channel
     *
     * This function takes a channel index (valid values are from 0 to the value returned by
     * SPI_GetChannelInfo -1) and provides information about the channel in the form of a
     * populated ChannelInfo structure.
     *
     * \param[in] index Index of the channel
     * \param[out] chanInfo Pointer to FT_DEVICE_LIST_INFO_NODE structure(see D2XX \
    Programmer's Guide)
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \note  The channel ID can be determined by the user from the last digit of the location ID
     * \sa
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_GetChannelInfo(int index,
        out FT_DEVICE_LIST_INFO_NODE chanInfo);

    /*!
     * \brief Opens a channel and returns a handle to it
     *
     * This function opens the indexed channel and returns a handle to it
     *
     * \param[in] index Index of the channel
     * \param[out] handle Pointer to the handle of the opened channel
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note Trying to open an already open channel will return an error code
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_OpenChannel(int index, out IntPtr handle);

    /*!
     * \brief Initializes a channel
     *
     * This function initializes the channel and the communication parameters associated with it
     *
     * \param[in] handle Handle of the channel
     * \param[out] config Pointer to ChannelConfig structure(memory to be allocated by caller)
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_InitChannel(IntPtr handle, out ChannelConfig config);

    /*!
     * \brief Closes a channel
     *
     * Closes a channel and frees all resources that were used by it
     *
     * \param[in] handle Handle of the channel
     * \param[out] none
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_CloseChannel(IntPtr handle);

    /*!
     * \brief Reads data from a SPI slave device
     *
     * This function reads the specified number of bits or bytes from the SPI device
     *
     * \param[in] handle Handle of the channel
     * \param[in] *buffer Pointer to buffer to where data will be read to
     * \param[in] sizeToTransfer Size of data to be transfered
     * \param[out] sizeTransfered Pointer to variable containing the size of data
     *			that got transferred
     * \param[in] transferOptions This parameter specifies data transfer options
     *				if BIT0 is 0 then size is in bytes, otherwise in bits
     *				if BIT1 is 1 then CHIP_SELECT line will be enables at start of transfer
     *				if BIT2 is 1 then CHIP_SELECT line will be disabled at end of transfer
     *
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_Read(IntPtr handle, byte[] buffer,
        int sizeToTransfer, out int sizeTransfered, int options);

    /*!
     * \brief Writes data to a SPI slave device
     *
     * This function writes the specified number of bits or bytes to the SPI device
     *
     * \param[in] handle Handle of the channel
     * \param[in] *buffer Pointer to buffer from containing the data
     * \param[in] sizeToTransfer Size of data to be transfered
     * \param[out] sizeTransfered Pointer to variable containing the size of data
     *			that got transferred
     * \param[in] transferOptions This parameter specifies data transfer options
     *				if BIT0 is 0 then size is in bytes, otherwise in bits
     *				if BIT1 is 1 then CHIP_SELECT line will be enables at start of transfer
     *				if BIT2 is 1 then CHIP_SELECT line will be disabled at end of transfer
     *
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_Write(IntPtr handle, byte[] buffer,
        int sizeToTransfer, out int sizeTransfered, int options);

    /*!
     * \brief Reads and writes data from/to a SPI slave device
     *
     * This function transfers data in both directions between a SPI master and a slave. One bit is
     * clocked out and one bit is clocked in during every clock.
     *
     * \param[in] handle Handle of the channel
     * \param[in] *inBuffer Pointer to buffer to which data read will be stored
     * \param[in] *outBuffer Pointer to buffer that contains data to be transferred to the slave
     * \param[in] sizeToTransfer Size of data to be transferred
     * \param[out] sizeTransfered Pointer to variable containing the size of data
     *			that got transferred
     * \param[in] transferOptions This parameter specifies data transfer options
     *				if BIT0 is 0 then size is in bytes, otherwise in bits
     *				if BIT1 is 1 then CHIP_SELECT line will be enables at start of transfer
     *				if BIT2 is 1 then CHIP_SELECT line will be disabled at end of transfer
     *
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_ReadWrite(IntPtr handle, byte[] inBuffer,
        byte[] outBuffer, int sizeToTransfer, out int sizeTransferred,
        int transferOptions);

    /*!
     * \brief Read the state of SPI MISO line
     *
     * Reads the logic state of the SPI MISO line without clocking the bus
     *
     * \param[in] handle Handle of the channel
     * \param[out] *state State of the line
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note This function may be used for applications that needs to poll the state of
     *		the MISO line to check if the device is busy
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_IsBusy(IntPtr handle, out bool state);

    /*!
     * \brief Changes the chip select line
     *
     * This function changes the chip select line that is to be used to communicate to the SPI slave
     *
     * \param[in] handle Handle of the channel
     * \param[in] configOptions Provides a way to select the chip select line & SPI mode
     *	BIT1-0=CPOL-CPHA:	00 - MODE0 - data captured on rising edge, propagated on falling
     *						01 - MODE1 - data captured on falling edge, propagated on rising
     *						10 - MODE2 - data captured on falling edge, propagated on rising
     *						11 - MODE3 - data captured on rising edge, propagated on falling
     *	BIT4-BIT2: 000 - A/B/C/D_DBUS3=ChipSelect
     *			 : 001 - A/B/C/D_DBUS4=ChipSelect
     * 			 : 010 - A/B/C/D_DBUS5=ChipSelect
     *			 : 011 - A/B/C/D_DBUS6=ChipSelect
     *			 : 100 - A/B/C/D_DBUS7=ChipSelect
     *	BIT5: ChipSelect is active high if this bit is 0
     *	BIT6 -BIT31		: Reserved
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note This function should only be called after SPI_Init has been called
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_ChangeCS(IntPtr handle, int configOptions);

    /*!
     * \brief Toggles the state of the CS line
     *
     * This function turns ON/OFF the chip select line associated with the channel
     *
     * \param[in] handle Handle of the channel
     * \param[in] state TRUE if CS needs to be set, false otherwise
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int SPI_ToggleCS(IntPtr handle, bool state);

    /*!
     * \brief Writes to the 8 GPIO lines
     *
     * Writes to the 8 GPIO lines associated with the high byte of the MPSSE channel
     *
     * \param[in] handle Handle of the channel
     * \param[in] dir The direction of the 8 lines. 0 for in and 1 for out
     * \param[in] value Output state of the 8 GPIO lines
     * \return status
     * \sa
     * \note
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int FT_WriteGPIO(IntPtr handle, byte dir, byte value);

    /*!
     * \brief Reads from the 8 GPIO lines
     *
     * This function reads the GPIO lines associated with the high byte of the MPSSE channel
     *
     * \param[in] handle Handle of the channel
     * \param[out] *value Input state of the 8 GPIO lines(1 = high)
     * \return status
     * \sa
     * \note The directions of the GPIO pins have to be first set to input mode using FT_WriteGPIO
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int FT_ReadGPIO(IntPtr handle, out byte value);

    /******************************************************************************/

    /*!
     * \brief Version Number Function
     *
     * Returns libMPSSE and libFTD2XX version number
     *
     * \param[out]  *libmpsse	MPSSE version number is returned
     * \param[out]  *libftd2xx	D2XX version number is returned
     * \return Returns status code of type int(see D2XX Programmer's Guide)
     * \sa
     * \warning
     */
    [DllImport("Lib\\x64\\libmpsse.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
    public extern static int Ver_libMPSSE(out int libmpsse, out int libftd2xx);
}
