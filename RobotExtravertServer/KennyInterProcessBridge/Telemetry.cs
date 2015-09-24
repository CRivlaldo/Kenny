using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KennyInterProcessBridge
{
    /// <summary>
    /// Information from Kenny's sensors in a some moment.
    /// </summary>
    public struct Telemetry
    {
        /// <summary>
        /// Distance from ultrasonic sensor #0 in millimeters.
        /// </summary>
        public int ultrasonicSensor0Value;

        /// <summary>
        /// X-coordinate of the magnetometer vector.
        /// </summary>
        public float magnetometerX;

        /// <summary>
        /// Y-coordinate of the magnetometer vector.
        /// </summary>
        public float magnetometerY;

        /// <summary>
        /// Z-coordinate of the magnetometer vector.
        /// </summary>
        public float magnetometerZ;

        /// <summary>
        /// X-coordinate of the accelerometer vector.
        /// </summary>
        public float accelerometerX;

        /// <summary>
        /// Y-coordinate of the accelerometer vector.
        /// </summary>
        public float accelerometerY;

        /// <summary>
        /// Z-coordinate of the accelerometer vector.
        /// </summary>
        public float accelerometerZ;

        /// <summary>
        /// X-coordinate of the gyroscope vector.
        /// </summary>
        public float gyroscopeX;

        /// <summary>
        /// Y-coordinate of the gyroscope vector.
        /// </summary>
        public float gyroscopeY;

        /// <summary>
        /// Z-coordinate of the gyroscope vector.
        /// </summary>
        public float gyroscopeZ;
        
        /// <summary>
        /// Voltage on the battery in volts.
        /// </summary>
        public float batteryVoltage;

        /// <summary>
        /// Voltage on the charger in volts.
        /// </summary>
        public float chargerVoltage;
    }
}
