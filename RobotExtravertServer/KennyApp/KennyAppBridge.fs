module KennyAppBridge

    open System
    open System.ServiceModel
    open System.Drawing
    open System.IO
    open System.IO.Pipes
    open KennyInterProcessBridge
    open Kenny

//    [<ServiceContract>]
//    type IKennyAppBridge =
//
//        [<OperationContract>]
//        abstract Get42 : unit -> int
//
//        [<OperationContract>]
//        abstract IsManualModeEnabled : unit -> bool
//
//        [<OperationContract>]
//        abstract EnableManualMode : unit -> unit
//
//        [<OperationContract>]
//        abstract DisableManualMode : unit -> unit
//
//        [<OperationContract>]
//        abstract MoveForward : unit -> unit
//
//        [<OperationContract>]
//        abstract MoveBackward : unit -> unit
//
//        [<OperationContract>]
//        abstract RotateLeft : unit -> unit
//
//        [<OperationContract>]
//        abstract RotateRight : unit -> unit
//
//        [<OperationContract>]
//        abstract Stop : unit -> unit
//
//        [<OperationContract>]
//        abstract GetWebCameraFrame : unit -> Bitmap

    [<ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)>]
    type KennyAppBridge(kennyIn) =
//        let mutable pipeServer : NamedPipeServerStream = new NamedPipeServerStream("PipeKennyStream", PipeDirection.InOut, 4) //!!!vladimir: magic constants
        let mutable kenny : Kenny = kennyIn
        let mutable serviceHost = null
        
        member private this.CreateServiceHost() = 
    //        let httpUri = new Uri("http://localhost:8000");
            let pipeUri = new Uri("net.pipe://localhost");

            let serviceHost = new ServiceHost(this, [| pipeUri |])
    //        let serviceHost = new ServiceHost (bridge, [| httpUri; pipeUri |])

            let mutable pipeBinding = new NetNamedPipeBinding()
            pipeBinding.MaxReceivedMessageSize <- int64 2097152
            pipeBinding.MaxBufferSize <- 2097152
            pipeBinding.ReaderQuotas.MaxArrayLength <- 2097152

    //        serviceHost.AddServiceEndpoint(typedefof<IKennyAppBridge>, new BasicHttpBinding(), "KennyApp") |> ignore
            serviceHost.AddServiceEndpoint(typedefof<IKennyAppBridge>, pipeBinding, "PipeKennyApp") |> ignore

            serviceHost
            
        member this.Open() = 
            serviceHost <- this.CreateServiceHost()
            serviceHost.Open()
//            pipeServer.WaitForConnection()

        member this.Close() =
            serviceHost.Close()
            serviceHost <- null
//            pipeServer.Close()
//            pipeServer <- null

//        member this.WriteImage(name : String, image : Image) =
//            image.Save("C:\\WebCameraImage.bmp");
////            let binaryWriter = new BinaryWriter(pipeServer)
////            image.Save(binaryWriter.BaseStream, Imaging.ImageFormat.Bmp)

                
//    {
//        sr.ReadBlock(buffer, 0, buffer.Length);
//        unsafe
//        {
//            fixed (char* ptr = buffer)
//            {
//                using (Bitmap image = new Bitmap(640, 480, 640*3, PixelFormat.Format24bppRgb, new IntPtr(ptr)))
//                {
//                    pictureBox1.Image = image;
//                    image.Save("test.png");
//                }
//            }
//        }
//    }

//        member this.WriteWebCameraFrame(frame : Image) = this.WriteImage("WebCameraImage.bmp", frame)

        interface IKennyAppBridge with
            member o.Get42() = 42
            member o.IsManualModeEnabled() = kenny.IsManualModeEnabled
            member o.EnableManualMode() = kenny.EnableManualMode()
            member o.DisableManualMode() = kenny.DisableManualMode()
            member o.MoveForward() = kenny.MoveForward()
            member o.MoveBackward() = kenny.MoveBackward()
            member o.RotateLeft() = kenny.RotateLeft()
            member o.RotateRight() = kenny.RotateRight()
            member o.Stop() = kenny.Stop()
            member this.GetWebCameraFrame() = WebCam.GetFrame()
            member this.GetTelemetry() = kenny.GetSensorManager().GetTelemetry()
        
        //member this.Kenny = kenny


//      using (ServiceHost host = new ServiceHost(
//        typeof(KennyAppBridge),
//        new Uri[]{
//          new Uri("http://localhost:8000"),
//          new Uri("net.pipe://localhost")
//        }))
//      {
//        host.AddServiceEndpoint(typeof(IKennyAppBridge),
//          new BasicHttpBinding(),
//          "KennyApp");
//
//        host.AddServiceEndpoint(typeof(IKennyAppBridge),
//          new NetNamedPipeBinding(),
//          "PipeKennyApp");
//
//        host.Open();
//
//        Console.WriteLine("Service is available. " +  
//          "Press <ENTER> to exit.");
//        Console.ReadLine();
//
//        host.Close();
//      }

