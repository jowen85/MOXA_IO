namespace IOManager
{
    public interface IMoxaIO : IBaseIO
    {
        #region Methods
        void StartScanIO();
        #endregion Methods

        #region Properties
        string[] DI_IpAddress { set; get; }
        string[] DO_IpAddress { set; get; }
        int[] MaxDIPortNum { set; get; }
        int[] MaxDOPortNum { set; get; }
        int Num_IO_Card_Input { set; get; }
        int Num_IO_Card_Output { set; get; }
        bool FailTriggerOutBit { set; get; }
        #endregion Properties
    }
}
