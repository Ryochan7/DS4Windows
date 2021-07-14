using System;
using Sensorit.Base;

namespace DS4Windows
{
    public class SquareStickInfo
    {
        public bool lsMode;
        public bool rsMode;
        public double lsRoundness = 5.0;
        public double rsRoundness = 5.0;
    }

    public class StickDeadZoneInfo
    {
        public enum DeadZoneType : ushort
        {
            Radial,
            Axial
        }

        public const int DEFAULT_DEADZONE = 10;
        public const int DEFAULT_ANTIDEADZONE = 20;
        public const int DEFAULT_MAXZONE = 100;
        public const double DEFAULT_MAXOUTPUT = 100.0;
        public const bool DEFAULT_MAXOUTPUT_FORCE = false;
        public const int DEFAULT_FUZZ = 0;
        public const DeadZoneType DEFAULT_DEADZONE_TYPE = DeadZoneType.Radial;
        public const double DEFAULT_VERTICAL_SCALE = 100.0;
        public const double DEFAULT_OUTER_BIND_DEAD = 75.0;
        public const bool DEFAULT_OUTER_BIND_INVERT = false;

        public class AxisDeadZoneInfo
        {
            // DeadZone value from 0-127 (old bad convention)
            public int deadZone = DEFAULT_DEADZONE;
            public int antiDeadZone = DEFAULT_ANTIDEADZONE;
            public int maxZone = DEFAULT_MAXZONE;
            public double maxOutput = DEFAULT_MAXOUTPUT;

            public void Reset()
            {
                deadZone = DEFAULT_DEADZONE;
                antiDeadZone = DEFAULT_ANTIDEADZONE;
                maxZone = DEFAULT_MAXZONE;
                maxOutput = DEFAULT_MAXOUTPUT;
            }
        }

        // DeadZone value from 0-127 (old bad convention)
        public int deadZone;
        public int antiDeadZone;
        public int maxZone = DEFAULT_MAXZONE;
        public double maxOutput = DEFAULT_MAXOUTPUT;
        public bool maxOutputForce = DEFAULT_MAXOUTPUT_FORCE;
        public int fuzz = DEFAULT_FUZZ;
        public double verticalScale = DEFAULT_VERTICAL_SCALE;
        public DeadZoneType deadzoneType = DEFAULT_DEADZONE_TYPE;
        public double outerBindDeadZone = DEFAULT_OUTER_BIND_DEAD;
        public bool outerBindInvert = DEFAULT_OUTER_BIND_INVERT;
        public AxisDeadZoneInfo xAxisDeadInfo = new AxisDeadZoneInfo();
        public AxisDeadZoneInfo yAxisDeadInfo = new AxisDeadZoneInfo();

        public void Reset()
        {
            deadZone = 0;
            antiDeadZone = 0;
            maxZone = DEFAULT_MAXZONE;
            maxOutput = DEFAULT_MAXOUTPUT;
            maxOutputForce = DEFAULT_MAXOUTPUT_FORCE;

            fuzz = DEFAULT_FUZZ;
            verticalScale = DEFAULT_VERTICAL_SCALE;
            deadzoneType = DEFAULT_DEADZONE_TYPE;
            outerBindDeadZone = DEFAULT_OUTER_BIND_DEAD;
            outerBindInvert = DEFAULT_OUTER_BIND_INVERT;
            xAxisDeadInfo.Reset();
            yAxisDeadInfo.Reset();
        }
    }

    public class StickAntiSnapbackInfo
    {
        public const double DEFAULT_DELTA = 135;
        public const int DEFAULT_TIMEOUT = 50;
        public const bool DEFAULT_ENABLED = false;

        public bool enabled = DEFAULT_ENABLED;
        public double delta = DEFAULT_DELTA;
        public int timeout = DEFAULT_TIMEOUT;
    }

    public class TriggerDeadZoneZInfo
    {
        public byte deadZone; // Trigger deadzone is expressed in axis units
        public int antiDeadZone;
        public int maxZone = 100;
        public double maxOutput = 100.0;
    }

    public class GyroMouseInfo
    {
        public enum SmoothingMethod : byte
        {
            None,
            OneEuro,
            WeightedAverage,
        }

        public const double DEFAULT_MINCUTOFF = 1.0;
        public const double DEFAULT_BETA = 0.7;
        public const string DEFAULT_SMOOTH_TECHNIQUE = "one-euro";
        public const double DEFAULT_MIN_THRESHOLD = 1.0;

        public bool enableSmoothing = false;
        public double smoothingWeight = 0.5;
        public SmoothingMethod smoothingMethod;

        public double minCutoff = DEFAULT_MINCUTOFF;
        public double beta = DEFAULT_BETA;
        public double minThreshold = DEFAULT_MIN_THRESHOLD;

        public delegate void GyroMouseInfoEventHandler(GyroMouseInfo sender, EventArgs args);

        public double MinCutoff
        {
            get => minCutoff;
            set
            {
                if (minCutoff == value) return;
                minCutoff = value;
                MinCutoffChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event GyroMouseInfoEventHandler MinCutoffChanged;

        public double Beta
        {
            get => beta;
            set
            {
                if (beta == value) return;
                beta = value;
                BetaChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event GyroMouseInfoEventHandler BetaChanged;

        public void Reset()
        {
            minCutoff = DEFAULT_MINCUTOFF;
            beta = DEFAULT_BETA;
            enableSmoothing = false;
            smoothingMethod = SmoothingMethod.None;
            smoothingWeight = 0.5;
            minThreshold = DEFAULT_MIN_THRESHOLD;
        }

        public void ResetSmoothing()
        {
            enableSmoothing = false;
            ResetSmoothingMethods();
        }

        public void ResetSmoothingMethods()
        {
            smoothingMethod = SmoothingMethod.None;
        }

        public void DetermineSmoothMethod(string identier)
        {
            ResetSmoothingMethods();

            switch (identier)
            {
                case "weighted-average":
                    smoothingMethod = SmoothingMethod.WeightedAverage;
                    break;
                case "one-euro":
                    smoothingMethod = SmoothingMethod.OneEuro;
                    break;
                default:
                    smoothingMethod = SmoothingMethod.None;
                    break;
            }
        }

        public string SmoothMethodIdentifier()
        {
            string result = "none";
            if (smoothingMethod == SmoothingMethod.OneEuro)
            {
                result = "one-euro";
            }
            else if (smoothingMethod == SmoothingMethod.WeightedAverage)
            {
                result = "weighted-average";
            }

            return result;
        }

        public void SetRefreshEvents(OneEuroFilter euroFilter)
        {
            BetaChanged += (sender, args) =>
            {
                euroFilter.Beta = beta;
            };

            MinCutoffChanged += (sender, args) =>
            {
                euroFilter.MinCutoff = minCutoff;
            };
        }

        public void RemoveRefreshEvents()
        {
            BetaChanged = null;
            MinCutoffChanged = null;
        }
    }

    public class GyroMouseStickInfo
    {
        public enum SmoothingMethod : byte
        {
            None,
            OneEuro,
            WeightedAverage,
        }

        public const double DEFAULT_MINCUTOFF = 0.4;
        public const double DEFAULT_BETA = 0.7;
        public const string DEFAULT_SMOOTH_TECHNIQUE = "one-euro";

        public int deadZone;
        public int maxZone;
        public double antiDeadX;
        public double antiDeadY;
        public int vertScale;
        public bool maxOutputEnabled;
        public double maxOutput = 100.0;
        // Flags representing invert axis choices
        public uint inverted;
        public bool useSmoothing;
        public double smoothWeight;
        public SmoothingMethod smoothingMethod;
        public double minCutoff = DEFAULT_MINCUTOFF;
        public double beta = DEFAULT_BETA;

        public delegate void GyroMouseStickInfoEventHandler(GyroMouseStickInfo sender,
            EventArgs args);


        public double MinCutoff
        {
            get => minCutoff;
            set
            {
                if (minCutoff == value) return;
                minCutoff = value;
                MinCutoffChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event GyroMouseStickInfoEventHandler MinCutoffChanged;

        public double Beta
        {
            get => beta;
            set
            {
                if (beta == value) return;
                beta = value;
                BetaChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event GyroMouseStickInfoEventHandler BetaChanged;

        public void Reset()
        {
            deadZone = 30; maxZone = 830;
            antiDeadX = 0.4; antiDeadY = 0.4;
            inverted = 0; vertScale = 100;
            maxOutputEnabled = false; maxOutput = 100.0;

            minCutoff = DEFAULT_MINCUTOFF;
            beta = DEFAULT_BETA;
            smoothingMethod = SmoothingMethod.None;
            useSmoothing = false;
            smoothWeight = 0.5;
        }

        public void ResetSmoothing()
        {
            useSmoothing = false;
            ResetSmoothingMethods();
        }

        public void ResetSmoothingMethods()
        {
            smoothingMethod = SmoothingMethod.None;
        }

        public void DetermineSmoothMethod(string identier)
        {
            ResetSmoothingMethods();

            switch (identier)
            {
                case "weighted-average":
                    smoothingMethod = SmoothingMethod.WeightedAverage;
                    break;
                case "one-euro":
                    smoothingMethod = SmoothingMethod.OneEuro;
                    break;
                default:
                    smoothingMethod = SmoothingMethod.None;
                    break;
            }
        }

        public string SmoothMethodIdentifier()
        {
            string result = "none";
            switch (smoothingMethod)
            {
                case SmoothingMethod.WeightedAverage:
                    result = "weighted-average";
                    break;
                case SmoothingMethod.OneEuro:
                    result = "one-euro";
                    break;
                default:
                    break;
            }

            return result;
        }

        public void SetRefreshEvents(OneEuroFilter euroFilter)
        {
            BetaChanged += (sender, args) =>
            {
                euroFilter.Beta = beta;
            };

            MinCutoffChanged += (sender, args) =>
            {
                euroFilter.MinCutoff = minCutoff;
            };
        }

        public void RemoveRefreshEvents()
        {
            BetaChanged = null;
            MinCutoffChanged = null;
        }
    }

    public class GyroDirectionalSwipeInfo
    {
        public enum XAxisSwipe : ushort
        {
            Yaw,
            Roll,
        }

        public const string DEFAULT_TRIGGERS = "-1";
        public const int DEFAULT_GYRO_DIR_SPEED = 80; // degrees per second
        public const bool DEFAULT_TRIGGER_COND = true;
        public const bool DEFAULT_TRIGGER_TURNS = true;
        public const XAxisSwipe DEFAULT_X_AXIS = XAxisSwipe.Yaw;
        public const int DEFAULT_DELAY_TIME = 0;

        public int deadzoneX = DEFAULT_GYRO_DIR_SPEED;
        public int deadzoneY = DEFAULT_GYRO_DIR_SPEED;
        public string triggers = DEFAULT_TRIGGERS;
        public bool triggerCond = DEFAULT_TRIGGER_COND;
        public bool triggerTurns = DEFAULT_TRIGGER_TURNS;
        public XAxisSwipe xAxis = DEFAULT_X_AXIS;
        public int delayTime = DEFAULT_DELAY_TIME;

        public void Reset()
        {
            deadzoneX = DEFAULT_GYRO_DIR_SPEED;
            deadzoneY = DEFAULT_GYRO_DIR_SPEED;
            triggers = DEFAULT_TRIGGERS;
            triggerCond = DEFAULT_TRIGGER_COND;
            triggerTurns = DEFAULT_TRIGGER_TURNS;
            xAxis = DEFAULT_X_AXIS;
            delayTime = DEFAULT_DELAY_TIME;
        }
    }

    public class GyroControlsInfo
    {
        public const string DEFAULT_TRIGGERS = "-1";
        public const bool DEFAULT_TRIGGER_COND = true;
        public const bool DEFAULT_TRIGGER_TURNS = true;
        public const bool DEFAULT_TRIGGER_TOGGLE = false;

        public string triggers = DEFAULT_TRIGGERS;
        public bool triggerCond = DEFAULT_TRIGGER_COND;
        public bool triggerTurns = DEFAULT_TRIGGER_TURNS;
        public bool triggerToggle = DEFAULT_TRIGGER_TOGGLE;

        public void Reset()
        {
            triggers = DEFAULT_TRIGGERS;
            triggerCond = DEFAULT_TRIGGER_COND;
            triggerTurns = DEFAULT_TRIGGER_TURNS;
            triggerToggle = DEFAULT_TRIGGER_TOGGLE;
        }
    }

    public class ButtonMouseInfo
    {
        //public const double MOUSESTICKANTIOFFSET = 0.0128;
        public const double MOUSESTICKANTIOFFSET = 0.008;
        public const int DEFAULT_BUTTON_SENS = 25;
        public const double DEFAULT_BUTTON_VERTICAL_SCALE = 1.0;
        public const int DEFAULT_TEMP_SENS = -1;

        public int buttonSensitivity = DEFAULT_BUTTON_SENS;
        public int ButtonSensitivity
        {
            get => buttonSensitivity;
            set
            {
                if (buttonSensitivity == value) return;
                buttonSensitivity = value;
                ButtonMouseInfoChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler ButtonMouseInfoChanged;

        public bool mouseAccel;
        public int activeButtonSensitivity = DEFAULT_BUTTON_SENS;
        public int tempButtonSensitivity = DEFAULT_TEMP_SENS;
        public double mouseVelocityOffset = MOUSESTICKANTIOFFSET;
        public double buttonVerticalScale = DEFAULT_BUTTON_VERTICAL_SCALE;

        public ButtonMouseInfo()
        {
            ButtonMouseInfoChanged += ButtonMouseInfo_ButtonMouseInfoChanged;
        }

        private void ButtonMouseInfo_ButtonMouseInfoChanged(object sender, EventArgs e)
        {
            if (tempButtonSensitivity == DEFAULT_TEMP_SENS)
            {
                activeButtonSensitivity = buttonSensitivity;
            }
        }

        public void SetActiveButtonSensitivity(int sens)
        {
            activeButtonSensitivity = sens;
        }

        public void Reset()
        {
            buttonSensitivity = DEFAULT_BUTTON_SENS;
            mouseAccel = false;
            activeButtonSensitivity = DEFAULT_BUTTON_SENS;
            tempButtonSensitivity = DEFAULT_TEMP_SENS;
            mouseVelocityOffset = MOUSESTICKANTIOFFSET;
            buttonVerticalScale = DEFAULT_BUTTON_VERTICAL_SCALE;
        }
    }

    public enum LightbarMode : uint
    {
        None,
        DS4Win,
        Passthru,
    }

    public class LightbarDS4WinInfo
    {
        public bool useCustomLed;
        public bool ledAsBattery;
        public DS4Color m_CustomLed = new DS4Color(0, 0, 255);
        public DS4Color m_Led;
        public DS4Color m_LowLed;
        public DS4Color m_ChargingLed;
        public DS4Color m_FlashLed;
        public double rainbow;
        public double maxRainbowSat = 1.0;
        public int flashAt; // Battery % when flashing occurs. <0 means disabled
        public byte flashType;
        public int chargingType;
    }

    public class LightbarSettingInfo
    {
        public LightbarMode mode = LightbarMode.DS4Win;
        public LightbarDS4WinInfo ds4winSettings = new LightbarDS4WinInfo();
        public LightbarMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                ModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler ModeChanged;

        public LightbarSettingInfo()
        {
            /*ModeChanged += (sender, e) =>
            {
                if (mode != LightbarMode.DS4Win)
                {
                    ds4winSettings = null;
                }
            };
            */
        }
    }

    public class SteeringWheelSmoothingInfo
    {
        private double minCutoff = OneEuroFilterPair.DEFAULT_WHEEL_CUTOFF;
        private double beta = OneEuroFilterPair.DEFAULT_WHEEL_BETA;
        public bool enabled = false;

        public delegate void SmoothingInfoEventHandler(SteeringWheelSmoothingInfo sender, EventArgs args);

        public double MinCutoff
        {
            get => minCutoff;
            set
            {
                if (minCutoff == value) return;
                minCutoff = value;
                MinCutoffChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event SmoothingInfoEventHandler MinCutoffChanged;

        public double Beta
        {
            get => beta;
            set
            {
                if (beta == value) return;
                beta = value;
                BetaChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event SmoothingInfoEventHandler BetaChanged;

        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public void Reset()
        {
            MinCutoff = OneEuroFilterPair.DEFAULT_WHEEL_CUTOFF;
            Beta = OneEuroFilterPair.DEFAULT_WHEEL_BETA;
            enabled = false;
        }

        public void SetFilterAttrs(OneEuroFilter euroFilter)
        {
            euroFilter.MinCutoff = minCutoff;
            euroFilter.Beta = beta;
        }

        public void SetRefreshEvents(OneEuroFilter euroFilter)
        {
            BetaChanged += (sender, args) =>
            {
                euroFilter.Beta = beta;
            };

            MinCutoffChanged += (sender, args) =>
            {
                euroFilter.MinCutoff = minCutoff;
            };
        }
    }


    public class TouchpadRelMouseSettings
    {
        public const double DEFAULT_ANG_DEGREE = 0.0;
        public const double DEFAULT_ANG_RAD = DEFAULT_ANG_DEGREE * Math.PI / 180.0;
        public const double DEFAULT_MIN_THRESHOLD = 1.0;

        public double rotation = DEFAULT_ANG_RAD;
        public double minThreshold = DEFAULT_MIN_THRESHOLD;

        public void Reset()
        {
            rotation = DEFAULT_ANG_RAD;
            minThreshold = DEFAULT_MIN_THRESHOLD;
        }
    }

    public class TouchpadAbsMouseSettings
    {
        public const double DEFAULT_MAXZONE_X = 90.0;
        public const double DEFAULT_MAXZONE_Y = 90.0;
        public const double DEFAULT_MINPOS_X = 0.0;
        public const double DEFAULT_MINPOS_Y = 0.0;
        public const double DEFAULT_MAXPOS_X = 100.0;
        public const double DEFAULT_MAXPOS_Y = 100.0;
        public const bool DEFAULT_SNAP_CENTER = false;

        public double maxZoneX = DEFAULT_MAXZONE_X;
        public double maxZoneY = DEFAULT_MAXZONE_Y;
        public double minPosX = DEFAULT_MINPOS_X;
        public double minPosY = DEFAULT_MINPOS_Y;
        public double maxPosX = DEFAULT_MAXPOS_X;
        public double maxPosY = DEFAULT_MAXPOS_Y;
        public bool snapToCenter = DEFAULT_SNAP_CENTER;

        public void Reset()
        {
            maxZoneX = DEFAULT_MAXZONE_X;
            maxZoneY = DEFAULT_MAXZONE_Y;
            minPosX = DEFAULT_MINPOS_X;
            minPosY = DEFAULT_MINPOS_Y;
            maxPosX = DEFAULT_MAXPOS_X;
            maxPosY = DEFAULT_MAXPOS_Y;
            snapToCenter = DEFAULT_SNAP_CENTER;
        }
    }

    public enum StickMode : uint
    {
        None,
        Controls,
        FlickStick,
    }

    public enum TriggerMode : uint
    {
        Normal,
        TwoStage,
    }

    public enum TwoStageTriggerMode : uint
    {
        Disabled,
        Normal,
        ExclusiveButtons,
        HairTrigger,
        HipFire,
        HipFireExclusiveButtons,
    }

    public class FlickStickSettings
    {
        public const double DEFAULT_FLICK_THRESHOLD = 0.9;
        public const double DEFAULT_FLICK_TIME = 0.1;  // In seconds
        public const double DEFAULT_REAL_WORLD_CALIBRATION = 5.3;
        public const double DEFAULT_MIN_ANGLE_THRESHOLD = 0.0;

        public const double DEFAULT_MINCUTOFF = 0.4;
        public const double DEFAULT_BETA = 0.4;

        public double flickThreshold = DEFAULT_FLICK_THRESHOLD;
        public double flickTime = DEFAULT_FLICK_TIME; // In seconds
        public double realWorldCalibration = DEFAULT_REAL_WORLD_CALIBRATION;
        public double minAngleThreshold = DEFAULT_MIN_ANGLE_THRESHOLD;

        public double minCutoff = DEFAULT_MINCUTOFF;
        public double beta = DEFAULT_BETA;

        public delegate void FlickStickSettingsEventHandler(FlickStickSettings sender,
           EventArgs args);

        public double MinCutoff
        {
            get => minCutoff;
            set
            {
                if (minCutoff == value) return;
                minCutoff = value;
                MinCutoffChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event FlickStickSettingsEventHandler MinCutoffChanged;

        public double Beta
        {
            get => beta;
            set
            {
                if (beta == value) return;
                beta = value;
                BetaChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event FlickStickSettingsEventHandler BetaChanged;

        public void Reset()
        {
            flickThreshold = DEFAULT_FLICK_THRESHOLD;
            flickTime = DEFAULT_FLICK_TIME;
            realWorldCalibration = DEFAULT_REAL_WORLD_CALIBRATION;
            minAngleThreshold = DEFAULT_MIN_ANGLE_THRESHOLD;

            minCutoff = DEFAULT_MINCUTOFF;
            beta = DEFAULT_BETA;
        }

        public void SetRefreshEvents(OneEuroFilter euroFilter)
        {
            BetaChanged += (sender, args) =>
            {
                euroFilter.Beta = beta;
            };

            MinCutoffChanged += (sender, args) =>
            {
                euroFilter.MinCutoff = minCutoff;
            };
        }

        public void RemoveRefreshEvents()
        {
            BetaChanged = null;
            MinCutoffChanged = null;
        }
    }

    public class StickControlSettings
    {
        public void Reset()
        {
        }
    }

    public class StickModeSettings
    {
        public FlickStickSettings flickSettings = new FlickStickSettings();
        public StickControlSettings controlSettings = new StickControlSettings();
    }

    public class StickOutputSetting
    {
        public StickMode mode = StickMode.Controls;
        public StickModeSettings outputSettings = new StickModeSettings();

        public void ResetSettings()
        {
            mode = StickMode.Controls;
            outputSettings.controlSettings.Reset();
            outputSettings.flickSettings.Reset();
        }
    }

    public class TriggerOutputSettings
    {
        private const TwoStageTriggerMode DEFAULT_TRIG_MODE = TwoStageTriggerMode.Disabled;
        private const int DEFAULT_HIP_TIME = 100;
        private const InputDevices.TriggerEffects DEFAULT_TRIGGER_EFFECT = InputDevices.TriggerEffects.None;

        //public TriggerMode mode = TriggerMode.Normal;
        public TwoStageTriggerMode twoStageMode = DEFAULT_TRIG_MODE;
        public TwoStageTriggerMode TwoStageMode
        {
            get => twoStageMode;
            set
            {
                if (twoStageMode == value) return;
                twoStageMode = value;
                TwoStageModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TwoStageModeChanged;

        public int hipFireMS = DEFAULT_HIP_TIME;
        public InputDevices.TriggerEffects triggerEffect = DEFAULT_TRIGGER_EFFECT;
        public InputDevices.TriggerEffects TriggerEffect
        {
            get => triggerEffect;
            set
            {
                if (triggerEffect == value) return;
                triggerEffect = value;
                TriggerEffectChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TriggerEffectChanged;

        public void ResetSettings()
        {
            //mode = TriggerMode.Normal;
            twoStageMode = DEFAULT_TRIG_MODE;
            hipFireMS = DEFAULT_HIP_TIME;
            triggerEffect = DEFAULT_TRIGGER_EFFECT;
            TwoStageModeChanged?.Invoke(this, EventArgs.Empty);
            TriggerEffectChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ResetEvents()
        {
            TwoStageModeChanged = null;
            TriggerEffectChanged = null;
        }
    }
}