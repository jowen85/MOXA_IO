namespace IOManager
{
    /// <summary>
    /// IAdlinkAIO - Specific Analog IO Interface
    /// </summary>
    public interface IAdlinkIO : IBaseIO
    {
        #region Methods
        // Analog Card Functions
        /// <summary>
        /// This function is used to set the input range of the specified HSL AIO
        ///	modules.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="signalRange"></param>
        /// <returns></returns>
        bool SetSignalRange(ushort cardID, ushort setID, ushort slaveNo, ushort signalRange);
        /// <summary>
        /// This function is used to set the last of analog input channels about
        /// HSL AIO modules.
        /// Example
        /// HSL_A_set_last_channel(0, 0, 1, 5);
        /// The last channel is set to be 5, it means that analog input channels
        ///	0 to 5 are enabled and the others are disabled.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="lastChannel"></param>
        /// <returns></returns>
        bool SetLastChannel(ushort cardID, ushort setID, ushort slaveNo, ushort lastChannel);
        /// <summary>
        /// This function is used to initialize the Analog I/O channels reading
        /// operation of all HSL AIO modules.
        /// Before using HSL_A_read_input(), HSL_A_write_output() and
        /// HSL_A_sync_rw() functions to acquire the AIO channel value,
        /// please perform HSL_A_start_read() function to initialize the
        ///	initialization task.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <returns></returns>
        bool StartRead(ushort cardID, ushort setID);
        /// <summary>
        /// This function is used to stop the Analog I/O channel reading
        /// operation of all HSL AIO modules which connect to the controller
        ///	with the connect_index and card_ID.
        /// When you want to stop the AI channels value acquisition task,
        /// please use W_HSL_AI_Stopt_Read to stop.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <returns></returns>
        bool StopRead(ushort cardID, ushort setID);
        /// <summary>
        /// This function is used to read the specified AI channel on the slave
        /// module.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="inChan"></param>
        /// <returns></returns>
        bool ReadAI(ushort cardID, ushort setID,
            ushort slaveNo, ushort inChan);
        /// <summary>
        /// This function is used to write analog output data to HSL AIO module.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="setID"></param>
        /// <param name="slaveNo"></param>
        /// <param name="outChan"></param>
        /// <param name="outValue"></param>
        /// <returns></returns>
        bool WriteAO(ushort cardID, ushort setID, ushort slaveNo, ushort outChan, double outValue);
        #endregion Methods

        #region Properties
        /// <summary>
        /// Property to store the analog input value.
        /// </summary>
        double AI_Data { get; }
        /// <value>
        /// Number of PCI card plug into the mother board.
        /// </value>
        ushort Num_IO_Card { set; get; }
        /// <value>
        /// Only for Adlink Card.
        /// Maximum Port Number exist in the IO modules.
        /// For Adlink Card, it will always be 1.
        /// For Advantech Card = Card with most number of bit / 8 (normally)
        /// </value>
        ushort Max_Port_Num { set; get; }
        /// <summary>
        /// Only for Adlink Card.
        /// Refers to the number of master in the PCI card.
        /// </summary>
        ushort[] Num_Of_SetID { set; get; }
        #endregion Properties
    }
}
