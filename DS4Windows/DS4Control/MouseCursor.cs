using System;

namespace DS4Windows
{
    public class MouseCursor
    {
        private readonly int deviceNumber;
        private DS4Device.GyroMouseSens gyroMouseSensSettings;
        public MouseCursor(int deviceNum, DS4Device.GyroMouseSens gyroMouseSens)
        {
            deviceNumber = deviceNum;
            gyroMouseSensSettings = gyroMouseSens;
            filterPair.axis1Filter.MinCutoff = filterPair.axis2Filter.MinCutoff = GyroMouseInfo.DEFAULT_MINCUTOFF;
            filterPair.axis1Filter.Beta = filterPair.axis2Filter.Beta = GyroMouseInfo.DEFAULT_BETA;
            Global.GyroMouseInfo[deviceNum].SetRefreshEvents(filterPair.axis1Filter);
            Global.GyroMouseInfo[deviceNum].SetRefreshEvents(filterPair.axis2Filter);
        }

        public void ReplaceOneEuroFilterPair()
        {
            Global.GyroMouseInfo[deviceNumber].RemoveRefreshEvents();
            filterPair = new OneEuroFilterPair();
        }

        public void SetupLateOneEuroFilters()
        {
            filterPair.axis1Filter.MinCutoff = filterPair.axis2Filter.MinCutoff = Global.GyroMouseInfo[deviceNumber].MinCutoff;
            filterPair.axis1Filter.Beta = filterPair.axis2Filter.Beta = Global.GyroMouseInfo[deviceNumber].Beta;
            Global.GyroMouseInfo[deviceNumber].SetRefreshEvents(filterPair.axis1Filter);
            Global.GyroMouseInfo[deviceNumber].SetRefreshEvents(filterPair.axis2Filter);
        }

        // Keep track of remainders when performing moves or we lose fractional parts.
        private double horizontalRemainder = 0.0, verticalRemainder = 0.0;
        private double hRemainder = 0.0, vRemainder = 0.0;
        /** Indicate x/y direction for doing jitter compensation, etc. */
        public enum Direction { Negative, Neutral, Positive }
        // Track direction vector separately and very trivially for now.
        private Direction horizontalDirection = Direction.Neutral,
            verticalDirection = Direction.Neutral;
        private Direction hDirection = Direction.Neutral, vDirection = Direction.Neutral;

        public const int GYRO_MOUSE_DEADZONE = 10;
        private const double TOUCHPAD_MOUSE_OFFSET = 0.015;

        private const int SMOOTH_BUFFER_LEN = 3;
        private double[] xSmoothBuffer = new double[SMOOTH_BUFFER_LEN];
        private double[] ySmoothBuffer = new double[SMOOTH_BUFFER_LEN];
        private int smoothBufferTail = 0;
        private OneEuroFilterPair filterPair = new OneEuroFilterPair();

        private int gyroCursorDeadZone = GYRO_MOUSE_DEADZONE;
        public int GyroCursorDeadZone { get => gyroCursorDeadZone; set => gyroCursorDeadZone = value; }


        double coefficient = 0.0;
        double verticalScale = 0.0;
        bool gyroSmooth = false;

        int tempInt = 0;
        double tempDouble = 0.0;
        //bool tempBool = false;

        public virtual void sixaxisMoved(SixAxisEventArgs arg)
        {
            int deltaX = 0, deltaY = 0;
            deltaX = Global.getGyroMouseHorizontalAxis(deviceNumber) == 0 ? arg.sixAxis.gyroYawFull :
                arg.sixAxis.gyroRollFull;
            deltaY = -arg.sixAxis.gyroPitchFull;
            //tempDouble = arg.sixAxis.elapsed * 0.001 * 200.0; // Base default speed on 5 ms
            tempDouble = arg.sixAxis.elapsed * 200.0; // Base default speed on 5 ms

            GyroMouseInfo tempInfo = Global.GyroMouseInfo[deviceNumber];
            gyroSmooth = tempInfo.enableSmoothing;
            double gyroSmoothWeight = 0.0;

            coefficient = (Global.getGyroSensitivity(deviceNumber) * 0.01) * gyroMouseSensSettings.mouseCoefficient;
            double offset = gyroMouseSensSettings.mouseOffset;
            if (gyroSmooth)
            {
                offset = gyroMouseSensSettings.mouseSmoothOffset;
            }

            double tempAngle = Math.Atan2(-deltaY, deltaX);
            double normX = Math.Abs(Math.Cos(tempAngle));
            double normY = Math.Abs(Math.Sin(tempAngle));
            int signX = Math.Sign(deltaX);
            int signY = Math.Sign(deltaY);

            if (deltaX == 0 || (hRemainder > 0 != deltaX > 0))
            {
                hRemainder = 0.0;
            }

            if (deltaY == 0 || (vRemainder > 0 != deltaY > 0))
            {
                vRemainder = 0.0;
            }

            int deadzoneX = (int)Math.Abs(normX * gyroCursorDeadZone);
            int deadzoneY = (int)Math.Abs(normY * gyroCursorDeadZone);

            if (Math.Abs(deltaX) > deadzoneX)
            {
                deltaX -= signX * deadzoneX;
            }
            else
            {
                deltaX = 0;
            }

            if (Math.Abs(deltaY) > deadzoneY)
            {
                deltaY -= signY * deadzoneY;
            }
            else
            {
                deltaY = 0;
            }

            double xMotion = deltaX != 0 ? coefficient * (deltaX * tempDouble)
                + (normX * (offset * signX)) : 0;

            verticalScale = Global.getGyroSensVerticalScale(deviceNumber) * 0.01;
            double yMotion = deltaY != 0 ? (coefficient * verticalScale) * (deltaY * tempDouble)
                + (normY * (offset * signY)) : 0;

            int xAction = 0;
            if (xMotion != 0.0)
            {
                xMotion += hRemainder;
            }
            else
            {
                hRemainder = 0.0;
            }

            int yAction = 0;
            if (yMotion != 0.0)
            {
                yMotion += vRemainder;
            }
            else
            {
                vRemainder = 0.0;
            }

            if (gyroSmooth)
            {
                if (tempInfo.smoothingMethod == GyroMouseInfo.SmoothingMethod.OneEuro)
                {
                    double currentRate = 1.0 / arg.sixAxis.elapsed;
                    xMotion = filterPair.axis1Filter.Filter(xMotion, currentRate);
                    yMotion = filterPair.axis2Filter.Filter(yMotion, currentRate);
                }
                else
                {
                    int iIndex = smoothBufferTail % SMOOTH_BUFFER_LEN;
                    xSmoothBuffer[iIndex] = xMotion;
                    ySmoothBuffer[iIndex] = yMotion;
                    smoothBufferTail = iIndex + 1;

                    double currentWeight = 1.0;
                    double finalWeight = 0.0;
                    double x_out = 0.0, y_out = 0.0;
                    int idx = 0;
                    for (int i = 0; i < SMOOTH_BUFFER_LEN; i++)
                    {
                        idx = (smoothBufferTail - i - 1 + SMOOTH_BUFFER_LEN) % SMOOTH_BUFFER_LEN;
                        x_out += xSmoothBuffer[idx] * currentWeight;
                        y_out += ySmoothBuffer[idx] * currentWeight;
                        finalWeight += currentWeight;
                        currentWeight *= gyroSmoothWeight;
                    }

                    x_out /= finalWeight;
                    xMotion = x_out;
                    y_out /= finalWeight;
                    yMotion = y_out;
                }
            }

            hRemainder = vRemainder = 0.0;
            double distSqu = (xMotion * xMotion) + (yMotion * yMotion);

            xAction = (int)xMotion;
            yAction = (int)yMotion;

            if (tempInfo.minThreshold == 1.0)
            {
                hRemainder = xMotion - xAction;
                vRemainder = yMotion - yAction;
            }
            else
            {
                if (distSqu >= (tempInfo.minThreshold * tempInfo.minThreshold))
                {
                    hRemainder = xMotion - xAction;
                    vRemainder = yMotion - yAction;
                }
                else
                {
                    hRemainder = xMotion;
                    xAction = 0;

                    vRemainder = yMotion;
                    yAction = 0;
                }
            }

            int gyroInvert = Global.getGyroInvert(deviceNumber);
            if ((gyroInvert & 0x02) == 2)
                xAction *= -1;

            if ((gyroInvert & 0x01) == 1)
                yAction *= -1;

            if (yAction != 0 || xAction != 0)
                Global.outputKBMHandler.MoveRelativeMouse(xAction, yAction);

            hDirection = xMotion > 0.0 ? Direction.Positive : xMotion < 0.0 ? Direction.Negative : Direction.Neutral;
            vDirection = yMotion > 0.0 ? Direction.Positive : yMotion < 0.0 ? Direction.Negative : Direction.Neutral;
        }

        public void mouseRemainderReset(SixAxisEventArgs arg)
        {
            hRemainder = vRemainder = 0.0;
            int iIndex = smoothBufferTail % SMOOTH_BUFFER_LEN;
            xSmoothBuffer[iIndex] = 0.0;
            ySmoothBuffer[iIndex] = 0.0;
            smoothBufferTail = iIndex + 1;

            GyroMouseInfo tempInfo = Global.GyroMouseInfo[deviceNumber];
            if (tempInfo.smoothingMethod == GyroMouseInfo.SmoothingMethod.OneEuro)
            {
                double currentRate = 1.0 / arg.sixAxis.elapsed;
                filterPair.axis1Filter.Filter(0.0, currentRate);
                filterPair.axis2Filter.Filter(0.0, currentRate);
            }
        }

        public void touchesBegan(TouchpadEventArgs arg)
        {
            if (arg.touches.Length == 1)
            {
                horizontalRemainder = verticalRemainder = 0.0;
                horizontalDirection = verticalDirection = Direction.Neutral;

            }
        }

        private byte lastTouchID;
        public void touchesMoved(TouchpadEventArgs arg, bool dragging, bool disableInvert = false)
        {
            int touchesLen = arg.touches.Length;
            if ((!dragging && touchesLen != 1) || (dragging && touchesLen < 1))
                return;

            int deltaX = 0, deltaY = 0;
            if (arg.touches[0].touchID != lastTouchID)
            {
                deltaX = deltaY = 0;
                horizontalRemainder = verticalRemainder = 0.0;
                horizontalDirection = verticalDirection = Direction.Neutral;
                lastTouchID = arg.touches[0].touchID;
            }
            else
            {
                if (dragging && touchesLen > 1)
                {
                    deltaX = arg.touches[1].deltaX;
                    deltaY = arg.touches[1].deltaY;
                }
                else
                {
                    deltaX = arg.touches[0].deltaX;
                    deltaY = arg.touches[0].deltaY;
                }
            }

            TouchMoveCursor(deltaX, deltaY, disableInvert);
        }

        public void TouchesMovedAbsolute(TouchpadEventArgs arg)
        {
            int touchesLen = arg.touches.Length;
            if (touchesLen != 1)
                return;

            int currentX = 0, currentY = 0;
            /// Is this a dead branch?
            if (touchesLen > 1)
            {
                currentX = arg.touches[1].hwX;
                currentY = arg.touches[1].hwY;
            }
            else
            {
                currentX = arg.touches[0].hwX;
                currentY = arg.touches[0].hwY;
            }

            TouchpadAbsMouseSettings absSettings = Global.TouchAbsMouse[deviceNumber];

            /// maxZone* determines what percentage of the touchpad will be used.
            /// If maxZoneX is 90, any touch below 5% of the range is clamped to the minimum input and any touch above 95% of the range is clamped to the maximum input.
            /// Values inbetween 5% and 95% are linearly interpolated to fill the whole range.

            double absX = ((currentX - DS4Touchpad.RES_HALFED_X) / (DS4Touchpad.RESOLUTION_X_MAX * absSettings.maxZoneX * 0.01)) + 0.5;
            double absY = ((currentY - DS4Touchpad.RES_HALFED_Y) / (DS4Touchpad.RESOLUTION_Y_MAX * absSettings.maxZoneY * 0.01)) + 0.5;

            absX = Math.Clamp(absX, 0.0, 1.0);
            absY = Math.Clamp(absY, 0.0, 1.0);

            /// minPos* and maxPos* determine what range of the screen will be used as outputs.
            /// If minPosX is 10 and maxPosX is 90, only the middle 80% of the screen will be used as outputs.

            double minPosX = absSettings.minPosX * 0.01;
            double minPosY = absSettings.minPosY * 0.01;
            double maxPosX = Math.Max(absSettings.maxPosX * 0.01, minPosX);
            double maxPosY = Math.Max(absSettings.maxPosY * 0.01, minPosY);

            double relX = minPosX + absX * (maxPosX - minPosX);
            double relY = minPosY + absY * (maxPosY - minPosY);

            //InputMethods.MoveAbsoluteMouse(relX, relY);
            Global.outputKBMHandler.MoveAbsoluteMouse(relX, relY);
        }

        public void TouchCenterAbsolute()
        {
            TouchpadAbsMouseSettings absSettings = Global.TouchAbsMouse[deviceNumber];

            double minPosX = absSettings.minPosX * 0.01;
            double minPosY = absSettings.minPosY * 0.01;
            double maxPosX = Math.Max(absSettings.maxPosX * 0.01, minPosX);
            double maxPosY = Math.Max(absSettings.maxPosY * 0.01, minPosY);

            double midX = (maxPosX + minPosX) * 0.5;
            double midY = (maxPosY + minPosY) * 0.5;

            //InputMethods.MoveAbsoluteMouse(midX, midY);
            Global.outputKBMHandler.MoveAbsoluteMouse(midX, midY);
        }

        public void TouchMoveCursor(int dx, int dy, bool disableInvert = false)
        {
            TouchpadRelMouseSettings relMouseSettings = Global.TouchRelMouse[deviceNumber];
            if (relMouseSettings.rotation != 0.0)
            {
                //double rotation = 5.0 * Math.PI / 180.0;
                double rotation = relMouseSettings.rotation;
                double sinAngle = Math.Sin(rotation), cosAngle = Math.Cos(rotation);
                int tempX = dx, tempY = dy;
                dx = (int)Global.Clamp(-DS4Touchpad.RESOLUTION_X_MAX, tempX * cosAngle - tempY * sinAngle, DS4Touchpad.RESOLUTION_X_MAX);
                dy = (int)Global.Clamp(-DS4Touchpad.RESOLUTION_Y_MAX, tempX * sinAngle + tempY * cosAngle, DS4Touchpad.RESOLUTION_Y_MAX);
            }

            double tempAngle = Math.Atan2(-dy, dx);
            double normX = Math.Abs(Math.Cos(tempAngle));
            double normY = Math.Abs(Math.Sin(tempAngle));
            int signX = Math.Sign(dx);
            int signY = Math.Sign(dy);
            double coefficient = Global.getTouchSensitivity(deviceNumber) * 0.01;
            bool jitterCompenstation = Global.getTouchpadJitterCompensation(deviceNumber);

            double xMotion = dx != 0 ?
                coefficient * dx + (normX * (TOUCHPAD_MOUSE_OFFSET * signX)) : 0.0;

            double yMotion = dy != 0 ?
                coefficient * dy + (normY * (TOUCHPAD_MOUSE_OFFSET * signY)) : 0.0;

            if (jitterCompenstation)
            {
                double absX = Math.Abs(xMotion);
                if (absX <= normX * 0.15)
                {
                    xMotion = signX * Math.Pow(absX / 0.15f, 1.408) * 0.15;
                }

                double absY = Math.Abs(yMotion);
                if (absY <= normY * 0.15)
                {
                    yMotion = signY * Math.Pow(absY / 0.15f, 1.408) * 0.15;
                }
            }

            // Collect rounding errors instead of losing motion.
            if (xMotion > 0.0 && horizontalRemainder > 0.0)
            {
                xMotion += horizontalRemainder;
            }
            else if (xMotion < 0.0 && horizontalRemainder < 0.0)
            {
                xMotion += horizontalRemainder;
            }

            if (yMotion > 0.0 && verticalRemainder > 0.0)
            {
                yMotion += verticalRemainder;
            }
            else if (yMotion < 0.0 && verticalRemainder < 0.0)
            {
                yMotion += verticalRemainder;
            }

            double distSqu = (xMotion * xMotion) + (yMotion * yMotion);
            int xAction = (int)xMotion;
            int yAction = (int)yMotion;

            if (relMouseSettings.minThreshold == 1.0)
            {
                horizontalRemainder = xMotion - xAction;
                verticalRemainder = yMotion - yAction;
            }
            else
            {
                //Console.WriteLine("{0} {1}", horizontalRemainder, xAction, distSqu);

                if (distSqu >= (relMouseSettings.minThreshold * relMouseSettings.minThreshold))
                {
                    horizontalRemainder = xMotion - xAction;
                    verticalRemainder = yMotion - yAction;
                }
                else
                {
                    horizontalRemainder = xMotion;
                    xAction = 0;

                    verticalRemainder = yMotion;
                    yAction = 0;
                }
            }

            if (disableInvert == false)
            {
                int touchpadInvert = tempInt = Global.getTouchpadInvert(deviceNumber);
                if ((touchpadInvert & 0x02) == 2)
                    xAction *= -1;

                if ((touchpadInvert & 0x01) == 1)
                    yAction *= -1;
            }

            if (yAction != 0 || xAction != 0)
                Global.outputKBMHandler.MoveRelativeMouse(xAction, yAction);

            horizontalDirection = xMotion > 0.0 ? Direction.Positive : xMotion < 0.0 ? Direction.Negative : Direction.Neutral;
            verticalDirection = yMotion > 0.0 ? Direction.Positive : yMotion < 0.0 ? Direction.Negative : Direction.Neutral;
        }
    }
}
