namespace Core.Enums
{
    public enum OperationCmd
    {
        Start,
        Stop,
        Init,
        Reset,
    }

    public enum Machine_Status
    {
        None,
        Idle,
        Ready,
        Initializing,
        Running,
        Stop,
        Error,
        Ending_Lot,
    }
}
