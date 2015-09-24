module MotorManager
    let ChangeMotorState side state = ComPortHelper.SendMessage([|byte 100; byte side; byte state|])

    let MoveForward() =
        printfn "Forward"
        ChangeMotorState 0 1
        ChangeMotorState 1 1

    let MoveBackward() =
        printfn "Backward"
        ChangeMotorState  0 2
        ChangeMotorState 1 2

    let RotateLeft() =
        printfn "Left"
        ChangeMotorState 0 1
        ChangeMotorState 1 2

    let RotateRight() =
        printfn "Right"
        ChangeMotorState 0 2
        ChangeMotorState 1 1

    let Stop() =
        printfn "Stop"
        ChangeMotorState 0 0
        ChangeMotorState 1 0
//
//module MotorManager
//    let ChangeMotorState(side : byte, state : byte) =
//        ComPortHelper.SendMessage [100, side, state]
//
//    let MoveForward() =
//        printfn "Forward"
//        ChangeMotorState(byte 0, byte 1)
//        ChangeMotorState(byte 1, byte 1)
//
//    let MoveBackward() =
//        printfn "Backward"
//        ChangeMotorState(byte 0, byte 2)
//        ChangeMotorState(byte 1, byte 2)
//
//    let RotateLeft() =
//        printfn "Left"
//        ChangeMotorState(byte 0, byte 1)
//        ChangeMotorState(byte 1, byte 2)
//
//    let RotateRight() =
//        printfn "Right"
//        ChangeMotorState(byte 0, byte 2)
//        ChangeMotorState(byte 1, byte 1)
//
//    let Stop() =
//        printfn "Stop"
//        ChangeMotorState(byte 0, byte 0)
//        ChangeMotorState(byte 1, byte 0)