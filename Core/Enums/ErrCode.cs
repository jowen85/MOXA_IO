
namespace Core.Enums
{
    //TODO : Manage Error Code
    public enum ErrCode
    {
        //Critical Scan Error
        Critical_EmoTrigger = 1001,
        Critical_AirLow,

        //Csst Elevator Motor Error
        Failed_To_Move_Csst_Elevator_To_FeederPost,
        Failed_To_Move_Csst_Elevator_To_StandbyPost,
        Failed_To_Home_Csst_Elevator,
        Failed_To_Move_Csst_Elevator_Slots_Indexing,
        Csst_Elevator_Hit_Feeder_Conveyor,

        //PreAligner Motor Error
        Failed_to_ServoOn_PreAligner_AllAxis,
        Failed_to_Home_PreAligner_ZAxis,
        Failed_to_Home_PreAligner_XYWAxis,
        Failed_to_Move_XYW_to_Standby_Pos,
        Failed_to_Move_W_Offset_Pos,
        Failed_to_Vacuum_On,
        Failed_to_Vacuum_Off,

        //Buffer Motor Error
        Failed_To_ServoOn_Buffer_Elevator,
        Failed_To_Home_Buffer_Elevator,
        Failed_To_Move_Buffer_Elevator_To_FirstLocation,
        Failed_To_Move_Buffer_Elevator_Down,
        Failed_To_Move_Buffer_Elevator_Up,

        //Conveyor Motor Error
        Failed_To_ServoOn_Elevetor_or_FeederConveyor,
        Failed_To_ServoOn_AllConveyor,
        Failed_To_Home_AllConveyor,
        Failed_To_Move_Output_Conveyor,
        Failed_To_Move_All_Conveyor,
        Failed_To_Move_Wafer_To_Load_Conveyor,
        Extra_Wafer_On_Buffer_Conveyor,

        //Cylinder Error
        Failed_To_Home_AllValve,
        Failed_To_Extend_Feeder_Conveyor,
        Failed_To_Retract_Feeder_Conveyor,
        Failed_To_Clamp_Csst,
        Failed_To_Unclamp_Csst,
        Failed_To_Extend_Wafer_Aligner_Gripper,
        Failed_To_Retract_Wafer_Aligner_Gripper,
        Failed_To_Extend_Reject_Conveyor,
        Failed_To_Retract_Reject_Conveyor,
        Failed_To_Extend_Wafer_Stopper,
        Failed_To_Retract_Wafer_Stopper,

        //Vision
        Failed_To_Trigger_Vision_PreAligner,
        Failed_To_Trigger_Vision_1,
        Failed_To_Trigger_Vision_2,
    }
}
