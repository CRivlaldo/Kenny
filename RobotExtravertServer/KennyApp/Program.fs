open Kenny
open KennyAppBridge

let Init() =
    ComPortHelper.OpenConnection()
    WebCam.Init()

let Shutdown() =
    ComPortHelper.CloseConnection()
    WebCam.Shutdown()

let rec Update(kenny : Kenny, bridge : KennyAppBridge.KennyAppBridge) =
    let image = WebCam.Capture()
    kenny.Update()
    //bridge.WriteWebCameraFrame(image.ToBitmap())
    Update(kenny, bridge)

[<EntryPoint>]
let Main argv = 
//ToDo:
//проверить на уникальность процесс (только 1 экземляр приложения должен существовать)


//    printfn "%A" argv

    Init()

    let kenny = new Kenny()
    let bridge = new KennyAppBridge(kenny)

    bridge.Open()

    Update(kenny, bridge)

    bridge.Close()

    Shutdown()

    0 // return an integer exit code
