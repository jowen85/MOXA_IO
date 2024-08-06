using ConfigManager;
using IOManager;
using Core.Facilities;
using Prism.Events;
using System.Collections.Generic;
using Core.Enums;
using System.Diagnostics;

namespace Core.Variables
{
    public class GlobalVar
    {
        public static SystemCFG systemCFG; //System configuration
        public static IBaseIO IIO; //Digital IO
        public static ILogger ILog; //Log

        #region UserDetails
        public static bool IsLoggedIn = false;

        public static string UserID;
        public static string UserName;
        public static string DisplayName;

        #endregion UserDetails

        #region OEE
        public static Machine_Status CurrentMachineStatus;

        public static string StartTime;
        public static string FinishedTime;
        public static string ElapsedTime;
        public static string UPH;
        public static string MTBA;
        public static string MTTA;
        public static string MTTR;
        public static string MTBF;

        public static Stopwatch swElapseTime;

        #endregion OEE

        #region Events
        public static IEventAggregator Publisher_DeviceConnectivity = new EventAggregator();
        public static IEventAggregator Publisher_Alarm = new EventAggregator();
        public static IEventAggregator Publisher_MachineOperation = new EventAggregator();
        public static IEventAggregator Publisher_MachineStatus = new EventAggregator();
        #endregion Events

        #region Sequence
        public static bool IsRun = false;
        public static bool IsInitDone = false;
        public static bool IsInitNeeded = true;

        #endregion Sequence


    }
}
