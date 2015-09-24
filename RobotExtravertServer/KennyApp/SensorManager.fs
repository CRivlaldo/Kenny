module SensorManager

    open KennyInterProcessBridge
    open ComPortHelper
    
    type SensorManager() = 
        let getUltrasonicSensorValueMessageCode = byte 20
        let getMagnetometerVectorMessageCode = byte 21
        let getAccelerometerVectorMessageCode = byte 22
        let getGyroscopeVectorMessageCode = byte 23
        let getBatteryVoltageMessageCode = byte 30
        let getChargerVoltageMessageCode = byte 31

        let GetEmpty3dVector = Array.zeroCreate 3

        let mutable ultrasonicSensor0Value = uint16 0
        let mutable magnetometerVector : float32 array = GetEmpty3dVector
        let mutable accelerometerVector : float32 array = GetEmpty3dVector
        let mutable gyroscopeVector : float32 array = GetEmpty3dVector
        let mutable batteryVoltage = 0.0f
        let mutable chargerVoltage = 0.0f

        let ReadValue message readingMethod = 
            SendMessage(message)
            readingMethod()

        let ReadValueWithouArgs messageCode readingFunc = ReadValue [|messageCode|] readingFunc

        let ReadUltrasonicSensorValue(index : byte) = 
            ReadValue [|getUltrasonicSensorValueMessageCode; index|] ReadUnsignedShort
            
        let Read3dVectorValue messageCode = ReadValueWithouArgs messageCode Read3dVector
        let ReadMagnetometerVector() = Read3dVectorValue getMagnetometerVectorMessageCode            
        let ReadAccelerometerVector() = Read3dVectorValue getAccelerometerVectorMessageCode            
        let ReadGyroscopeVector() = Read3dVectorValue getGyroscopeVectorMessageCode

        let ReadBatteryVoltage() = ReadValueWithouArgs getBatteryVoltageMessageCode ReadSingle
        let ReadChargerVoltage() = ReadValueWithouArgs getChargerVoltageMessageCode ReadSingle

//        let ReadUltrasonicSensorValue(index : byte) =
//            SendMessage([|getUltrasonicSensorValueMessageCode; index|])
//            ReadUnsignedShort()
            
//        let ReadMagnetometerVector() =
//            ComPortHelper.SendMessage([|getMagnetometerVectorMessageCode|])
//            ComPortHelper.Read3dVector()
            
//        let ReadAccelerometerVector() =
//            ComPortHelper.SendMessage([|getAccelerometerVectorMessageCode|])
//            ComPortHelper.Read3dVector()
//            
//        let ReadGyroscopeVector() =
//            ComPortHelper.SendMessage([|getGyroscopeVectorMessageCode|])
//            ComPortHelper.Read3dVector()
//
//        let ReadBatteryVoltage() =
//            ComPortHelper.SendMessage([|getBatteryVoltageMessageCode|])
//            ComPortHelper.ReadSingle()
//
//        let ReadChargerVoltage() =
//            ComPortHelper.SendMessage([|getChargerVoltageMessageCode|])
//            ComPortHelper.ReadSingle()

        member this.Update() = 
            ultrasonicSensor0Value <- ReadUltrasonicSensorValue(byte 0)

            magnetometerVector <- ReadMagnetometerVector()
            accelerometerVector <- ReadAccelerometerVector()
            gyroscopeVector <- ReadGyroscopeVector()

            batteryVoltage <- ReadBatteryVoltage()
            chargerVoltage <- ReadChargerVoltage()
        
        member this.GetTelemetry() = 
            let mutable telemetry = new Telemetry()

            telemetry.ultrasonicSensor0Value <- int ultrasonicSensor0Value

            telemetry.magnetometerX <- magnetometerVector.[0]
            telemetry.magnetometerY <- magnetometerVector.[1]
            telemetry.magnetometerZ <- magnetometerVector.[2]

            telemetry.accelerometerX <- accelerometerVector.[0]
            telemetry.accelerometerY <- accelerometerVector.[1]
            telemetry.accelerometerZ <- accelerometerVector.[2]

            telemetry.gyroscopeX <- gyroscopeVector.[0]
            telemetry.gyroscopeY <- gyroscopeVector.[1]
            telemetry.gyroscopeZ <- gyroscopeVector.[2]

            telemetry.batteryVoltage <- batteryVoltage
            telemetry.chargerVoltage <- chargerVoltage

            telemetry

