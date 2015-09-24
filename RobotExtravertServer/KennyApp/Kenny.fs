module Kenny

    open SensorManager

    type Kenny() =
        let mutable manualMode = false
        let sensorManager = new SensorManager()

        member this.IsManualModeEnabled = manualMode
        
        member this.EnableManualMode() = 
            manualMode <- true
            MotorManager.Stop()

        member this.DisableManualMode() = 
            manualMode <- false
            MotorManager.Stop()

        member this.MoveForward() = 
            if manualMode then MotorManager.MoveForward()

        member this.MoveBackward() = 
            if manualMode then MotorManager.MoveBackward()
        member this.RotateLeft() = 
            if manualMode then MotorManager.RotateLeft()
        member this.RotateRight() = 
            if manualMode then MotorManager.RotateRight()
        member this.Stop() = 
            if manualMode then MotorManager.Stop()

        member this.GetSensorManager() = sensorManager

        member this.Update() =
            sensorManager.Update()