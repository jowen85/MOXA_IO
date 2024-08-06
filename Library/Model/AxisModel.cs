using System.Collections.Generic;
using PolarisCommunication;

namespace Library.Model
{
    /// <summary>
    /// An individual Axis. Implements INotifyPropertyChanged such that the view can be updated of changed properties.
    /// </summary>
    public class AxisModel : ModelBase
    {
        public AxisModel(string name)
        {
            Name = name;
            _status = ""; // so we don't get null exceptions

            // We can only add mercury map registers here, as polaris server variables such
            // as homing state require the device number.
            // todo - need to note this as a limitation somewhere paw 11mar18
            // todo - perhaps this means that this is not the best place to add these
            // todo - data sources?
            EncoderPosition = new RegisterDouble("ENC_POS_FLOAT", Name, DataSourceType.MercuryMapRegister);
            ErrorStatus = new RegisterInt("ERROR_STATUS", Name, DataSourceType.MercuryMapRegister);
            DigitalOut = new RegisterInt("DIGOUT", Name, DataSourceType.MercuryMapRegister);

            DataSources = new List<IDataSource> {
                EncoderPosition,
                ErrorStatus,
                DigitalOut
            };
        }

        public string Name { get; }
        public int Number { get; set; }

        // todo - think about removing this paw 14feb19
        public double Acceleration { get;  set; }

        public RegisterDouble EncoderPosition { get; }
        public RegisterInt ErrorStatus { get; }
        public RegisterInt DigitalOut { get; }
        public RegisterInt AutoPhaseState { get; set; }
        public RegisterInt HomingState { get; set; }

        public List<IDataSource> DataSources { get; }

        #region PropertyChanged Event Properties

        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                FirePropertyChanged();
            }
        }

        private string _units;
        public string Units
        {
            get => _units;
            set
            {
                _units = value;
                FirePropertyChanged();
            }
        }

        private double _speed;
        public double Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                FirePropertyChanged();
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                FirePropertyChanged();
            }
        }

        private bool _inError;
        public bool InError
        {
            get => _inError;
            set
            {
                _inError = value;
                FirePropertyChanged();
            }
        }

        private double _position;
        public double Position
        {
            get => _position;
            set
            {
                _position = value;
                FirePropertyChanged();
            }
        }

        private bool _supportsAutophase;

        public bool SupportsAutophase
        {
            get => _supportsAutophase;
            set
            {
                _supportsAutophase = value;
                FirePropertyChanged(); // this isn't displayed in the UI so it might not be required
            }
        }

        private AutophaseAxisStatus _autoPhaseStatus;
        public AutophaseAxisStatus AutoPhaseStatus
        {
            get => _autoPhaseStatus;
            set
            {
                _autoPhaseStatus = value;
                FirePropertyChanged();
            }
        }

        private HomingAxisStatus _homingStatus;
        public HomingAxisStatus HomingStatus
        {
            get => _homingStatus;
            set
            {
                _homingStatus = value;
                FirePropertyChanged();
            }
        }

        private HomingDirection _homingDirection;

        public HomingDirection HomingDirection
        {
            get => _homingDirection;
            set
            {
                _homingDirection = value;
                FirePropertyChanged();
            }
        }

        private HomingType _homingType;
        public HomingType HomingType
        {
            get => _homingType;
            set
            {
                _homingType = value;
                FirePropertyChanged();
            }
        }

        // this is used for both home on speed and home off speed
        private double _homingSpeed;

        public double HomingSpeed
        {
            get => _homingSpeed;
            set
            {
                _homingSpeed = value;
                FirePropertyChanged();
            }
        }

        private double _homingAcceleration;
        public double HomingAcceleration
        {
            get => _homingAcceleration;
            set
            {
                _homingAcceleration = value;
                FirePropertyChanged();
            }
        }

        #endregion

      


    }

    public enum AutophaseAxisStatus
    {
        NotRequired = -2,
        Unavailable = -1,
        Ready = 0,
        Running,
        Complete,
        Error
    }

    public enum HomingAxisStatus
    {
        NotHomed = 0,
        Homing = 1,
        Homed = 2,
        Aborted = 3,
        InvalidInput
    }

    public enum HomingDirection
    {
        Plus = 1,
        Minus = -1
    }

    // must match the mciHomeType_e enum in mci.h
    public enum HomingType
    {
        ToIndex,
        ToSwitch,
        Virtual,
        FromLimitToIndex,
        FromLimitToSwitch,
        GalvoToIndex
    }

}
