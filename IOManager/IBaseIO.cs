namespace IOManager
{
    /// <summary>
    /// General IO interface
    /// </summary>
    public interface IBaseIO
    {
        #region Methods
        bool OpenDevice();
        bool CloseDevice();
        bool ClearOutput();
        bool ReadInBit(ushort? bit, bool invert);
        bool ReadInBit(ushort? bit);
        bool ReadOutBit(ushort? bit);
        bool WriteOutBit(ushort? bit, ushort state);
        bool WritePort(ushort startBitNum, uint oData);
        uint ReadPort(ushort startBitNum);
        #endregion Methods

        #region Properties
        /// <summary>
        /// Maximum number of bit exist in a port / card.
        /// For Adlink Card, this will represent the maximum number of bit exsit in a slave module.
        /// For Advantech Card, it will be 8.
        /// </summary>
        ushort Max_Bit_PerPort { set; get; }
        /// <value>
        /// Error message reported after a call into the actual IO library.
        /// </value>
        string Err_Msg { set; get; }

        bool IsConnected { get; }
        #endregion Properties
    }
}

