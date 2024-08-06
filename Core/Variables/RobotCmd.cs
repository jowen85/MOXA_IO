
using System.ComponentModel;

namespace Core.Variables
{
    public class RobotCmd
    {
        #region Motion Command
        public static string READ_MAPPING_DATA()
        {
            return "RSR";
        }

        public static string SERVO_ON_ALL()
        {
            return "SVON A";
        }

        public static string SERVO_ON(string Axis)
        {
            return "SVON " + Axis;
        }

        public static string SERVO_OFF_ALL()
        {
            return "SVOF A";
        }

        public static string SERVO_OFF(string Axis)
        {
            return "SVOF " + Axis;
        }

        public static string HOME_ALL()
        {
            return "HOME A";
        }

        public static string HOME(string Axis)
        {
            return "HOME " + Axis;
        }

        public static string STOP_ALL()
        {
            return "STOP A";
        }

        public static string STOP(string Axis)
        {
            return "STOP " + Axis;
        }

        public static string RESET()
        {
            return "RES";
        }

        public static string RESET_EMO()
        {
            return "REMS";
        }

        public static string E_STOP()
        {
            return "ABM";
        }

        public static string MOVE_ABSOLUTE(string Axis, int Position)
        {
            return "MOVA " + Axis + " " + Position;
        }

        public static string MOVE_RELATIVE(string Axis, int Position)
        {
            return "MOVR " + Axis + " " + Position;
        }

        public static string TURN_ON_R_VACUUM()
        {
            // OUTP [1 = R Axis, 2 = W Axis] [1 = On, 0 = Off] 
            return "OUTP 1 1";
        }

        public static string TURN_OFF_R_VACUUM()
        {
            // OUTP [1 = R Axis, 2 = W Axis] [1 = On, 0 = Off] 
            return "OUTP 1 0";
        }

        public static string TURN_ON_W_VACUUM()
        {
            // OUTP [1 = R Axis, 2 = W Axis] [1 = On, 0 = Off] 
            return "OUTP 2 1";
        }

        public static string TURN_OFF_W_VACUUM()
        {
            // OUTP [1 = R Axis, 2 = W Axis] [1 = On, 0 = Off] 
            return "OUTP 2 0";
        }

        public static string TURN_ON_LASER_SENSOR()
        {
            //Inverted due to wiring type of laser sensor
            return "OUTPUT 3 0";
        }

        public static string TURN_OFF_LASER_SENSOR()
        {
            return "OUTPUT 3 1";
        }

        public static string GET(string Station, int Slot)
        {
            //WILL MOVE TO HOME POSITION AFTER GET
            return "GET " + Station + " " + Slot;
        }

        public static string PUT(string Station, int Slot)
        {
            //WILL MOVE TO HOME POSITION AFTER PUT
            return "PUT " + Station + " " + Slot;
        }

        public static string MOVE_TO_STATION(string Station, string Axis = "T")
        {
            return "MATS " + Station + " " + Axis;
        }

        public static string MAP(string Station)
        {
            //mapping wafer
            return "MAP " + Station;
        }

        #endregion Motion Command

        #region Set Command
        public static string SAVE_PARAMETER(string WorkGroup = "A")
        {
            //Work Group: A to E
            return "SAV " + WorkGroup;
        }

        public static string SET_REPICK_WAFER_AND_VACUUM_TIME(int Retry, int Timeout_ms)
        {
            //SET UP REPICKING TIMES OF GET AND VACUUM DETECTING TIME
            return "SVAC " + Retry + " " + Timeout_ms;
        }

        public static string SET_All_AXIS_SPEED(int VelocityT, int VelocityR, int VelocityZ, int VelocityW)
        {
            //Axis: A(All Axis)
            return "SSP A " + VelocityT + " " + VelocityR + " " + VelocityZ + " " + VelocityW;
        }

        public static string SET_AXIS_SPEED(string Axis, int VelocityT, int VelocityR, int VelocityZ, int VelocityW)
        {
            //Axis: T,R,Z,W
            return "SSP " + Axis + " " + VelocityT + " " + VelocityR + " " + VelocityZ + " " + VelocityW;
        }

        public static string SET_WORKING_ARM(string Station, int Flag)
        {
            //select R-axis of W-axis of robotic arms to excute the pick/palce of wafer
            //Station : A-Z,AA-AZ...CA-CV
            //Flag: 0(R Axis), 1(W Axis)

            return "SSEE " + Station + " " + Flag;
        }

        public static string SET_PICKUP_SPEED(int Velocity)
        {
            return "SSPZ 0 " + Velocity;
        }

        public static string SET_PLACING_SPEED(int Velocity)
        {
            return "SSPZ 1 " + Velocity;
        }

        public static string SET_MAPPING_COORDINATE(string Station, int CoordinateT, int _CoordinateR, int CoordinateZ, int CoordinateW)
        {
            // SET UP MAPPING COORDINATION OF STATION
            return "SPSC " + Station + " " + CoordinateT + " " + _CoordinateR + " " + CoordinateZ + " " + CoordinateW;
        }

        public static string SET_WAFER_CASSETTE_COORDINATE(string Station, int CoordinateT, int _CoordinateR, int CoordinateZ, int CoordinateW)
        {
            // SET UP COORDINATE OF WAFER CASETTE STATION 
            return "SPC " + Station + " " + CoordinateT + " " + _CoordinateR + " " + CoordinateZ + " " + CoordinateW;
        }

        public static string SET_TOTAL_WAFER_SLOT(string Station, int TotalSlot)
        {
            //total wafer quantity
            return "SSN " + Station + " " + TotalSlot;
        }

        public static string SET_WAFER_SIZE(string Station, int SizeGroup)
        {
            return "SWS " + Station + " " + SizeGroup;
        }

        public static string SET_PITCH(string Station, int Pitch)
        {
            return "PITCH " + Station + " " + Pitch;
        }

        public static string SET_OFFSET(string Station, int Offset)
        {
            return "SOF " + Station + " " + Offset;
        }

        public static string SET_STROKE(string Station, int Stroke)
        {
            return "SST " + Station + " " + Stroke;
        }

        public static string SET_RETRACT_POSITION(string Station, int RETRACT)
        {
            return "SRET " + Station + " " + RETRACT;
        }

        public static string SET_Z_AXIS_SPEED(int SizeGroup, int Velocity)
        {
            return "SSPM " + SizeGroup + " " + Velocity;
        }

        public static string SET_Z_AXIS_ACCELERATION(int SizeGroup, int Acceleration)
        {
            return "SADM " + SizeGroup + " " + Acceleration;
        }

        public static string SET_T_AXIS_ROTATION_ANGLE(int SizeGroup, int Angle)
        {
            return "SDGM " + SizeGroup + " " + Angle;
        }

        public static string SET_MAXIMUM_THICKNESS(int SizeGroup, int Thikness)
        {
            return "SMAXM " + SizeGroup + " " + Thikness;
        }

        public static string SET_MINIMUNM_THICKNESS(int SizeGroup, int Thikness)
        {
            return "SMINM " + SizeGroup + " " + Thikness;
        }

        public static string SET_MAXIMUM_SKEWNESS(int SizeGroup, int Thikness)
        {
            return "SHORM " + SizeGroup + " " + Thikness;
        }

        public static string SET_MINIMUM_PITCH(int SizeGroup, int Pitch)
        {
            return "SGAPM " + SizeGroup + " " + Pitch;
        }

        #endregion Set Command

        #region Read Command
        public static string READ_VACUUM_R()
        {
            // INPUT [INPUT/OUTPUT IO] [10=R Axis]
            return "INPUT 0 10";
        }

        public static string READ_VACUUM_W()
        {
            // INPUT [INPUT/OUTPUT IO] [11=W Axis]
            return "INPUT 0 11";
        }

        public static string READ_SPEED_Z_PICK()
        {
            // READ SPEED Z AXIS WHILE PICKING
            return "RSPZ 0";
        }

        public static string READ_SPEED_Z_PLACE()
        {
            // READ SPEED Z AXIS WHILE PLACING
            return "RSPZ 1";
        }

        public static string READ_REPICK_AND_VACUUM_TIME()
        {
            // READ RE-PICKING TIME OF GET AND VACUUM DETECTING TIME (ms)
            return "RVAC";
        }

        public static string READ_WORKING_ARM(string Station)
        {
            //select R-axis of W-axis of robotic arms to excute the pick/palce of wafer
            return "RSEE " + Station;
        }

        public static string READ_CURRENT_POSITION_ALL()
        {
            // RCP A will return T,R,Z,W and H axis
            return "RCP A";
        }

        public static string READ_CURRENT_POSITION(string Axis)
        {
            // RCP A will return T/R/Z/W/H axis
            return "RCP " + Axis;
        }

        public static string READ_TEACH_POSITION(string Station)
        {
            return "RCS " + Station;
        }

        public static string READ_WAFER_THICKNESS(int Slot)
        {
            return "RSTH " + Slot;
        }

        public static string READ_TOTAL_WAFER_SLOT(string Station)
        {
            //Read total wafer quantity  
            return "RSN " + Station;
        }

        public static string READ_SLOT_PITCH(string Station)
        {
            return "RPI " + Station;
        }

        public static string READ_OFFSET(string Station)
        {
            return "ROF " + Station;
        }

        public static string READ_STROKE(string Station)
        {
            return "RST " + Station;
        }

        public static string READ_Z_SPEED(int SizeGroup)
        {
            return "RSPM " + SizeGroup;
        }

        public static string READ_Z_ACCELERATION(int SizeGroup)
        {
            return "RADM " + SizeGroup;
        }

        public static string READ_MAXIMUM_THICKNESS(int SizeGroup)
        {
            return "RMAXM " + SizeGroup;
        }

        public static string READ_MINIMUNM_THICKNESS(int SizeGroup)
        {
            return "RMINM " + SizeGroup;
        }

        public static string READ_MAXIMUM_SKEWNESS(int SizeGroup)
        {
            return "RHORM " + SizeGroup;
        }

        public static string READ_MINIMUM_PITCH(int SizeGroup)
        {
            return "RGAPM " + SizeGroup;
        }

        #endregion Read Command

        public enum Stations
        {
            [Description("Top Arm Cassette Station")]
            A_TopArmCsst,
            [Description("Top Arm PreAligner Station")]
            B_TopArmPreAligner,
            [Description("Top Arm Chuck Station")]
            C_TopArmChuck,
            [Description("Bottom Arm Cassette Station")]
            D_BtmArmCsst,
            [Description("Bottom Arm PreAligner Station")]
            E_BtmArmPreAligner,
            [Description("Bottom Arm Chuck Station")]
            F_BtmArmChuck,
        }
    }
}
