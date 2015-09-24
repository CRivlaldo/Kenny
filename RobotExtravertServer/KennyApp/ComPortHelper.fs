module ComPortHelper

    open System
    open System.IO.Ports
    open CRC16

    let internal comPort = new SerialPort()

    let mutable answerBuffer : List<byte> = List<byte>.Empty

    let timeout = float 1000.0f//!!!vladimir: magic constant

//    internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
//        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);

  
    //let SerialPinChanged : SerialPinChangedEventHandler //!!!vladimir: ????

    let ArrayToList(buffer : byte array, length : int) = [for i in [0..length-1] -> buffer.[i]]

    let DataReceived(e) =
        let buffer : byte[] = Array.zeroCreate 4096 //!!!!vladimir: magic const
        let bytesRead = comPort.Read(buffer, 0, buffer.Length)//!!!!vladimir: magic const
        if bytesRead <> 0 then answerBuffer <- answerBuffer @ ArrayToList(buffer, bytesRead)
//            for i in [0..bytesRead-1] do
//                answerBuffer <- answerBuffer @ [buffer.[i]]
            //ToDo:
            //by delegate push messages from the robot or do some internal stuff
            //inputdata = comport.readexisting();
            //if (inputdata != string.empty)
            //{
            //    //byte[] bytes = new byte[inputdata.length * sizeof(char)];
            //    //system.buffer.blockcopy(inputdata.tochararray(), 0, bytes, 0, bytes.length);
            //    sethex(encoding.ascii.getbytes(inputdata));
            //    //this.begininvoke(new settextcallback(settext), new object[] { inputdata });
            //}

    //SerialPinChanged = new SerialPinChangedEventHandler(PinChanged);//!!!!vladimir: !!!
    comPort.DataReceived.Add(DataReceived)

//        internal void PinChanged(object sender, SerialPinChangedEventArgs e)
//        {
//            SerialPinChange serialPinChange = 0;
//            bool signalState = false;
//
//            serialPinChange = e.EventType;
//            //lblCTSStatus.BackColor = Color.Green;
//            //lblDSRStatus.BackColor = Color.Green;
//            //lblRIStatus.BackColor = Color.Green;
//            //lblBreakStatus.BackColor = Color.Green;
//            switch (serialPinChange)
//            {
//                case SerialPinChange.Break:
//                    //lblBreakStatus.BackColor = Color.Red;
//                    ////MessageBox.Show("Break is Set");
//                    break;
//                case SerialPinChange.CDChanged:
//                    signalState = comPort.CtsHolding;
//                    //  MessageBox.Show("CD = " + signalState.ToString());
//                    break;
//                case SerialPinChange.CtsChanged:
//                    signalState = comPort.CDHolding;
//                    //lblCTSStatus.BackColor = Color.Red;
//                    ////MessageBox.Show("CTS = " + signalState.ToString());
//                    break;
//                case SerialPinChange.DsrChanged:
//                    signalState = comPort.DsrHolding;
//                    //lblDSRStatus.BackColor = Color.Red;
//                    //// MessageBox.Show("DSR = " + signalState.ToString());
//                    break;
//                case SerialPinChange.Ring:
//                    //lblRIStatus.BackColor = Color.Red;
//                    ////MessageBox.Show("Ring Detected");
//                    break;
//            }
//        }

    let OpenConnection() =
        //TODO:
        //Check data, return errors
        
        let comPortsNames = SerialPort.GetPortNames()
        comPort.PortName <- comPortsNames.[0]//!!!vladimir: need input parameter
        comPort.BaudRate <- 9600//!!!vladimir: need input parameter
        comPort.DataBits <- 8//!!!vladimir: need input parameter
        comPort.StopBits <- StopBits.Two//!!!vladimir: need input parameter
        comPort.Handshake <- Handshake.None//!!!vladimir: need input parameter
        comPort.Parity <- Parity.None//!!!ivladimir: need nput parameter
        comPort.Open()
         
    let CloseConnection() =
        comPort.Close()

    let SendMessage(buffer : byte[]) =
        printfn "Send %s" (buffer |> Array.fold (fun r s -> r.ToString() + s.ToString() + " ") "")
        let checkSum = CRC16.GetCRC16(buffer, buffer.Length)
        let message : byte[] = Array.zeroCreate (buffer.Length + 2)
        buffer.CopyTo(message, 0)
        message.[buffer.Length + 0] <- byte (checkSum % (uint16 256))
        message.[buffer.Length + 1] <- byte (checkSum / (uint16 256))
        comPort.Write(message, 0, message.Length)
//
//            byte[] message = new byte[buffer.Length + 2];
//
//            buffer.CopyTo(message, 0);
//
//            message[buffer.Length] = (byte)(checkSum % 256);
//            message[buffer.Length + 1] = (byte)(checkSum / 256);
//
//            comPort.Write(message, 0, message.Length);

    let PopFromAnswer() =
        let firstByte = answerBuffer.Head
        answerBuffer <- answerBuffer.Tail
        firstByte

    let ReadControlSum() =
        let crcL = PopFromAnswer()        
        let crcH = PopFromAnswer()
        (uint16 crcH) * (uint16 256) + (uint16 crcL)

    let WaitForAnswer(bytesToRead : int) : bool = 
        let rec WaitForAnswerLoop(startTime : DateTime, totalBytes : int) : bool =
            if answerBuffer.Length >= totalBytes then true
            else
                if (DateTime.Now - startTime).TotalMilliseconds > timeout then false
                else 
                    System.Threading.Thread.Sleep(1)
                    WaitForAnswerLoop(startTime, totalBytes)

        let totalBytes = bytesToRead + CRCLength 
        WaitForAnswerLoop(DateTime.Now, totalBytes)
        
    let ResetAnswerBuffer() =
        answerBuffer <- []

    let ReadBytes(bytesToRead : int) : byte array =
        if WaitForAnswer(bytesToRead) then       
            let mutable buffer = [|for i in [0..bytesToRead-1] -> PopFromAnswer()|]
        
    //        let mutable buffer : byte array = Array.zeroCreate bytesToRead
    //        for i in [0..bytesToRead-1] do 
    //            buffer.[i] <-     

            if ReadControlSum() = GetCRC16(buffer, bytesToRead) then buffer
            else Array.empty //!!!vladimir: need exception here
        else  
            ResetAnswerBuffer()
            Array.empty //!!!vladimir: need exception here

//    let GetBytesFromArray(buffer : byte array, startIndex : int, bytesCount : int) : byte array =
//        let mutable resultBuffer : byte array = Array.zeroCreate bytesCount
//        for i in [0..bytesCount-1] do
//            resultBuffer.[i] <- buffer.[startIndex + i]
//        resultBuffer

    let Read3dVector() : float32 array =
        let bytes = ReadBytes(12)
        if bytes.Length = 12 then
//            System.BitConverter.ToSingle(GetBytesFromArray(bytes, 0, 4), 
            let x = System.BitConverter.ToSingle(bytes, 0)
            let y = System.BitConverter.ToSingle(bytes, 4)
            let z = System.BitConverter.ToSingle(bytes, 8)
            [|x; y; z|]
        else Array.empty //!!!vladimir: need exception here

    let ReadSingle() =
        let bytes = ReadBytes(4)
        if bytes.Length = 4 then
            System.BitConverter.ToSingle(bytes, 0)
        else nanf //!!!vladimir: need exception here

    let ReadUnsignedShort() =
        let bytes = ReadBytes(2)
        if bytes.Length = 2 then
            (uint16 bytes.[0]) * (uint16 256) + (uint16 bytes.[1])
        else uint16 0 //!!!vladimir: need exception here
