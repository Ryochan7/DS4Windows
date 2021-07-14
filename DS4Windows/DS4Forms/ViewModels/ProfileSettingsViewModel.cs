using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using DS4Windows;

namespace DS4WinWPF.DS4Forms.ViewModels
{
    public class ProfileSettingsViewModel
    {
        private int device;
        public int Device { get => device; }

        private int funcDevNum;
        public int FuncDevNum { get => funcDevNum; }

        private ImageBrush lightbarImgBrush = new ImageBrush();
        private SolidColorBrush lightbarColBrush = new SolidColorBrush();

        public int LightbarModeIndex
        {
            get
            {
                int index = 0;
                switch(Global.LightbarSettingsInfo[device].Mode)
                {
                    case LightbarMode.DS4Win:
                        index = 0; break;
                    case LightbarMode.Passthru:
                        index = 1; break;
                    default: break;
                }

                return index;
            }
            set
            {
                LightbarMode temp = LightbarMode.DS4Win;
                switch(value)
                {
                    case 0:
                        temp = LightbarMode.DS4Win; break;
                    case 1:
                        temp = LightbarMode.Passthru; break;
                    default: break;
                }

                Global.LightbarSettingsInfo[device].Mode = temp;
                LightbarModeIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LightbarModeIndexChanged;

        public Visibility DS4WinLightVisible
        {
            get
            {
                Visibility temp = Visibility.Visible;
                switch(Global.LightbarSettingsInfo[device].Mode)
                {
                    case LightbarMode.DS4Win:
                        temp = Visibility.Visible; break;
                    case LightbarMode.Passthru:
                        temp = Visibility.Collapsed; break;
                }

                return temp;
            }
        }
        public event EventHandler DS4WinLightVisibleChanged;

        public System.Windows.Media.Brush LightbarBrush
        {
            get
            {
                System.Windows.Media.Brush tempBrush;
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_Led;
                if (!RainbowExists)
                {
                    lightbarColBrush.Color = new System.Windows.Media.Color()
                    {
                        A = 255,
                        R = color.red,
                        G = color.green,
                        B = color.blue
                    };
                    tempBrush = lightbarColBrush as System.Windows.Media.Brush;
                }
                else
                {
                    tempBrush = lightbarImgBrush as System.Windows.Media.Brush;
                }

                return tempBrush;
            }
        }
        public event EventHandler LightbarBrushChanged;

        public System.Windows.Media.Color MainColor
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_Led; //ref Global.MainColor[device];
                return new System.Windows.Media.Color()
                {
                    A = 255,
                    R = color.red,
                    G = color.green,
                    B = color.blue
                };
            }
        }
        public event EventHandler MainColorChanged;

        public string MainColorString
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_Led; //ref Global.MainColor[device];
                return $"#FF{color.red.ToString("X2")}{color.green.ToString("X2")}{color.blue.ToString("X2")}";
                /*return new System.Windows.Media.Color()
                {
                    A = 255,
                    R = color.red,
                    G = color.green,
                    B = color.blue
                }.ToString();
                */
            }
        }
        public event EventHandler MainColorStringChanged;

        public int MainColorR
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.red;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.red = (byte)value;
                MainColorRChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler MainColorRChanged;

        public string MainColorRString
        {
            get => $"#{ Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.red.ToString("X2")}FF0000";
        }
        public event EventHandler MainColorRStringChanged;

        public int MainColorG
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.green;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.green = (byte)value;
                MainColorGChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler MainColorGChanged;

        public string MainColorGString
        {
            get => $"#{ Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.green.ToString("X2")}00FF00";
        }
        public event EventHandler MainColorGStringChanged;

        public int MainColorB
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.blue;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.blue = (byte)value;
                MainColorBChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler MainColorBChanged;

        public string MainColorBString
        {
            get => $"#{ Global.LightbarSettingsInfo[device].ds4winSettings.m_Led.blue.ToString("X2")}0000FF";
        }
        public event EventHandler MainColorBStringChanged;

        public string LowColor
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed; //ref Global.LowColor[device];
                return $"#FF{color.red.ToString("X2")}{color.green.ToString("X2")}{color.blue.ToString("X2")}";
            }
        }
        public event EventHandler LowColorChanged;

        public int LowColorR
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.red;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.red = (byte)value;
                LowColorRChanged?.Invoke(this, EventArgs.Empty);
                LowColorRStringChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LowColorRChanged;

        public string LowColorRString
        {
            get => $"#{ Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.red.ToString("X2")}FF0000";
        }
        public event EventHandler LowColorRStringChanged;

        public int LowColorG
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.green;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.green = (byte)value;
                LowColorGChanged?.Invoke(this, EventArgs.Empty);
                LowColorGStringChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LowColorGChanged;

        public string LowColorGString
        {
            get => $"#{ Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.green.ToString("X2")}00FF00";
        }
        public event EventHandler LowColorGStringChanged;

        public int LowColorB
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.blue;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.blue = (byte)value;
                LowColorBChanged?.Invoke(this, EventArgs.Empty);
                LowColorBStringChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LowColorBChanged;

        public string LowColorBString
        {
            get => $"#{ Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed.blue.ToString("X2")}0000FF";
        }
        public event EventHandler LowColorBStringChanged;

        public System.Windows.Media.Color LowColorMedia
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed;
                return new System.Windows.Media.Color()
                {
                    A = 255,
                    R = color.red,
                    B = color.blue,
                    G = color.green
                };
            }
        }

        public int FlashTypeIndex
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.flashType; //Global.FlashType[device];
            set => Global.LightbarSettingsInfo[device].ds4winSettings.flashType = (byte)value;
        }

        public int FlashAt
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.flashAt; //Global.FlashAt[device];
            set => Global.LightbarSettingsInfo[device].ds4winSettings.flashAt = value;
        }

        public string FlashColor
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_FlashLed;
                if (color.red == 0 && color.green == 0 && color.blue == 0)
                {
                    color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_Led;
                }

                return $"#FF{color.red.ToString("X2")}{color.green.ToString("X2")}{color.blue.ToString("X2")}";
            }
        }
        public event EventHandler FlashColorChanged;

        public System.Windows.Media.Color FlashColorMedia
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_FlashLed;
                if (color.red == 0 && color.green == 0 && color.blue == 0)
                {
                    color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_Led;
                }

                return new System.Windows.Media.Color()
                {
                    A = 255,
                    R = color.red,
                    B = color.blue,
                    G = color.green
                };
            }
        }

        public int ChargingType
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.chargingType;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.chargingType = value;
                ChargingColorVisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool ColorBatteryPercent
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.ledAsBattery;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.ledAsBattery = value;
            }
        }

        public string ChargingColor
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_ChargingLed;
                return $"#FF{color.red.ToString("X2")}{color.green.ToString("X2")}{color.blue.ToString("X2")}";
            }
        }
        public event EventHandler ChargingColorChanged;

        public System.Windows.Media.Color ChargingColorMedia
        {
            get
            {
                ref DS4Color color = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_ChargingLed;
                return new System.Windows.Media.Color()
                {
                    A = 255,
                    R = color.red,
                    B = color.blue,
                    G = color.green
                };
            }
        }

        public Visibility ChargingColorVisible
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.chargingType == 3 ? Visibility.Visible : Visibility.Hidden;
        }
        public event EventHandler ChargingColorVisibleChanged;

        public double Rainbow
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.rainbow;
            set
            {
                Global.LightbarSettingsInfo[device].ds4winSettings.rainbow = value;
                RainbowChanged?.Invoke(this, EventArgs.Empty);
                RainbowExistsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler RainbowChanged;

        public bool RainbowExists
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.rainbow != 0.0;
        }

        public event EventHandler RainbowExistsChanged;

        public double MaxSatRainbow
        {
            get => Global.LightbarSettingsInfo[device].ds4winSettings.maxRainbowSat * 100.0;
            set => Global.LightbarSettingsInfo[device].ds4winSettings.maxRainbowSat = value / 100.0;
        }

        public int RumbleBoost
        {
            get => Global.RumbleBoost[device];
            set => Global.RumbleBoost[device] = (byte)value;
        }

        public int RumbleAutostopTime
        {
            // RumbleAutostopTime value is in milliseconds in XML config file, but GUI uses just seconds
            get => Global.getRumbleAutostopTime(device) / 1000;
            set => Global.setRumbleAutostopTime(device, value * 1000);
        }

        private bool heavyRumbleActive;
        public bool HeavyRumbleActive
        {
            get => heavyRumbleActive;
            set
            {
                heavyRumbleActive = value;
                HeavyRumbleActiveChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler HeavyRumbleActiveChanged;

        private bool lightRumbleActive;
        public bool LightRumbleActive
        {
            get => lightRumbleActive;
            set
            {
                lightRumbleActive = value;
                LightRumbleActiveChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LightRumbleActiveChanged;

        public bool UseControllerReadout
        {
            get => Global.DS4Mapping;
            set => Global.DS4Mapping = value;
        }

        public int ButtonMouseSensitivity
        {
            get => Global.ButtonMouseInfos[device].buttonSensitivity;
            set
            {
                int temp = Global.ButtonMouseInfos[device].buttonSensitivity;
                if (temp == value) return;
                Global.ButtonMouseInfos[device].ButtonSensitivity = value;
                ButtonMouseSensitivityChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler ButtonMouseSensitivityChanged;

        public int ButtonMouseVerticalScale
        {
            get => Convert.ToInt32(Global.ButtonMouseInfos[device].buttonVerticalScale * 100.0);
            set
            {
                double temp = Global.ButtonMouseInfos[device].buttonVerticalScale;
                double attemptValue = value * 0.01;
                if (temp == attemptValue) return;
                Global.ButtonMouseInfos[device].buttonVerticalScale = attemptValue;
                ButtonMouseVerticalScaleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler ButtonMouseVerticalScaleChanged;

        private double RawButtonMouseOffset
        {
            get => Global.ButtonMouseInfos[device].mouseVelocityOffset;
        }

        public double ButtonMouseOffset
        {
            get => Global.ButtonMouseInfos[device].mouseVelocityOffset * 100.0;
            set
            {
                double temp = Global.ButtonMouseInfos[device].mouseVelocityOffset * 100.0;
                if (temp == value) return;
                Global.ButtonMouseInfos[device].mouseVelocityOffset = value * 0.01;
                ButtonMouseOffsetChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler ButtonMouseOffsetChanged;

        private int outputMouseSpeed;
        public int OutputMouseSpeed
        {
            get => outputMouseSpeed;
            set
            {
                if (value == outputMouseSpeed) return;
                outputMouseSpeed = value;
                OutputMouseSpeedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler OutputMouseSpeedChanged;

        private double mouseOffsetSpeed;
        public double MouseOffsetSpeed
        {
            get => mouseOffsetSpeed;
            set
            {
                if (mouseOffsetSpeed == value) return;
                mouseOffsetSpeed = value;
                MouseOffsetSpeedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler MouseOffsetSpeedChanged;

        public bool MouseAcceleration
        {
            get => Global.ButtonMouseInfos[device].mouseAccel;
            set => Global.ButtonMouseInfos[device].mouseAccel = value;
        }

        public bool EnableTouchpadToggle
        {
            get => Global.EnableTouchToggle[device];
            set => Global.EnableTouchToggle[device] = value;
        }

        public bool EnableOutputDataToDS4
        {
            get => Global.EnableOutputDataToDS4[device];
            set => Global.EnableOutputDataToDS4[device] = value;
        }

        public bool LaunchProgramExists
        {
            get => !string.IsNullOrEmpty(Global.LaunchProgram[device]);
            set
            {
                if (!value) ResetLauchProgram();
            }
        }
        public event EventHandler LaunchProgramExistsChanged;

        public string LaunchProgram
        {
            get => Global.LaunchProgram[device];
        }
        public event EventHandler LaunchProgramChanged;

        public string LaunchProgramName
        {
            get
            {
                string temp = Global.LaunchProgram[device];
                if (!string.IsNullOrEmpty(temp))
                {
                    temp = Path.GetFileNameWithoutExtension(temp);
                }
                else
                {
                    temp = "Browse";
                }

                return temp;
            }
        }
        public event EventHandler LaunchProgramNameChanged;

        public ImageSource LaunchProgramIcon
        {
            get
            {
                ImageSource exeicon = null;
                string path = Global.LaunchProgram[device];
                if (File.Exists(path) && Path.GetExtension(path).ToLower() == ".exe")
                {
                    using (Icon ico = Icon.ExtractAssociatedIcon(path))
                    {
                        exeicon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                        exeicon.Freeze();
                    }
                }

                return exeicon;
            }
        }
        public event EventHandler LaunchProgramIconChanged;

        public bool DInputOnly
        {
            get => Global.DinputOnly[device];
            set
            {
                bool temp = Global.DinputOnly[device];
                if (temp == value) return;

                Global.DinputOnly[device] = value;
                DInputOnlyChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public EventHandler DInputOnlyChanged;

        public bool IdleDisconnectExists
        {
            get => Global.IdleDisconnectTimeout[device] != 0;
            set
            {
                Global.IdleDisconnectTimeout[device] = value ? 5 * 60 : 0;
                IdleDisconnectChanged?.Invoke(this, EventArgs.Empty);
                IdleDisconnectExistsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler IdleDisconnectExistsChanged;

        public int IdleDisconnect
        {
            get => Global.IdleDisconnectTimeout[device] / 60;
            set
            {
                int temp = Global.IdleDisconnectTimeout[device] / 60;
                if (temp == value) return;
                Global.IdleDisconnectTimeout[device] = value * 60;
                IdleDisconnectChanged?.Invoke(this, EventArgs.Empty);
                IdleDisconnectExistsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler IdleDisconnectChanged;

        private int tempBtPollRate;
        public int TempBTPollRateIndex
        {
            get => tempBtPollRate;
            set => tempBtPollRate = value;
        }

        public int ControllerTypeIndex
        {
            get
            {
                int type = 0;
                switch (Global.OutContType[device])
                {
                    case OutContType.X360:
                        type = 0;
                        break;

                    case OutContType.DS4:
                        type = 1;
                        break;

                    default: break;
                }

                return type;
            }
        }

        private int tempControllerIndex;
        public int TempControllerIndex
        {
            get => tempControllerIndex; set
            {
                tempControllerIndex = value;
                Global.outDevTypeTemp[device] = TempConType;
            }
        }

        public OutContType TempConType
        {
            get
            {
                OutContType result = OutContType.None;
                switch (tempControllerIndex)
                {
                    case 0:
                        result = OutContType.X360; break;
                    case 1:
                        result = OutContType.DS4; break;
                    default: result = OutContType.X360; break;
                }
                return result;
            }
        }

        public int GyroOutModeIndex
        {
            get
            {
                int index = 0;
                switch (Global.GyroOutputMode[device])
                {
                    case GyroOutMode.Controls:
                        index = 0; break;
                    case GyroOutMode.Mouse:
                        index = 1; break;
                    case GyroOutMode.MouseJoystick:
                        index = 2; break;
                    case GyroOutMode.DirectionalSwipe:
                        index = 3; break;
                    case GyroOutMode.Passthru:
                        index = 4; break;
                    default: break;
                }

                return index;
            }
            set
            {
                GyroOutMode temp = GyroOutMode.Controls;
                switch(value)
                {
                    case 0: break;
                    case 1:
                        temp = GyroOutMode.Mouse; break;
                    case 2:
                        temp = GyroOutMode.MouseJoystick; break;
                    case 3:
                        temp = GyroOutMode.DirectionalSwipe; break;
                    case 4:
                        temp = GyroOutMode.Passthru; break;
                    default: break;
                }

                GyroOutMode current = Global.GyroOutputMode[device];
                if (temp == current) return;
                Global.GyroOutputMode[device] = temp;
                GyroOutModeIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroOutModeIndexChanged;

        public OutContType ContType
        {
            get => Global.OutContType[device];
        }

        public int SASteeringWheelEmulationAxisIndex
        {
            get => (int)Global.SASteeringWheelEmulationAxis[device];
            set
            {
                int temp = (int)Global.SASteeringWheelEmulationAxis[device];
                if (temp == value) return;

                Global.SASteeringWheelEmulationAxis[device] = (SASteeringWheelEmulationAxisType)value;
                SASteeringWheelEmulationAxisIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler SASteeringWheelEmulationAxisIndexChanged;

        private int[] saSteeringRangeValues =
            new int[9] { 90, 180, 270, 360, 450, 720, 900, 1080, 1440 };
        public int SASteeringWheelEmulationRangeIndex
        {
            get
            {
                int index = 360;
                switch(Global.SASteeringWheelEmulationRange[device])
                {
                    case 90:
                        index = 0; break;
                    case 180:
                        index = 1; break;
                    case 270:
                        index = 2; break;
                    case 360:
                        index = 3; break;
                    case 450:
                        index = 4; break;
                    case 720:
                        index = 5; break;
                    case 900:
                        index = 6; break;
                    case 1080:
                        index = 7; break;
                    case 1440:
                        index = 8; break;
                    default: break;
                }

                return index;
            }
            set
            {
                int temp = saSteeringRangeValues[value];
                Global.SASteeringWheelEmulationRange[device] = temp;
            }
        }

        public int SASteeringWheelEmulationRange
        {
            get => Global.SASteeringWheelEmulationRange[device];
            set => Global.SASteeringWheelEmulationRange[device] = value;
        }

        public int SASteeringWheelFuzz
        {
            get => Global.SAWheelFuzzValues[device];
            set => Global.SAWheelFuzzValues[device] = value;
        }

        public bool SASteeringWheelUseSmoothing
        {
            get => Global.WheelSmoothInfo[device].Enabled;
            set
            {
                bool temp = Global.WheelSmoothInfo[device].Enabled;
                if (temp == value) return;
                Global.WheelSmoothInfo[device].Enabled = value;
                SASteeringWheelUseSmoothingChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler SASteeringWheelUseSmoothingChanged;

        public double SASteeringWheelSmoothMinCutoff
        {
            get => Global.WheelSmoothInfo[device].MinCutoff;
            set => Global.WheelSmoothInfo[device].MinCutoff = value;
        }

        public double SASteeringWheelSmoothBeta
        {
            get => Global.WheelSmoothInfo[device].Beta;
            set => Global.WheelSmoothInfo[device].Beta = value;
        }

        public double LSDeadZone
        {
            get => Math.Round(Global.LSModInfo[device].deadZone / 127d, 2);
            set
            {
                double temp = Math.Round(Global.LSModInfo[device].deadZone / 127d, 2);
                if (temp == value) return;
                Global.LSModInfo[device].deadZone = (int)Math.Round(value * 127d);
                LSDeadZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LSDeadZoneChanged;

        public double RSDeadZone
        {
            get => Math.Round(Global.RSModInfo[device].deadZone / 127d, 2);
            set
            {
                double temp = Math.Round(Global.RSModInfo[device].deadZone / 127d, 2);
                if (temp == value) return;
                Global.RSModInfo[device].deadZone = (int)Math.Round(value * 127d);
                RSDeadZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler RSDeadZoneChanged;

        public double LSMaxZone
        {
            get => Global.LSModInfo[device].maxZone / 100.0;
            set => Global.LSModInfo[device].maxZone = (int)(value * 100.0);
        }

        public double RSMaxZone
        {
            get => Global.RSModInfo[device].maxZone / 100.0;
            set => Global.RSModInfo[device].maxZone = (int)(value * 100.0);
        }

        public double LSAntiDeadZone
        {
            get => Global.LSModInfo[device].antiDeadZone / 100.0;
            set => Global.LSModInfo[device].antiDeadZone = (int)(value * 100.0);
        }

        public double RSAntiDeadZone
        {
            get => Global.RSModInfo[device].antiDeadZone / 100.0;
            set => Global.RSModInfo[device].antiDeadZone = (int)(value * 100.0);
        }

        public double LSVerticalScale
        {
            get => Global.LSModInfo[device].verticalScale / 100.0;
            set => Global.LSModInfo[device].verticalScale = value * 100.0;
        }

        public double LSMaxOutput
        {
            get => Global.LSModInfo[device].maxOutput / 100.0;
            set => Global.LSModInfo[device].maxOutput = value * 100.0;
        }

        public bool LSMaxOutputForce
        {
            get => Global.LSModInfo[device].maxOutputForce;
            set => Global.LSModInfo[device].maxOutputForce = value;
        }

        public double RSVerticalScale
        {
            get => Global.RSModInfo[device].verticalScale / 100.0;
            set => Global.RSModInfo[device].verticalScale = value * 100.0;
        }

        public double RSMaxOutput
        {
            get => Global.RSModInfo[device].maxOutput / 100.0;
            set => Global.RSModInfo[device].maxOutput = value * 100.0;
        }

        public bool RSMaxOutputForce
        {
            get => Global.RSModInfo[device].maxOutputForce;
            set => Global.RSModInfo[device].maxOutputForce = value;
        }

        public int LSDeadTypeIndex
        {
            get
            {
                int index = 0;
                switch(Global.LSModInfo[device].deadzoneType)
                {
                    case StickDeadZoneInfo.DeadZoneType.Radial:
                        break;
                    case StickDeadZoneInfo.DeadZoneType.Axial:
                        index = 1; break;
                    default: break;
                }

                return index;
            }
            set
            {
                StickDeadZoneInfo.DeadZoneType temp = StickDeadZoneInfo.DeadZoneType.Radial;
                switch(value)
                {
                    case 0: break;
                    case 1:
                        temp = StickDeadZoneInfo.DeadZoneType.Axial;
                        break;
                    default: break;
                }

                StickDeadZoneInfo.DeadZoneType current = Global.LSModInfo[device].deadzoneType;
                if (temp == current) return;
                Global.LSModInfo[device].deadzoneType = temp;
            }
        }

        public int RSDeadTypeIndex
        {
            get
            {
                int index = 0;
                switch (Global.RSModInfo[device].deadzoneType)
                {
                    case StickDeadZoneInfo.DeadZoneType.Radial:
                        break;
                    case StickDeadZoneInfo.DeadZoneType.Axial:
                        index = 1; break;
                    default: break;
                }

                return index;
            }
            set
            {
                StickDeadZoneInfo.DeadZoneType temp = StickDeadZoneInfo.DeadZoneType.Radial;
                switch (value)
                {
                    case 0: break;
                    case 1:
                        temp = StickDeadZoneInfo.DeadZoneType.Axial;
                        break;
                    default: break;
                }

                StickDeadZoneInfo.DeadZoneType current = Global.RSModInfo[device].deadzoneType;
                if (temp == current) return;
                Global.RSModInfo[device].deadzoneType = temp;
            }
        }

        public double LSSens
        {
            get => Global.LSSens[device];
            set => Global.LSSens[device] = value;
        }

        public double RSSens
        {
            get => Global.RSSens[device];
            set => Global.RSSens[device] = value;
        }

        public bool LSSquareStick
        {
            get => Global.SquStickInfo[device].lsMode;
            set => Global.SquStickInfo[device].lsMode = value;
        }

        public bool RSSquareStick
        {
            get => Global.SquStickInfo[device].rsMode;
            set => Global.SquStickInfo[device].rsMode = value;
        }

        public double LSSquareRoundness
        {
            get => Global.SquStickInfo[device].lsRoundness;
            set => Global.SquStickInfo[device].lsRoundness = value;
        }

        public double RSSquareRoundness
        {
            get => Global.SquStickInfo[device].rsRoundness;
            set => Global.SquStickInfo[device].rsRoundness = value;
        }

        public int LSOutputCurveIndex
        {
            get => Global.getLsOutCurveMode(device);
            set
            {
                Global.setLsOutCurveMode(device, value);
                LSCustomCurveSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int RSOutputCurveIndex
        {
            get => Global.getRsOutCurveMode(device);
            set
            {
                Global.setRsOutCurveMode(device, value);
                RSCustomCurveSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public double LSRotation
        {
            get => Global.LSRotation[device] * 180.0 / Math.PI;
            set => Global.LSRotation[device] = value * Math.PI / 180.0;
        }

        public double RSRotation
        {
            get => Global.RSRotation[device] * 180.0 / Math.PI;
            set => Global.RSRotation[device] = value * Math.PI / 180.0;
        }

        public bool LSCustomCurveSelected
        {
            get => Global.getLsOutCurveMode(device) == 6;
        }
        public event EventHandler LSCustomCurveSelectedChanged;

        public bool RSCustomCurveSelected
        {
            get => Global.getRsOutCurveMode(device) == 6;
        }
        public event EventHandler RSCustomCurveSelectedChanged;

        public string LSCustomCurve
        {
            get => Global.lsOutBezierCurveObj[device].CustomDefinition;
            set => Global.lsOutBezierCurveObj[device].InitBezierCurve(value, BezierCurve.AxisType.LSRS, true);
        }

        public string RSCustomCurve
        {
            get => Global.rsOutBezierCurveObj[device].CustomDefinition;
            set => Global.rsOutBezierCurveObj[device].InitBezierCurve(value, BezierCurve.AxisType.LSRS, true);
        }

        public int LSFuzz
        {
            get => Global.LSModInfo[device].fuzz;
            set => Global.LSModInfo[device].fuzz = value;
        }

        public int RSFuzz
        {
            get => Global.RSModInfo[device].fuzz;
            set => Global.RSModInfo[device].fuzz = value;
        }

        public bool LSAntiSnapback
        {
            get => Global.LSAntiSnapbackInfo[device].enabled;
            set => Global.LSAntiSnapbackInfo[device].enabled = value;
        }

        public bool RSAntiSnapback
        {
            get => Global.RSAntiSnapbackInfo[device].enabled;
            set => Global.RSAntiSnapbackInfo[device].enabled = value;
        }

        public double LSAntiSnapbackDelta
        {
            get => Global.LSAntiSnapbackInfo[device].delta;
            set => Global.LSAntiSnapbackInfo[device].delta = value;
        }

        public double RSAntiSnapbackDelta
        {
            get => Global.RSAntiSnapbackInfo[device].delta;
            set => Global.RSAntiSnapbackInfo[device].delta = value;
        }
        public int LSAntiSnapbackTimeout
        {
            get => Global.LSAntiSnapbackInfo[device].timeout;
            set => Global.LSAntiSnapbackInfo[device].timeout = value;
        }

        public int RSAntiSnapbackTimeout
        {
            get => Global.RSAntiSnapbackInfo[device].timeout;
            set => Global.RSAntiSnapbackInfo[device].timeout = value;
        }

        public bool LSOuterBindInvert
        {
            get => Global.LSModInfo[device].outerBindInvert;
            set => Global.LSModInfo[device].outerBindInvert = value;
        }

        public bool RSOuterBindInvert
        {
            get => Global.RSModInfo[device].outerBindInvert;
            set => Global.RSModInfo[device].outerBindInvert = value;
        }

        public double LSOuterBindDead
        {
            get => Global.LSModInfo[device].outerBindDeadZone / 100.0;
            set => Global.LSModInfo[device].outerBindDeadZone = value * 100.0;
        }

        public double RSOuterBindDead
        {
            get => Global.RSModInfo[device].outerBindDeadZone / 100.0;
            set => Global.RSModInfo[device].outerBindDeadZone = value * 100.0;
        }

        public int LSOutputIndex
        {
            get
            {
                int index = 0;
                switch (Global.LSOutputSettings[device].mode)
                {
                    case StickMode.None:
                        index = 0; break;
                    case StickMode.Controls:
                        index = 1; break;
                    case StickMode.FlickStick:
                        index = 2; break;
                    default: break;
                }
                return index;
            }
            set
            {
                StickMode temp = StickMode.None;
                switch(value)
                {
                    case 0:
                        temp = StickMode.None;
                        break;
                    case 1:
                        temp = StickMode.Controls;
                        break;
                    case 2:
                        temp = StickMode.FlickStick;
                        break;
                    default:
                        break;
                }

                StickMode current = Global.LSOutputSettings[device].mode;
                if (temp == current) return;
                Global.LSOutputSettings[device].mode = temp;
                LSOutputIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler LSOutputIndexChanged;

        public double LSFlickRWC
        {
            get => Global.LSOutputSettings[device].outputSettings.flickSettings.realWorldCalibration;
            set
            {
                Global.LSOutputSettings[device].outputSettings.flickSettings.realWorldCalibration = value;
            }
        }

        public double LSFlickThreshold
        {
            get => Global.LSOutputSettings[device].outputSettings.flickSettings.flickThreshold;
            set
            {
                Global.LSOutputSettings[device].outputSettings.flickSettings.flickThreshold = value;
            }
        }

        public double LSFlickTime
        {
            get => Global.LSOutputSettings[device].outputSettings.flickSettings.flickTime;
            set
            {
                Global.LSOutputSettings[device].outputSettings.flickSettings.flickTime = value;
            }
        }

        public double LSMinAngleThreshold
        {
            get => Global.LSOutputSettings[device].outputSettings.flickSettings.minAngleThreshold;
            set
            {
                Global.LSOutputSettings[device].outputSettings.flickSettings.minAngleThreshold = value;
            }
        }

        public int RSOutputIndex
        {
            get
            {
                int index = 0;
                switch (Global.RSOutputSettings[device].mode)
                {
                    case StickMode.None:
                        break;
                    case StickMode.Controls:
                        index = 1; break;
                    case StickMode.FlickStick:
                        index = 2; break;
                    default: break;
                }
                return index;
            }
            set
            {
                StickMode temp = StickMode.None;
                switch (value)
                {
                    case 0:
                        temp = StickMode.None;
                        break;
                    case 1:
                        temp = StickMode.Controls;
                        break;
                    case 2:
                        temp = StickMode.FlickStick;
                        break;
                    default:
                        break;
                }

                StickMode current = Global.RSOutputSettings[device].mode;
                if (temp == current) return;
                Global.RSOutputSettings[device].mode = temp;
                RSOutputIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler RSOutputIndexChanged;

        public double RSFlickRWC
        {
            get => Global.RSOutputSettings[device].outputSettings.flickSettings.realWorldCalibration;
            set
            {
                Global.RSOutputSettings[device].outputSettings.flickSettings.realWorldCalibration = value;
            }
        }

        public double RSFlickThreshold
        {
            get => Global.RSOutputSettings[device].outputSettings.flickSettings.flickThreshold;
            set
            {
                Global.RSOutputSettings[device].outputSettings.flickSettings.flickThreshold = value;
            }
        }

        public double RSFlickTime
        {
            get => Global.RSOutputSettings[device].outputSettings.flickSettings.flickTime;
            set
            {
                Global.RSOutputSettings[device].outputSettings.flickSettings.flickTime = value;
            }
        }

        public double RSMinAngleThreshold
        {
            get => Global.RSOutputSettings[device].outputSettings.flickSettings.minAngleThreshold;
            set
            {
                Global.RSOutputSettings[device].outputSettings.flickSettings.minAngleThreshold = value;
            }
        }

        public double L2DeadZone
        {
            get => Global.L2ModInfo[device].deadZone / 255.0;
            set
            {
                double temp = Global.L2ModInfo[device].deadZone / 255.0;
                if (temp == value) return;
                Global.L2ModInfo[device].deadZone = (byte)(value * 255.0);
                L2DeadZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler L2DeadZoneChanged;

        public double R2DeadZone
        {
            get => Global.R2ModInfo[device].deadZone / 255.0;
            set
            {
                double temp = Global.R2ModInfo[device].deadZone / 255.0;
                if (temp == value) return;
                Global.R2ModInfo[device].deadZone = (byte)(value * 255.0);
                R2DeadZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler R2DeadZoneChanged;

        public double L2MaxZone
        {
            get => Global.L2ModInfo[device].maxZone / 100.0;
            set => Global.L2ModInfo[device].maxZone = (int)(value * 100.0);
        }

        public double R2MaxZone
        {
            get => Global.R2ModInfo[device].maxZone / 100.0;
            set => Global.R2ModInfo[device].maxZone = (int)(value * 100.0);
        }

        public double L2AntiDeadZone
        {
            get => Global.L2ModInfo[device].antiDeadZone / 100.0;
            set => Global.L2ModInfo[device].antiDeadZone = (int)(value * 100.0);
        }

        public double R2AntiDeadZone
        {
            get => Global.R2ModInfo[device].antiDeadZone / 100.0;
            set => Global.R2ModInfo[device].antiDeadZone = (int)(value * 100.0);
        }

        public double L2MaxOutput
        {
            get => Global.L2ModInfo[device].maxOutput / 100.0;
            set => Global.L2ModInfo[device].maxOutput = value * 100.0;
        }

        public double R2MaxOutput
        {
            get => Global.R2ModInfo[device].maxOutput / 100.0;
            set => Global.R2ModInfo[device].maxOutput = value * 100.0;
        }

        public double L2Sens
        {
            get => Global.L2Sens[device];
            set => Global.L2Sens[device] = value;
        }

        public double R2Sens
        {
            get => Global.R2Sens[device];
            set => Global.R2Sens[device] = value;
        }

        public int L2OutputCurveIndex
        {
            get => Global.getL2OutCurveMode(device);
            set
            {
                Global.setL2OutCurveMode(device, value);
                L2CustomCurveSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int R2OutputCurveIndex
        {
            get => Global.getR2OutCurveMode(device);
            set
            {
                Global.setR2OutCurveMode(device, value);
                R2CustomCurveSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool L2CustomCurveSelected
        {
            get => Global.getL2OutCurveMode(device) == 6;
        }
        public event EventHandler L2CustomCurveSelectedChanged;

        public bool R2CustomCurveSelected
        {
            get => Global.getR2OutCurveMode(device) == 6;
        }
        public event EventHandler R2CustomCurveSelectedChanged;

        public string L2CustomCurve
        {
            get => Global.l2OutBezierCurveObj[device].CustomDefinition;
            set => Global.l2OutBezierCurveObj[device].InitBezierCurve(value, BezierCurve.AxisType.L2R2, true);
        }

        public string R2CustomCurve
        {
            get => Global.r2OutBezierCurveObj[device].CustomDefinition;
            set => Global.r2OutBezierCurveObj[device].InitBezierCurve(value, BezierCurve.AxisType.L2R2, true);
        }

        private List<TriggerModeChoice> triggerModeChoices = new List<TriggerModeChoice>()
        {
            new TriggerModeChoice("Normal", TriggerMode.Normal),
        };

        private List<TwoStageChoice> twoStageModeChoices = new List<TwoStageChoice>()
        {
            new TwoStageChoice("Disabled", TwoStageTriggerMode.Disabled),
            new TwoStageChoice("Normal", TwoStageTriggerMode.Normal),
            new TwoStageChoice("Exclusive", TwoStageTriggerMode.ExclusiveButtons),
            new TwoStageChoice("Hair Trigger", TwoStageTriggerMode.HairTrigger),
            new TwoStageChoice("Hip Fire", TwoStageTriggerMode.HipFire),
            new TwoStageChoice("Hip Fire Exclusive", TwoStageTriggerMode.HipFireExclusiveButtons),
        };
        public List<TwoStageChoice> TwoStageModeChoices { get => twoStageModeChoices; }

        public TwoStageTriggerMode L2TriggerMode
        {
            get => Global.L2OutputSettings[device].twoStageMode;
            set
            {
                TwoStageTriggerMode temp = Global.L2OutputSettings[device].TwoStageMode;
                if (temp == value) return;

                Global.L2OutputSettings[device].TwoStageMode = value;
                L2TriggerModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler L2TriggerModeChanged;

        public TwoStageTriggerMode R2TriggerMode
        {
            get => Global.R2OutputSettings[device].TwoStageMode;
            set
            {
                TwoStageTriggerMode temp = Global.R2OutputSettings[device].TwoStageMode;
                if (temp == value) return;

                Global.R2OutputSettings[device].twoStageMode = value;
                R2TriggerModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler R2TriggerModeChanged;

        public int L2HipFireTime
        {
            get => Global.L2OutputSettings[device].hipFireMS;
            set => Global.L2OutputSettings[device].hipFireMS = value;
        }

        public int R2HipFireTime
        {
            get => Global.R2OutputSettings[device].hipFireMS;
            set => Global.R2OutputSettings[device].hipFireMS = value;
        }

        private List<TriggerEffectChoice> triggerEffectChoices = new List<TriggerEffectChoice>()
        {
            new TriggerEffectChoice("None", DS4Windows.InputDevices.TriggerEffects.None),
            new TriggerEffectChoice("Full Click", DS4Windows.InputDevices.TriggerEffects.FullClick),
            new TriggerEffectChoice("Rigid", DS4Windows.InputDevices.TriggerEffects.Rigid),
            new TriggerEffectChoice("Pulse", DS4Windows.InputDevices.TriggerEffects.Pulse),
        };
        public List<TriggerEffectChoice> TriggerEffectChoices { get => triggerEffectChoices; }

        public DS4Windows.InputDevices.TriggerEffects L2TriggerEffect
        {
            get => Global.L2OutputSettings[device].triggerEffect;
            set
            {
                DS4Windows.InputDevices.TriggerEffects temp = Global.L2OutputSettings[device].TriggerEffect;
                if (temp == value) return;

                Global.L2OutputSettings[device].TriggerEffect = value;
            }
        }

        public DS4Windows.InputDevices.TriggerEffects R2TriggerEffect
        {
            get => Global.R2OutputSettings[device].triggerEffect;
            set
            {
                DS4Windows.InputDevices.TriggerEffects temp = Global.R2OutputSettings[device].TriggerEffect;
                if (temp == value) return;

                Global.R2OutputSettings[device].TriggerEffect = value;
            }
        }

        public double SXDeadZone
        {
            get => Global.SXDeadzone[device];
            set
            {
                double temp = Global.SXDeadzone[device];
                if (temp == value) return;
                Global.SXDeadzone[device] = value;
                SXDeadZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler SXDeadZoneChanged;

        public double SZDeadZone
        {
            get => Global.SZDeadzone[device];
            set
            {
                double temp = Global.SZDeadzone[device];
                if (temp == value) return;
                Global.SZDeadzone[device] = value;
                SZDeadZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler SZDeadZoneChanged;

        public double SXMaxZone
        {
            get => Global.SXMaxzone[device];
            set => Global.SXMaxzone[device] = value;
        }

        public double SZMaxZone
        {
            get => Global.SZMaxzone[device];
            set => Global.SZMaxzone[device] = value;
        }

        public double SXAntiDeadZone
        {
            get => Global.SXAntiDeadzone[device];
            set => Global.SXAntiDeadzone[device] = value;
        }

        public double SZAntiDeadZone
        {
            get => Global.SZAntiDeadzone[device];
            set => Global.SZAntiDeadzone[device] = value;
        }

        public double SXSens
        {
            get => Global.SXSens[device];
            set => Global.SXSens[device] = value;
        }

        public double SZSens
        {
            get => Global.SZSens[device];
            set => Global.SZSens[device] = value;
        }

        public int SXOutputCurveIndex
        {
            get => Global.getSXOutCurveMode(device);
            set
            {
                Global.setSXOutCurveMode(device, value);
                SXCustomCurveSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int SZOutputCurveIndex
        {
            get => Global.getSZOutCurveMode(device);
            set
            {
                Global.setSZOutCurveMode(device, value);
                SZCustomCurveSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool SXCustomCurveSelected
        {
            get => Global.getSXOutCurveMode(device) == 6;
        }
        public event EventHandler SXCustomCurveSelectedChanged;

        public bool SZCustomCurveSelected
        {
            get => Global.getSZOutCurveMode(device) == 6;
        }
        public event EventHandler SZCustomCurveSelectedChanged;

        public string SXCustomCurve
        {
            get => Global.sxOutBezierCurveObj[device].CustomDefinition;
            set => Global.sxOutBezierCurveObj[device].InitBezierCurve(value, BezierCurve.AxisType.SA, true);
        }

        public string SZCustomCurve
        {
            get => Global.szOutBezierCurveObj[device].CustomDefinition;
            set => Global.szOutBezierCurveObj[device].InitBezierCurve(value, BezierCurve.AxisType.SA, true);
        }

        public int TouchpadOutputIndex
        {
            get
            {
                int index = 0;
                switch (Global.TouchOutMode[device])
                {
                    case TouchpadOutMode.Mouse:
                        index = 0; break;
                    case TouchpadOutMode.Controls:
                        index = 1; break;
                    case TouchpadOutMode.AbsoluteMouse:
                        index = 2; break;
                    case TouchpadOutMode.Passthru:
                        index = 3; break;
                    default: break;
                }
                return index;
            }
            set
            {
                TouchpadOutMode temp = TouchpadOutMode.Mouse;
                switch (value)
                {
                    case 0: break;
                    case 1:
                        temp = TouchpadOutMode.Controls; break;
                    case 2:
                        temp = TouchpadOutMode.AbsoluteMouse; break;
                    case 3:
                        temp = TouchpadOutMode.Passthru; break;
                    default: break;
                }

                TouchpadOutMode current = Global.TouchOutMode[device];
                if (temp == current) return;
                Global.TouchOutMode[device] = temp;
                TouchpadOutputIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchpadOutputIndexChanged;

        public bool TouchSenExists
        {
            get => Global.TouchSensitivity[device] != 0;
            set
            {
                Global.TouchSensitivity[device] = value ? (byte)100 : (byte)0;
                TouchSenExistsChanged?.Invoke(this, EventArgs.Empty);
                TouchSensChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchSenExistsChanged;

        public int TouchSens
        {
            get => Global.TouchSensitivity[device];
            set
            {
                int temp = Global.TouchSensitivity[device];
                if (temp == value) return;
                Global.TouchSensitivity[device] = (byte)value;
                if (value == 0) TouchSenExistsChanged?.Invoke(this, EventArgs.Empty);
                TouchSensChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchSensChanged;

        public bool TouchScrollExists
        {
            get => Global.ScrollSensitivity[device] != 0;
            set
            {
                Global.ScrollSensitivity[device] = value ? (byte)100 : (byte)0;
                TouchScrollExistsChanged?.Invoke(this, EventArgs.Empty);
                TouchScrollChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchScrollExistsChanged;

        public int TouchScroll
        {
            get => Global.ScrollSensitivity[device];
            set
            {
                int temp = Global.ScrollSensitivity[device];
                if (temp == value) return;
                Global.ScrollSensitivity[device] = value;
                if (value == 0) TouchScrollExistsChanged?.Invoke(this, EventArgs.Empty);
                TouchScrollChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchScrollChanged;

        public bool TouchTapExists
        {
            get => Global.TapSensitivity[device] != 0;
            set
            {
                Global.TapSensitivity[device] = value ? (byte)100 : (byte)0;
                TouchTapExistsChanged?.Invoke(this, EventArgs.Empty);
                TouchTapChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchTapExistsChanged;

        public int TouchTap
        {
            get => Global.TapSensitivity[device];
            set
            {
                int temp = Global.TapSensitivity[device];
                if (temp == value) return;
                Global.TapSensitivity[device] = (byte)value;
                if (value == 0) TouchTapExistsChanged?.Invoke(this, EventArgs.Empty);
                TouchTapChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchTapChanged;

        public bool TouchDoubleTap
        {
            get => Global.DoubleTap[device];
            set
            {
                Global.DoubleTap[device] = value;
            }
        }
        
        public bool TouchJitter
        {
            get => Global.TouchpadJitterCompensation[device];
            set => Global.TouchpadJitterCompensation[device] = value;
        }

        private int[] touchpadInvertToValue = new int[4] { 0, 2, 1, 3 };
        public int TouchInvertIndex
        {
            get
            {
                int invert = Global.TouchpadInvert[device];
                int index = Array.IndexOf(touchpadInvertToValue, invert);
                return index;
            }
            set
            {
                int invert = touchpadInvertToValue[value];
                Global.TouchpadInvert[device] = invert;
            }
        }

        public bool LowerRightTouchRMB
        {
            get => Global.LowerRCOn[device];
            set
            {
                Global.LowerRCOn[device] = value;
            }
        }

        public bool TouchpadClickPassthru
        {
            get => Global.TouchClickPassthru[device];
            set
            {
                Global.TouchClickPassthru[device] = value;
            }
        }

        public bool StartTouchpadOff
        {
            get => Global.StartTouchpadOff[device];
            set
            {
                Global.StartTouchpadOff[device] = value;
            }
        }

        public double TouchRelMouseRotation
        {
            get => Global.TouchRelMouse[device].rotation * 180.0 / Math.PI;
            set => Global.TouchRelMouse[device].rotation = value * Math.PI / 180.0;
        }

        public double TouchRelMouseMinThreshold
        {
            get => Global.TouchRelMouse[device].minThreshold;
            set
            {
                double temp = Global.TouchRelMouse[device].minThreshold;
                if (temp == value) return;
                Global.TouchRelMouse[device].minThreshold = value;
            }
        }

        public bool TouchTrackball
        {
            get => Global.TrackballMode[device];
            set => Global.TrackballMode[device] = value;
        }

        public double TouchTrackballFriction
        {
            get => Global.TrackballFriction[device];
            set => Global.TrackballFriction[device] = value;
        }

        public double TouchAbsMouseMaxZoneX
        {
            get => Global.TouchAbsMouse[device].maxZoneX;
            set
            {
                double temp = Global.TouchAbsMouse[device].maxZoneX;
                if (temp == value) return;
                Global.TouchAbsMouse[device].maxZoneX = value;
            }
        }

        public double TouchAbsMouseMaxZoneY
        {
            get => Global.TouchAbsMouse[device].maxZoneY;
            set
            {
                double temp = Global.TouchAbsMouse[device].maxZoneY;
                if (temp == value) return;
                Global.TouchAbsMouse[device].maxZoneY = value;
            }
        }

        public double TouchAbsMouseMinPosX
        {
            get => Global.TouchAbsMouse[device].minPosX;
            set
            {
                double temp = Global.TouchAbsMouse[device].minPosX;
                if (temp == value) return;
                Global.TouchAbsMouse[device].minPosX = value;
            }
        }

        public double TouchAbsMouseMinPosY
        {
            get => Global.TouchAbsMouse[device].minPosY;
            set
            {
                double temp = Global.TouchAbsMouse[device].minPosY;
                if (temp == value) return;
                Global.TouchAbsMouse[device].minPosY = value;
            }
        }

        public double TouchAbsMouseMaxPosX
        {
            get => Global.TouchAbsMouse[device].maxPosX;
            set
            {
                double temp = Global.TouchAbsMouse[device].maxPosX;
                if (temp == value) return;
                Global.TouchAbsMouse[device].maxPosX = value;
            }
        }

        public double TouchAbsMouseMaxPosY
        {
            get => Global.TouchAbsMouse[device].maxPosY;
            set
            {
                double temp = Global.TouchAbsMouse[device].maxPosY;
                if (temp == value) return;
                Global.TouchAbsMouse[device].maxPosY = value;
            }
        }

        public bool TouchAbsMouseSnapCenter
        {
            get => Global.TouchAbsMouse[device].snapToCenter;
            set
            {
                bool temp = Global.TouchAbsMouse[device].snapToCenter;
                if (temp == value) return;
                Global.TouchAbsMouse[device].snapToCenter = value;
            }
        }

        public bool GyroMouseTurns
        {
            get => Global.GyroTriggerTurns[device];
            set => Global.GyroTriggerTurns[device] = value;
        }

        public int GyroSensitivity
        {
            get => Global.GyroSensitivity[device];
            set => Global.GyroSensitivity[device] = value;
        }

        public int GyroVertScale
        {
            get => Global.GyroSensVerticalScale[device];
            set => Global.GyroSensVerticalScale[device] = value;
        }

        public int GyroMouseEvalCondIndex
        {
            get => Global.getSATriggerCond(device) ? 0 : 1;
            set => Global.SetSaTriggerCond(device, value == 0 ? "and" : "or");
        }

        public int GyroMouseXAxis
        {
            get => Global.GyroMouseHorizontalAxis[device];
            set => Global.GyroMouseHorizontalAxis[device] = value;
        }

        public double GyroMouseMinThreshold
        {
            get => Global.GyroMouseInfo[device].minThreshold;
            set
            {
                double temp = Global.GyroMouseInfo[device].minThreshold;
                if (temp == value) return;
                Global.GyroMouseInfo[device].minThreshold = value;
            }
        }

        public bool GyroMouseInvertX
        {
            get => (Global.GyroInvert[device] & 2) == 2;
            set
            {
                if (value)
                {
                    Global.GyroInvert[device] |= 2;
                }
                else
                {
                    Global.GyroInvert[device] &= ~2;
                }
            }
        }

        public bool GyroMouseInvertY
        {
            get => (Global.GyroInvert[device] & 1) == 1;
            set
            {
                if (value)
                {
                    Global.GyroInvert[device] |= 1;
                }
                else
                {
                    Global.GyroInvert[device] &= ~1;
                }
            }
        }

        public bool GyroMouseSmooth
        {
            get => Global.GyroMouseInfo[device].enableSmoothing;
            set
            {
                GyroMouseInfo tempInfo = Global.GyroMouseInfo[device];
                if (tempInfo.enableSmoothing == value) return;

                Global.GyroMouseInfo[device].enableSmoothing = value;
                GyroMouseSmoothChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroMouseSmoothChanged;

        private int gyroMouseSmoothMethodIndex;
        public int GyroMouseSmoothMethodIndex
        {
            get
            {
                return gyroMouseSmoothMethodIndex;
            }
            set
            {
                if (gyroMouseSmoothMethodIndex == value) return;

                GyroMouseInfo tempInfo = Global.GyroMouseInfo[device];
                switch (value)
                {
                    case 0:
                        tempInfo.ResetSmoothingMethods();
                        tempInfo.smoothingMethod = GyroMouseInfo.SmoothingMethod.OneEuro;
                        break;
                    case 1:
                        tempInfo.ResetSmoothingMethods();
                        tempInfo.smoothingMethod = GyroMouseInfo.SmoothingMethod.WeightedAverage;
                        break;
                    default:
                        break;
                }

                gyroMouseSmoothMethodIndex = value;
                GyroMouseSmoothMethodIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroMouseSmoothMethodIndexChanged;

        public Visibility GyroMouseWeightAvgPanelVisibility
        {
            get
            {
                Visibility result = Visibility.Collapsed;
                switch (Global.GyroMouseInfo[device].smoothingMethod)
                {
                    case GyroMouseInfo.SmoothingMethod.WeightedAverage:
                        result = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                return result;
            }

        }
        public event EventHandler GyroMouseWeightAvgPanelVisibilityChanged;

        public Visibility GyroMouseOneEuroPanelVisibility
        {
            get
            {
                Visibility result = Visibility.Collapsed;
                switch(Global.GyroMouseInfo[device].smoothingMethod)
                {
                    case GyroMouseInfo.SmoothingMethod.OneEuro:
                    case GyroMouseInfo.SmoothingMethod.None:
                        result = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                return result;
            }

        }
        public event EventHandler GyroMouseOneEuroPanelVisibilityChanged;

        public double GyroMouseSmoothWeight
        {
            get => Global.GyroMouseInfo[device].smoothingWeight;
            set => Global.GyroMouseInfo[device].smoothingWeight = value;
        }

        public double GyroMouseOneEuroMinCutoff
        {
            get => Global.GyroMouseInfo[device].MinCutoff;
            set => Global.GyroMouseInfo[device].MinCutoff = value;
        }

        public double GyroMouseOneEuroBeta
        {
            get => Global.GyroMouseInfo[device].Beta;
            set => Global.GyroMouseInfo[device].Beta = value;
        }



        private int gyroMouseStickSmoothMethodIndex;
        public int GyroMouseStickSmoothMethodIndex
        {
            get
            {
                return gyroMouseStickSmoothMethodIndex;
            }
            set
            {
                if (gyroMouseStickSmoothMethodIndex == value) return;

                GyroMouseStickInfo tempInfo = Global.GyroMouseStickInf[device];
                switch (value)
                {
                    case 0:
                        tempInfo.ResetSmoothingMethods();
                        tempInfo.smoothingMethod = GyroMouseStickInfo.SmoothingMethod.OneEuro;
                        break;
                    case 1:
                        tempInfo.ResetSmoothingMethods();
                        tempInfo.smoothingMethod = GyroMouseStickInfo.SmoothingMethod.WeightedAverage;
                        break;
                    default:
                        break;
                }

                gyroMouseStickSmoothMethodIndex = value;
                GyroMouseStickSmoothMethodIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroMouseStickSmoothMethodIndexChanged;

        public Visibility GyroMouseStickWeightAvgPanelVisibility
        {
            get
            {
                Visibility result = Visibility.Collapsed;
                switch (Global.GyroMouseStickInf[device].smoothingMethod)
                {
                    case GyroMouseStickInfo.SmoothingMethod.WeightedAverage:
                        result = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                return result;
            }
        }
        public event EventHandler GyroMouseStickWeightAvgPanelVisibilityChanged;

        public Visibility GyroMouseStickOneEuroPanelVisibility
        {
            get
            {
                Visibility result = Visibility.Collapsed;
                switch (Global.GyroMouseStickInf[device].smoothingMethod)
                {
                    case GyroMouseStickInfo.SmoothingMethod.OneEuro:
                    case GyroMouseStickInfo.SmoothingMethod.None:
                        result = Visibility.Visible;
                        break;
                    default:
                        break;
                }

                return result;
            }
        }
        public event EventHandler GyroMouseStickOneEuroPanelVisibilityChanged;

        public double GyroMouseStickSmoothWeight
        {
            get => Global.GyroMouseStickInf[device].smoothWeight;
            set => Global.GyroMouseStickInf[device].smoothWeight = value;
        }

        public double GyroMouseStickOneEuroMinCutoff
        {
            get => Global.GyroMouseStickInf[device].MinCutoff;
            set => Global.GyroMouseStickInf[device].MinCutoff = value;
        }

        public double GyroMouseStickOneEuroBeta
        {
            get => Global.GyroMouseStickInf[device].Beta;
            set => Global.GyroMouseStickInf[device].Beta = value;
        }


        public int GyroMouseDeadZone
        {
            get => Global.GyroMouseDeadZone[device];
            set
            {
                Global.SetGyroMouseDeadZone(device, value, App.rootHub);

            }
        }

        public bool GyroMouseToggle
        {
            get => Global.GyroMouseToggle[device];
            set
            {
                Global.SetGyroMouseToggle(device, value, App.rootHub);
            }
        }

        public bool GyroMouseStickTurns
        {
            get => Global.GyroMouseStickTriggerTurns[device];
            set
            {
                Global.GyroMouseStickTriggerTurns[device] = value;
            }
        }

        public bool GyroMouseStickToggle
        {
            get => Global.GyroMouseStickToggle[device];
            set
            {
                Global.SetGyroMouseStickToggle(device, value, App.rootHub);
            }
        }

        public int GyroMouseStickDeadZone
        {
            get => Global.GyroMouseStickInf[device].deadZone;
            set => Global.GyroMouseStickInf[device].deadZone = value;
        }

        public int GyroMouseStickMaxZone
        {
            get => Global.GyroMouseStickInf[device].maxZone;
            set => Global.GyroMouseStickInf[device].maxZone = value;
        }

        public double GyroMouseStickAntiDeadX
        {
            get => Global.GyroMouseStickInf[device].antiDeadX * 100.0;
            set => Global.GyroMouseStickInf[device].antiDeadX = value * 0.01;
        }

        public double GyroMouseStickAntiDeadY
        {
            get => Global.GyroMouseStickInf[device].antiDeadY * 100.0;
            set => Global.GyroMouseStickInf[device].antiDeadY = value * 0.01;
        }

        public int GyroMouseStickVertScale
        {
            get => Global.GyroMouseStickInf[device].vertScale;
            set => Global.GyroMouseStickInf[device].vertScale = value;
        }

        public bool GyroMouseStickMaxOutputEnabled
        {
            get => Global.GyroMouseStickInf[device].maxOutputEnabled;
            set
            {
                bool temp = Global.GyroMouseStickInf[device].maxOutputEnabled;
                if (temp == value) return;
                Global.GyroMouseStickInf[device].maxOutputEnabled = value;
                GyroMouseStickMaxOutputChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroMouseStickMaxOutputChanged;

        public double GyroMouseStickMaxOutput
        {
            get => Global.GyroMouseStickInf[device].maxOutput;
            set => Global.GyroMouseStickInf[device].maxOutput = value;
        }

        public int GyroMouseStickEvalCondIndex
        {
            get => Global.GetSAMouseStickTriggerCond(device) ? 0 : 1;
            set => Global.SetSaMouseStickTriggerCond(device, value == 0 ? "and" : "or");
        }

        public int GyroMouseStickXAxis
        {
            get => Global.GyroMouseStickHorizontalAxis[device];
            set => Global.GyroMouseStickHorizontalAxis[device] = value;
        }

        public bool GyroMouseStickInvertX
        {
            get => (Global.GyroMouseStickInf[device].inverted & 1) == 1;
            set
            {
                if (value)
                {
                    Global.GyroMouseStickInf[device].inverted |= 1;
                }
                else
                {
                    uint temp = Global.GyroMouseStickInf[device].inverted;
                    Global.GyroMouseStickInf[device].inverted = (uint)(temp & ~1);
                }
            }
        }

        public bool GyroMouseStickInvertY
        {
            get => (Global.GyroMouseStickInf[device].inverted & 2) == 2;
            set
            {
                if (value)
                {
                    Global.GyroMouseStickInf[device].inverted |= 2;
                }
                else
                {
                    uint temp = Global.GyroMouseStickInf[device].inverted;
                    Global.GyroMouseStickInf[device].inverted = (uint)(temp & ~2);
                }
            }
        }

        public bool GyroMouseStickSmooth
        {
            get => Global.GyroMouseStickInf[device].useSmoothing;
            set => Global.GyroMouseStickInf[device].useSmoothing = value;
        }

        public double GyroMousetickSmoothWeight
        {
            get => Global.GyroMouseStickInf[device].smoothWeight;
            set => Global.GyroMouseStickInf[device].smoothWeight = value;
        }
        
        private string touchDisInvertString = "None";
        public string TouchDisInvertString
        {
            get => touchDisInvertString;
            set
            {
                touchDisInvertString = value;
                TouchDisInvertStringChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler TouchDisInvertStringChanged;


        private string gyroControlsTrigDisplay = "Always On";
        public string GyroControlsTrigDisplay
        {
            get => gyroControlsTrigDisplay;
            set
            {
                gyroControlsTrigDisplay = value;
                GyroControlsTrigDisplayChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroControlsTrigDisplayChanged;

        public bool GyroControlsTurns
        {
            get => Global.GyroControlsInf[device].triggerTurns;
            set => Global.GyroControlsInf[device].triggerTurns = value;
        }

        public int GyroControlsEvalCondIndex
        {
            get => Global.GyroControlsInf[device].triggerCond ? 0 : 1;
            set => Global.GyroControlsInf[device].triggerCond = value == 0 ? true : false;
        }

        public bool GyroControlsToggle
        {
            get => Global.GyroControlsInf[device].triggerToggle;
            set
            {
                Global.SetGyroControlsToggle(device, value, App.rootHub);
            }
        }


        private string gyroMouseTrigDisplay = "Always On";
        public string GyroMouseTrigDisplay
        {
            get => gyroMouseTrigDisplay;
            set
            {
                gyroMouseTrigDisplay = value;
                GyroMouseTrigDisplayChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroMouseTrigDisplayChanged;

        private string gyroMouseStickTrigDisplay = "Always On";
        public string GyroMouseStickTrigDisplay
        {
            get => gyroMouseStickTrigDisplay;
            set
            {
                gyroMouseStickTrigDisplay = value;
                GyroMouseStickTrigDisplayChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler GyroMouseStickTrigDisplayChanged;

        private string gyroSwipeTrigDisplay = "Always On";
        public string GyroSwipeTrigDisplay
        {
            get => gyroSwipeTrigDisplay;
            set
            {
                gyroSwipeTrigDisplay = value;
                GyroSwipeTrigDisplayChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler GyroSwipeTrigDisplayChanged;

        public bool GyroSwipeTurns
        {
            get => Global.GyroSwipeInf[device].triggerTurns;
            set => Global.GyroSwipeInf[device].triggerTurns = value;
        }

        public int GyroSwipeEvalCondIndex
        {
            get => Global.GyroSwipeInf[device].triggerCond ? 0 : 1;
            set => Global.GyroSwipeInf[device].triggerCond =  value == 0 ? true : false;
        }

        public int GyroSwipeXAxis
        {
            get => (int)Global.GyroSwipeInf[device].xAxis;
            set => Global.GyroSwipeInf[device].xAxis = (GyroDirectionalSwipeInfo.XAxisSwipe)value;
        }

        public int GyroSwipeDeadZoneX
        {
            get => Global.GyroSwipeInf[device].deadzoneX;
            set
            {
                Global.GyroSwipeInf[device].deadzoneX = value;
            }
        }

        public int GyroSwipeDeadZoneY
        {
            get => Global.GyroSwipeInf[device].deadzoneY;
            set
            {
                Global.GyroSwipeInf[device].deadzoneY = value;
            }
        }

        public int GyroSwipeDelayTime
        {
            get => Global.GyroSwipeInf[device].delayTime;
            set
            {
                Global.GyroSwipeInf[device].delayTime = value;
            }
        }

        private PresetMenuHelper presetMenuUtil;
        public PresetMenuHelper PresetMenuUtil
        {
            get => presetMenuUtil;
        }


        public ProfileSettingsViewModel(int device)
        {
            this.device = device;
            funcDevNum = device < ControlService.CURRENT_DS4_CONTROLLER_LIMIT ? device : 0;
            tempControllerIndex = ControllerTypeIndex;
            Global.outDevTypeTemp[device] = OutContType.X360;
            tempBtPollRate = Global.BTPollRate[device];

            outputMouseSpeed = CalculateOutputMouseSpeed(ButtonMouseSensitivity);
            mouseOffsetSpeed = RawButtonMouseOffset * outputMouseSpeed;

            /*ImageSourceConverter sourceConverter = new ImageSourceConverter();
            ImageSource temp = sourceConverter.
                ConvertFromString($"{Global.ASSEMBLY_RESOURCE_PREFIX}component/Resources/rainbowCCrop.png") as ImageSource;
            lightbarImgBrush.ImageSource = temp.Clone();
            */
            Uri tempResourceUri = new Uri($"{Global.ASSEMBLY_RESOURCE_PREFIX}component/Resources/rainbowCCrop.png");
            BitmapImage tempBitmap = new BitmapImage();
            tempBitmap.BeginInit();
            // Needed for some systems not using the System default color profile
            tempBitmap.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            tempBitmap.UriSource = tempResourceUri;
            tempBitmap.EndInit();
            lightbarImgBrush.ImageSource = tempBitmap.Clone();

            presetMenuUtil = new PresetMenuHelper(device);
            gyroMouseSmoothMethodIndex = FindGyroMouseSmoothMethodIndex();
            gyroMouseStickSmoothMethodIndex = FindGyroMouseStickSmoothMethodIndex();

            SetupEvents();
        }

        private int FindGyroMouseSmoothMethodIndex()
        {
            int result = 0;
            GyroMouseInfo tempInfo = Global.GyroMouseInfo[device];
            if (tempInfo.smoothingMethod == GyroMouseInfo.SmoothingMethod.OneEuro ||
                tempInfo.smoothingMethod == GyroMouseInfo.SmoothingMethod.None)
            {
                result = 0;
            }
            else if (tempInfo.smoothingMethod == GyroMouseInfo.SmoothingMethod.WeightedAverage)
            {
                result = 1;
            }

            return result;
        }

        private int FindGyroMouseStickSmoothMethodIndex()
        {
            int result = 0;
            GyroMouseStickInfo tempInfo = Global.GyroMouseStickInf[device];
            switch (tempInfo.smoothingMethod)
            {
                case GyroMouseStickInfo.SmoothingMethod.OneEuro:
                case GyroMouseStickInfo.SmoothingMethod.None:
                    result = 0;
                    break;
                case GyroMouseStickInfo.SmoothingMethod.WeightedAverage:
                    result = 1;
                    break;
                default:
                    break;
            }

            return result;
        }

        private void CalcProfileFlags(object sender, EventArgs e)
        {
            Global.CacheProfileCustomsFlags(device);
        }

        private void SetupEvents()
        {
            MainColorChanged += ProfileSettingsViewModel_MainColorChanged;
            MainColorRChanged += (sender, args) =>
            {
                MainColorRStringChanged?.Invoke(this, EventArgs.Empty);
                MainColorStringChanged?.Invoke(this, EventArgs.Empty);
                LightbarBrushChanged?.Invoke(this, EventArgs.Empty);
            };
            MainColorGChanged += (sender, args) =>
            {
                MainColorGStringChanged?.Invoke(this, EventArgs.Empty);
                MainColorStringChanged?.Invoke(this, EventArgs.Empty);
                LightbarBrushChanged?.Invoke(this, EventArgs.Empty);
            };
            MainColorBChanged += (sender, args) =>
            {
                MainColorBStringChanged?.Invoke(this, EventArgs.Empty);
                MainColorStringChanged?.Invoke(this, EventArgs.Empty);
                LightbarBrushChanged?.Invoke(this, EventArgs.Empty);
            };

            RainbowChanged += (sender, args) =>
            {
                LightbarBrushChanged?.Invoke(this, EventArgs.Empty);
            };

            ButtonMouseSensitivityChanged += (sender, args) =>
            {
                OutputMouseSpeed = CalculateOutputMouseSpeed(ButtonMouseSensitivity);
                MouseOffsetSpeed = RawButtonMouseOffset * OutputMouseSpeed;
            };

            LightbarModeIndexChanged += (sender, args) =>
            {
                DS4WinLightVisibleChanged?.Invoke(this, EventArgs.Empty);
            };

            GyroOutModeIndexChanged += CalcProfileFlags;
            SASteeringWheelEmulationAxisIndexChanged += CalcProfileFlags;
            LSOutputIndexChanged += CalcProfileFlags;
            RSOutputIndexChanged += CalcProfileFlags;
            ButtonMouseOffsetChanged += ProfileSettingsViewModel_ButtonMouseOffsetChanged;
            GyroMouseSmoothMethodIndexChanged += ProfileSettingsViewModel_GyroMouseSmoothMethodIndexChanged;
            GyroMouseStickSmoothMethodIndexChanged += ProfileSettingsViewModel_GyroMouseStickSmoothMethodIndexChanged;
        }

        private void ProfileSettingsViewModel_GyroMouseStickSmoothMethodIndexChanged(object sender, EventArgs e)
        {
            GyroMouseStickWeightAvgPanelVisibilityChanged?.Invoke(this, EventArgs.Empty);
            GyroMouseStickOneEuroPanelVisibilityChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ProfileSettingsViewModel_GyroMouseSmoothMethodIndexChanged(object sender, EventArgs e)
        {
            GyroMouseWeightAvgPanelVisibilityChanged?.Invoke(this, EventArgs.Empty);
            GyroMouseOneEuroPanelVisibilityChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ProfileSettingsViewModel_ButtonMouseOffsetChanged(object sender,
            EventArgs e)
        {
            MouseOffsetSpeed = RawButtonMouseOffset * OutputMouseSpeed;
        }

        private void ProfileSettingsViewModel_MainColorChanged(object sender, EventArgs e)
        {
            MainColorStringChanged?.Invoke(this, EventArgs.Empty);
            MainColorRChanged?.Invoke(this, EventArgs.Empty);
            MainColorGChanged?.Invoke(this, EventArgs.Empty);
            MainColorBChanged?.Invoke(this, EventArgs.Empty);
            LightbarBrushChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateFlashColor(System.Windows.Media.Color color)
        {
            Global.LightbarSettingsInfo[device].ds4winSettings.m_FlashLed = new DS4Color() { red = color.R, green = color.G, blue = color.B };
            FlashColorChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateMainColor(System.Windows.Media.Color color)
        {
            Global.LightbarSettingsInfo[device].ds4winSettings.m_Led = new DS4Color() { red = color.R, green = color.G, blue = color.B };
            MainColorChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateLowColor(System.Windows.Media.Color color)
        {
            ref DS4Color lowColor = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_LowLed;
            lowColor.red = color.R;
            lowColor.green = color.G;
            lowColor.blue = color.B;

            LowColorChanged?.Invoke(this, EventArgs.Empty);
            LowColorRChanged?.Invoke(this, EventArgs.Empty);
            LowColorGChanged?.Invoke(this, EventArgs.Empty);
            LowColorBChanged?.Invoke(this, EventArgs.Empty);
            LowColorRStringChanged?.Invoke(this, EventArgs.Empty);
            LowColorGStringChanged?.Invoke(this, EventArgs.Empty);
            LowColorBStringChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateForcedColor(System.Windows.Media.Color color)
        {
            if (device < ControlService.CURRENT_DS4_CONTROLLER_LIMIT)
            {
                DS4Color dcolor = new DS4Color() { red = color.R, green = color.G, blue = color.B };
                DS4LightBar.forcedColor[device] = dcolor;
                DS4LightBar.forcedFlash[device] = 0;
                DS4LightBar.forcelight[device] = true;
            }
        }

        public void StartForcedColor(System.Windows.Media.Color color)
        {
            if (device < ControlService.CURRENT_DS4_CONTROLLER_LIMIT)
            {
                DS4Color dcolor = new DS4Color() { red = color.R, green = color.G, blue = color.B };
                DS4LightBar.forcedColor[device] = dcolor;
                DS4LightBar.forcedFlash[device] = 0;
                DS4LightBar.forcelight[device] = true;
            }
        }

        public void EndForcedColor()
        {
            if (device < ControlService.CURRENT_DS4_CONTROLLER_LIMIT)
            {
                DS4LightBar.forcedColor[device] = new DS4Color(0, 0, 0);
                DS4LightBar.forcedFlash[device] = 0;
                DS4LightBar.forcelight[device] = false;
            }
        }

        public void UpdateChargingColor(System.Windows.Media.Color color)
        {
            ref DS4Color chargeColor = ref Global.LightbarSettingsInfo[device].ds4winSettings.m_ChargingLed;
            chargeColor.red = color.R;
            chargeColor.green = color.G;
            chargeColor.blue = color.B;
            ChargingColorChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateLaunchProgram(string path)
        {
            Global.LaunchProgram[device] = path;
            LaunchProgramExistsChanged?.Invoke(this, EventArgs.Empty);
            LaunchProgramChanged?.Invoke(this, EventArgs.Empty);
            LaunchProgramNameChanged?.Invoke(this, EventArgs.Empty);
            LaunchProgramIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ResetLauchProgram()
        {
            Global.LaunchProgram[device] = string.Empty;
            LaunchProgramExistsChanged?.Invoke(this, EventArgs.Empty);
            LaunchProgramChanged?.Invoke(this, EventArgs.Empty);
            LaunchProgramNameChanged?.Invoke(this, EventArgs.Empty);
            LaunchProgramIconChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateTouchDisInvert(ContextMenu menu)
        {
            int index = 0;
            List<int> triggerList = new List<int>();
            List<string> triggerName = new List<string>();
            
            foreach(MenuItem item in menu.Items)
            {
                if (item.IsChecked)
                {
                    triggerList.Add(index);
                    triggerName.Add(item.Header.ToString());
                }
                
                index++;
            }

            if (triggerList.Count == 0)
            {
                triggerList.Add(-1);
                triggerName.Add("None");
            }

            Global.TouchDisInvertTriggers[device] = triggerList.ToArray();
            TouchDisInvertString = string.Join(", ", triggerName.ToArray());
        }

        public void PopulateTouchDisInver(ContextMenu menu)
        {
            int[] triggers = Global.TouchDisInvertTriggers[device];
            int itemCount = menu.Items.Count;
            List<string> triggerName = new List<string>();
            foreach (int trigid in triggers)
            {
                if (trigid >= 0 && trigid < itemCount - 1)
                {
                    MenuItem current = menu.Items[trigid] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add(current.Header.ToString());
                }
                else if (trigid == -1)
                {
                    triggerName.Add("None");
                    break;
                }
            }

            if (triggerName.Count == 0)
            {
                triggerName.Add("None");
            }

            TouchDisInvertString = string.Join(", ", triggerName.ToArray());
        }

        public void UpdateGyroMouseTrig(ContextMenu menu, bool alwaysOnChecked)
        {
            int index = 0;
            List<int> triggerList = new List<int>();
            List<string> triggerName = new List<string>();

            int itemCount = menu.Items.Count;
            MenuItem alwaysOnItem = menu.Items[itemCount - 1] as MenuItem;
            if (alwaysOnChecked)
            {
                for (int i = 0; i < itemCount - 1; i++)
                {
                    MenuItem item = menu.Items[i] as MenuItem;
                    item.IsChecked = false;
                }
            }
            else
            {
                alwaysOnItem.IsChecked = false;
                foreach (MenuItem item in menu.Items)
                {
                    if (item.IsChecked)
                    {
                        triggerList.Add(index);
                        triggerName.Add(item.Header.ToString());
                    }

                    index++;
                }
            }

            if (triggerList.Count == 0)
            {
                triggerList.Add(-1);
                triggerName.Add("Always On");
                alwaysOnItem.IsChecked = true;
            }

            Global.SATriggers[device] = string.Join(",", triggerList.ToArray());
            GyroMouseTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        public void PopulateGyroMouseTrig(ContextMenu menu)
        {
            string[] triggers = Global.SATriggers[device].Split(',');
            int itemCount = menu.Items.Count;
            List<string> triggerName = new List<string>();
            foreach (string trig in triggers)
            {
                bool valid = int.TryParse(trig, out int trigid);
                if (valid && trigid >= 0 && trigid < itemCount - 1)
                {
                    MenuItem current = menu.Items[trigid] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add(current.Header.ToString());
                }
                else if (valid && trigid == -1)
                {
                    MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add("Always On");
                    break;
                }
            }

            if (triggerName.Count == 0)
            {
                MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                current.IsChecked = true;
                triggerName.Add("Always On");
            }

            GyroMouseTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        public void UpdateGyroMouseStickTrig(ContextMenu menu, bool alwaysOnChecked)
        {
            int index = 0;
            List<int> triggerList = new List<int>();
            List<string> triggerName = new List<string>();

            int itemCount = menu.Items.Count;
            MenuItem alwaysOnItem = menu.Items[itemCount - 1] as MenuItem;
            if (alwaysOnChecked)
            {
                for (int i = 0; i < itemCount - 1; i++)
                {
                    MenuItem item = menu.Items[i] as MenuItem;
                    item.IsChecked = false;
                }
            }
            else
            {
                alwaysOnItem.IsChecked = false;
                foreach (MenuItem item in menu.Items)
                {
                    if (item.IsChecked)
                    {
                        triggerList.Add(index);
                        triggerName.Add(item.Header.ToString());
                    }

                    index++;
                }
            }

            if (triggerList.Count == 0)
            {
                triggerList.Add(-1);
                triggerName.Add("Always On");
                alwaysOnItem.IsChecked = true;
            }

            Global.SAMousestickTriggers[device] = string.Join(",", triggerList.ToArray());
            GyroMouseStickTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        public void PopulateGyroMouseStickTrig(ContextMenu menu)
        {
            string[] triggers = Global.SAMousestickTriggers[device].Split(',');
            int itemCount = menu.Items.Count;
            List<string> triggerName = new List<string>();
            foreach (string trig in triggers)
            {
                bool valid = int.TryParse(trig, out int trigid);
                if (valid && trigid >= 0 && trigid < itemCount - 1)
                {
                    MenuItem current = menu.Items[trigid] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add(current.Header.ToString());
                }
                else if (valid && trigid == -1)
                {
                    MenuItem current = menu.Items[itemCount-1] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add("Always On");
                    break;
                }
            }

            if (triggerName.Count == 0)
            {
                MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                current.IsChecked = true;
                triggerName.Add("Always On");
            }

            GyroMouseStickTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        public void UpdateGyroSwipeTrig(ContextMenu menu, bool alwaysOnChecked)
        {
            int index = 0;
            List<int> triggerList = new List<int>();
            List<string> triggerName = new List<string>();

            int itemCount = menu.Items.Count;
            MenuItem alwaysOnItem = menu.Items[itemCount - 1] as MenuItem;
            if (alwaysOnChecked)
            {
                for (int i = 0; i < itemCount - 1; i++)
                {
                    MenuItem item = menu.Items[i] as MenuItem;
                    item.IsChecked = false;
                }
            }
            else
            {
                alwaysOnItem.IsChecked = false;
                foreach (MenuItem item in menu.Items)
                {
                    if (item.IsChecked)
                    {
                        triggerList.Add(index);
                        triggerName.Add(item.Header.ToString());
                    }

                    index++;
                }
            }

            if (triggerList.Count == 0)
            {
                triggerList.Add(-1);
                triggerName.Add("Always On");
                alwaysOnItem.IsChecked = true;
            }

            Global.GyroSwipeInf[device].triggers = string.Join(",", triggerList.ToArray());
            GyroSwipeTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        public void PopulateGyroSwipeTrig(ContextMenu menu)
        {
            string[] triggers = Global.GyroSwipeInf[device].triggers.Split(',');
            int itemCount = menu.Items.Count;
            List<string> triggerName = new List<string>();
            foreach (string trig in triggers)
            {
                bool valid = int.TryParse(trig, out int trigid);
                if (valid && trigid >= 0 && trigid < itemCount - 1)
                {
                    MenuItem current = menu.Items[trigid] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add(current.Header.ToString());
                }
                else if (valid && trigid == -1)
                {
                    MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add("Always On");
                    break;
                }
            }

            if (triggerName.Count == 0)
            {
                MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                current.IsChecked = true;
                triggerName.Add("Always On");
            }

            GyroSwipeTrigDisplay = string.Join(", ", triggerName.ToArray());
        }


        public void UpdateGyroControlsTrig(ContextMenu menu, bool alwaysOnChecked)
        {
            int index = 0;
            List<int> triggerList = new List<int>();
            List<string> triggerName = new List<string>();

            int itemCount = menu.Items.Count;
            MenuItem alwaysOnItem = menu.Items[itemCount - 1] as MenuItem;
            if (alwaysOnChecked)
            {
                for (int i = 0; i < itemCount - 1; i++)
                {
                    MenuItem item = menu.Items[i] as MenuItem;
                    item.IsChecked = false;
                }
            }
            else
            {
                alwaysOnItem.IsChecked = false;
                foreach (MenuItem item in menu.Items)
                {
                    if (item.IsChecked)
                    {
                        triggerList.Add(index);
                        triggerName.Add(item.Header.ToString());
                    }

                    index++;
                }
            }

            if (triggerList.Count == 0)
            {
                triggerList.Add(-1);
                triggerName.Add("Always On");
                alwaysOnItem.IsChecked = true;
            }

            Global.GyroControlsInf[device].triggers = string.Join(",", triggerList.ToArray());
            GyroControlsTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        public void PopulateGyroControlsTrig(ContextMenu menu)
        {
            string[] triggers = Global.GyroControlsInf[device].triggers.Split(',');
            int itemCount = menu.Items.Count;
            List<string> triggerName = new List<string>();
            foreach (string trig in triggers)
            {
                bool valid = int.TryParse(trig, out int trigid);
                if (valid && trigid >= 0 && trigid < itemCount - 1)
                {
                    MenuItem current = menu.Items[trigid] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add(current.Header.ToString());
                }
                else if (valid && trigid == -1)
                {
                    MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                    current.IsChecked = true;
                    triggerName.Add("Always On");
                    break;
                }
            }

            if (triggerName.Count == 0)
            {
                MenuItem current = menu.Items[itemCount - 1] as MenuItem;
                current.IsChecked = true;
                triggerName.Add("Always On");
            }

            GyroControlsTrigDisplay = string.Join(", ", triggerName.ToArray());
        }

        private int CalculateOutputMouseSpeed(int mouseSpeed)
        {
            int result = mouseSpeed * Mapping.MOUSESPEEDFACTOR;
            return result;
        }

        public void LaunchCurveEditor(string customDefinition)
        {
            // Custom curve editor web link clicked. Open the bezier curve editor web app usign the default browser app and pass on current custom definition as a query string parameter.
            // The Process.Start command using HTML page doesn't support query parameters, so if there is a custom curve definition then lookup the default browser executable name from a sysreg.
            string defaultBrowserCmd = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(customDefinition))
                {
                    string progId = String.Empty;
                    using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice"))
                    {
                        progId = userChoiceKey?.GetValue("Progid")?.ToString();
                    }

                    if (!String.IsNullOrEmpty(progId))
                    {
                        using (RegistryKey browserPathCmdKey = Registry.ClassesRoot.OpenSubKey($"{progId}\\shell\\open\\command"))
                        {
                            defaultBrowserCmd = browserPathCmdKey?.GetValue(null).ToString().ToLower();
                        }

                        if (!String.IsNullOrEmpty(defaultBrowserCmd))
                        {
                            int iStartPos = (defaultBrowserCmd[0] == '"' ? 1 : 0);
                            defaultBrowserCmd = defaultBrowserCmd.Substring(iStartPos, defaultBrowserCmd.LastIndexOf(".exe") + 4 - iStartPos);
                            if (Path.GetFileName(defaultBrowserCmd) == "launchwinapp.exe")
                                defaultBrowserCmd = String.Empty;
                        }

                        // Fallback to IE executable if the default browser HTML shell association is for some reason missing or is not set
                        if (String.IsNullOrEmpty(defaultBrowserCmd))
                            defaultBrowserCmd = "C:\\Program Files\\Internet Explorer\\iexplore.exe";

                        if (!File.Exists(defaultBrowserCmd))
                            defaultBrowserCmd = String.Empty;
                    }
                }

                // Launch custom bezier editor webapp using a default browser executable command or via a default shell command. The default shell exeution doesn't support query parameters.
                if (!String.IsNullOrEmpty(defaultBrowserCmd))
                    System.Diagnostics.Process.Start(defaultBrowserCmd, $"\"file:///{Global.exedirpath}\\BezierCurveEditor\\index.html?curve={customDefinition.Replace(" ", "")}\"");
                else
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo($"{Global.exedirpath}\\BezierCurveEditor\\index.html");
                    startInfo.UseShellExecute = true;
                    using (System.Diagnostics.Process temp = System.Diagnostics.Process.Start(startInfo))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogToGui($"ERROR. Failed to open {Global.exedirpath}\\BezierCurveEditor\\index.html web app. Check that the web file exits or launch it outside of DS4Windows application. {ex.Message}", true);
            }
        }

        public void UpdateLateProperties()
        {
            tempControllerIndex = ControllerTypeIndex;
            Global.outDevTypeTemp[device] = Global.OutContType[device];
            tempBtPollRate = Global.BTPollRate[device];
            outputMouseSpeed = CalculateOutputMouseSpeed(ButtonMouseSensitivity);
            mouseOffsetSpeed = RawButtonMouseOffset * outputMouseSpeed;
            gyroMouseSmoothMethodIndex = FindGyroMouseSmoothMethodIndex();
            gyroMouseStickSmoothMethodIndex = FindGyroMouseStickSmoothMethodIndex();
        }
    }

    public class PresetMenuHelper
    {
        public enum ControlSelection : uint
        {
            None,
            LeftStick,
            RightStick,
            DPad,
            FaceButtons,
        }

        private Dictionary<ControlSelection, string> presetInputLabelDict =
            new Dictionary<ControlSelection, string>()
            {
                [ControlSelection.None] = "None",
                [ControlSelection.DPad] = "DPad",
                [ControlSelection.LeftStick] = "Left Stick",
                [ControlSelection.RightStick] = "Right Stick",
                [ControlSelection.FaceButtons] = "Face Buttons",
            };

        public Dictionary<ControlSelection, string> PresetInputLabelDict
        {
            get => presetInputLabelDict;
        }

        public string PresetInputLabel
        {
            get => presetInputLabelDict[highlightControl];
        }

        private ControlSelection highlightControl = ControlSelection.None;

        public ControlSelection HighlightControl {
            get => highlightControl;
        }

        private int deviceNum;

        public PresetMenuHelper(int device)
        {
            deviceNum = device;
        }

        public ControlSelection PresetTagIndex(DS4Controls control)
        {
            ControlSelection controlInput = ControlSelection.None;
            switch (control)
            {
                case DS4Controls.DpadUp:
                case DS4Controls.DpadDown:
                case DS4Controls.DpadLeft:
                case DS4Controls.DpadRight:
                    controlInput = ControlSelection.DPad;
                    break;
                case DS4Controls.LXNeg:
                case DS4Controls.LXPos:
                case DS4Controls.LYNeg:
                case DS4Controls.LYPos:
                case DS4Controls.L3:
                    controlInput = ControlSelection.LeftStick;
                    break;
                case DS4Controls.RXNeg:
                case DS4Controls.RXPos:
                case DS4Controls.RYNeg:
                case DS4Controls.RYPos:
                case DS4Controls.R3:
                    controlInput = ControlSelection.RightStick;
                    break;
                case DS4Controls.Cross:
                case DS4Controls.Circle:
                case DS4Controls.Triangle:
                case DS4Controls.Square:
                    controlInput = ControlSelection.FaceButtons;
                    break;
                default:
                    break;
            }


            return controlInput;
        }

        public void SetHighlightControl(DS4Controls control)
        {
            ControlSelection controlInput = PresetTagIndex(control);
            highlightControl = controlInput;
        }

        public List<DS4Controls> ModifySettingWithPreset(int baseTag, int subTag)
        {
            List<object> actionBtns = new List<object>(5);
            List<DS4Controls> inputControls = new List<DS4Controls>(5);
            if (baseTag == 0)
            {
                actionBtns.AddRange(new object[5]
                {
                    null, null, null, null, null,
                });
            }
            else if (baseTag == 1)
            {
                switch(subTag)
                {
                    case 0:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.DpadUp, X360Controls.DpadDown,
                            X360Controls.DpadLeft, X360Controls.DpadRight, X360Controls.Unbound,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.DpadDown, X360Controls.DpadUp,
                            X360Controls.DpadRight, X360Controls.DpadLeft, X360Controls.Unbound,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.DpadUp, X360Controls.DpadDown,
                            X360Controls.DpadRight, X360Controls.DpadLeft, X360Controls.Unbound,
                        });
                        break;
                    case 3:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.DpadDown, X360Controls.DpadUp,
                            X360Controls.DpadLeft, X360Controls.DpadRight, X360Controls.Unbound,
                        });
                        break;
                    case 4:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.DpadRight, X360Controls.DpadLeft,
                            X360Controls.DpadUp, X360Controls.DpadDown, X360Controls.Unbound,
                        });
                        break;
                    case 5:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.DpadLeft, X360Controls.DpadRight,
                            X360Controls.DpadDown, X360Controls.DpadUp, X360Controls.Unbound,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 2)
            {
                switch (subTag)
                {
                    case 0:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.LYNeg, X360Controls.LYPos,
                            X360Controls.LXNeg, X360Controls.LXPos, X360Controls.LS,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.LYPos, X360Controls.LYNeg,
                            X360Controls.LXPos, X360Controls.LXNeg, X360Controls.LS,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.LYNeg, X360Controls.LYPos,
                            X360Controls.LXPos, X360Controls.LXNeg, X360Controls.LS,
                        });
                        break;
                    case 3:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.LYPos, X360Controls.LYNeg,
                            X360Controls.LXNeg, X360Controls.LXPos, X360Controls.LS,
                        });
                        break;
                    case 4:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.LXPos, X360Controls.LXNeg,
                            X360Controls.LYNeg, X360Controls.LYPos, X360Controls.LS,
                        });
                        break;
                    case 5:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.LXNeg, X360Controls.LXPos,
                            X360Controls.LYPos, X360Controls.LYNeg, X360Controls.LS,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 3)
            {
                switch (subTag)
                {
                    case 0:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.RYNeg, X360Controls.RYPos,
                            X360Controls.RXNeg, X360Controls.RXPos, X360Controls.RS,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.RYPos, X360Controls.RYNeg,
                            X360Controls.RXPos, X360Controls.RXNeg, X360Controls.RS,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.RYNeg, X360Controls.RYPos,
                            X360Controls.RXPos, X360Controls.RXNeg, X360Controls.RS,
                        });
                        break;
                    case 3:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.RYPos, X360Controls.RYNeg,
                            X360Controls.RXNeg, X360Controls.RXPos, X360Controls.RS,
                        });
                        break;
                    case 4:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.RXPos, X360Controls.RXNeg,
                            X360Controls.RYNeg, X360Controls.RYPos, X360Controls.RS,
                        });
                        break;
                    case 5:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.RXNeg, X360Controls.RXPos,
                            X360Controls.RYPos, X360Controls.RYNeg, X360Controls.RS,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 4)
            {
                switch(subTag)
                {
                    case 0:
                        // North, South, West, East
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.Y, X360Controls.A, X360Controls.X, X360Controls.B, X360Controls.Unbound,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.B, X360Controls.X, X360Controls.Y, X360Controls.A, X360Controls.Unbound,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.X, X360Controls.B, X360Controls.A, X360Controls.Y, X360Controls.Unbound,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 5)
            {
                switch(subTag)
                {
                    case 0:
                        // North, South, West, East
                        actionBtns.AddRange(new object[5]
                        {
                            KeyInterop.VirtualKeyFromKey(Key.W), KeyInterop.VirtualKeyFromKey(Key.S),
                            KeyInterop.VirtualKeyFromKey(Key.A), KeyInterop.VirtualKeyFromKey(Key.D),
                            X360Controls.Unbound,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            KeyInterop.VirtualKeyFromKey(Key.D), KeyInterop.VirtualKeyFromKey(Key.A),
                            KeyInterop.VirtualKeyFromKey(Key.W), KeyInterop.VirtualKeyFromKey(Key.S),
                            X360Controls.Unbound,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            KeyInterop.VirtualKeyFromKey(Key.A), KeyInterop.VirtualKeyFromKey(Key.D),
                            KeyInterop.VirtualKeyFromKey(Key.S), KeyInterop.VirtualKeyFromKey(Key.W),
                            X360Controls.Unbound,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 6)
            {
                switch(subTag)
                {
                    case 0:
                        // North, South, West, East
                        actionBtns.AddRange(new object[5]
                        {
                            KeyInterop.VirtualKeyFromKey(Key.Up), KeyInterop.VirtualKeyFromKey(Key.Down),
                            KeyInterop.VirtualKeyFromKey(Key.Left), KeyInterop.VirtualKeyFromKey(Key.Right),
                            X360Controls.Unbound,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            KeyInterop.VirtualKeyFromKey(Key.Right), KeyInterop.VirtualKeyFromKey(Key.Left),
                            KeyInterop.VirtualKeyFromKey(Key.Up), KeyInterop.VirtualKeyFromKey(Key.Down),
                            X360Controls.Unbound,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            KeyInterop.VirtualKeyFromKey(Key.Left), KeyInterop.VirtualKeyFromKey(Key.Right),
                            KeyInterop.VirtualKeyFromKey(Key.Down), KeyInterop.VirtualKeyFromKey(Key.Up),
                            X360Controls.Unbound,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 7)
            {
                switch (subTag)
                {
                    case 0:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.MouseUp, X360Controls.MouseDown,
                            X360Controls.MouseLeft, X360Controls.MouseRight,
                            X360Controls.Unbound,
                        });
                        break;
                    case 1:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.MouseDown, X360Controls.MouseUp,
                            X360Controls.MouseRight, X360Controls.MouseLeft,
                            X360Controls.Unbound,
                        });
                        break;
                    case 2:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.MouseUp, X360Controls.MouseDown,
                            X360Controls.MouseRight, X360Controls.MouseLeft,
                            X360Controls.Unbound,
                        });
                        break;
                    case 3:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.MouseDown, X360Controls.MouseUp,
                            X360Controls.MouseLeft, X360Controls.MouseRight,
                            X360Controls.Unbound,
                        });
                        break;
                    case 4:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.MouseRight, X360Controls.MouseLeft,
                            X360Controls.MouseUp, X360Controls.MouseDown,
                            X360Controls.Unbound,
                        });
                        break;
                    case 5:
                        actionBtns.AddRange(new object[5]
                        {
                            X360Controls.MouseLeft, X360Controls.MouseRight,
                            X360Controls.MouseDown, X360Controls.MouseUp,
                            X360Controls.Unbound,
                        });
                        break;
                    default:
                        break;
                }
            }
            else if (baseTag == 8)
            {
                actionBtns.AddRange(new object[5]
                {
                    X360Controls.Unbound, X360Controls.Unbound,
                    X360Controls.Unbound, X360Controls.Unbound,
                    X360Controls.Unbound,
                });
            }


            switch (highlightControl)
            {
                case ControlSelection.DPad:
                    inputControls.AddRange(new DS4Controls[4]
                    {
                        DS4Controls.DpadUp, DS4Controls.DpadDown,
                        DS4Controls.DpadLeft, DS4Controls.DpadRight,
                    });
                    break;
                case ControlSelection.LeftStick:
                    inputControls.AddRange(new DS4Controls[5]
                    {
                        DS4Controls.LYNeg, DS4Controls.LYPos,
                        DS4Controls.LXNeg, DS4Controls.LXPos, DS4Controls.L3,
                    });
                    break;
                case ControlSelection.RightStick:
                    inputControls.AddRange(new DS4Controls[5]
                    {
                        DS4Controls.RYNeg, DS4Controls.RYPos,
                        DS4Controls.RXNeg, DS4Controls.RXPos, DS4Controls.R3,
                    });
                    break;
                case ControlSelection.FaceButtons:
                    inputControls.AddRange(new DS4Controls[4]
                    {
                        DS4Controls.Triangle, DS4Controls.Cross,
                        DS4Controls.Square, DS4Controls.Circle,
                    });
                    break;
                case ControlSelection.None:
                default:
                    break;
            }

            int idx = 0;
            foreach(DS4Controls dsControl in inputControls)
            {
                DS4ControlSettings setting = Global.GetDS4CSetting(deviceNum, dsControl);
                setting.Reset();
                if (idx < actionBtns.Count && actionBtns[idx] != null)
                {
                    object outAct = actionBtns[idx];
                    X360Controls defaultControl = Global.defaultButtonMapping[(int)dsControl];
                    if (!(outAct is X360Controls) || defaultControl != (X360Controls)outAct)
                    {
                        setting.UpdateSettings(false, outAct, null, DS4KeyType.None);
                        Global.RefreshActionAlias(setting, false);
                    }
                }

                idx++;
            }

            return inputControls;
        }
    }

    public class TriggerModeChoice
    {
        private string displayName;
        public string DisplayName { get => displayName; set => displayName = value; }

        public TriggerMode mode;
        public TriggerMode Mode { get => mode; set => mode = value; }

        public TriggerModeChoice(string name, TriggerMode mode)
        {
            this.displayName = name;
            this.mode = mode;
        }

        public override string ToString()
        {
            return displayName;
        }
    }

    public class TwoStageChoice
    {
        private string displayName;
        public string DisplayName { get => displayName; set => displayName = value; }


        private TwoStageTriggerMode mode;
        public TwoStageTriggerMode Mode { get => mode; set => mode = value; }

        public TwoStageChoice(string name, TwoStageTriggerMode mode)
        {
            this.displayName = name;
            this.mode = mode;
        }
    }

    public class TriggerEffectChoice
    {
        private string displayName;
        public string DisplayName { get => displayName; set => displayName = value; }


        private DS4Windows.InputDevices.TriggerEffects mode;
        public DS4Windows.InputDevices.TriggerEffects Mode { get => mode; set => mode = value; }

        public TriggerEffectChoice(string name, DS4Windows.InputDevices.TriggerEffects mode)
        {
            this.displayName = name;
            this.mode = mode;
        }
    }
}
