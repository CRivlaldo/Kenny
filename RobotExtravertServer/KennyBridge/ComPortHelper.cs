using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace KennyBridge
{
    class ComPortHelper
    {
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);

        private SerialPinChangedEventHandler SerialPinChanged;

        SerialPort comPort = new SerialPort();

        //

        public ComPortHelper()
        {
            SerialPinChanged = new SerialPinChangedEventHandler(PinChanged);
            comPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);
        }

        internal void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            SerialPinChange serialPinChange = 0;
            bool signalState = false;

            serialPinChange = e.EventType;
            //lblCTSStatus.BackColor = Color.Green;
            //lblDSRStatus.BackColor = Color.Green;
            //lblRIStatus.BackColor = Color.Green;
            //lblBreakStatus.BackColor = Color.Green;
            switch (serialPinChange)
            {
                case SerialPinChange.Break:
                    //lblBreakStatus.BackColor = Color.Red;
                    ////MessageBox.Show("Break is Set");
                    break;
                case SerialPinChange.CDChanged:
                    signalState = comPort.CtsHolding;
                    //  MessageBox.Show("CD = " + signalState.ToString());
                    break;
                case SerialPinChange.CtsChanged:
                    signalState = comPort.CDHolding;
                    //lblCTSStatus.BackColor = Color.Red;
                    ////MessageBox.Show("CTS = " + signalState.ToString());
                    break;
                case SerialPinChange.DsrChanged:
                    signalState = comPort.DsrHolding;
                    //lblDSRStatus.BackColor = Color.Red;
                    //// MessageBox.Show("DSR = " + signalState.ToString());
                    break;
                case SerialPinChange.Ring:
                    //lblRIStatus.BackColor = Color.Red;
                    ////MessageBox.Show("Ring Detected");
                    break;
            }
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //InputData = ComPort.ReadExisting();
            //if (InputData != String.Empty)
            //{
            //    //byte[] bytes = new byte[InputData.Length * sizeof(char)];
            //    //System.Buffer.BlockCopy(InputData.ToCharArray(), 0, bytes, 0, bytes.Length);
            //    SetHEX(Encoding.ASCII.GetBytes(InputData));
            //    //this.BeginInvoke(new SetTextCallback(SetText), new object[] { InputData });
            //}

            byte[] buffer = new byte[4096];
            int byteRead = comPort.Read(buffer, 0, 4096);
            if (byteRead != 0)
            {
                //TODO:
                //By delegate push messages from the robot or do some internal stuff
            }
        }

        internal void SendMessage(byte[] buffer)
        {
            //Crc16 crc = new Crc16();
            ushort checkSum = Crc16.ModRTU_CRC(buffer, buffer.Length);

            byte[] message = new byte[buffer.Length + 2];

            buffer.CopyTo(message, 0);

            message[buffer.Length] = (byte)(checkSum % 256);
            message[buffer.Length + 1] = (byte)(checkSum / 256);

            comPort.Write(message, 0, message.Length);
        }

        public void OpenConnection()
        {
            //TODO:
            //Check data, return errors

            string[] comPortsNames = SerialPort.GetPortNames();

            comPort.PortName = comPortsNames[0];//!!!input parameter
            comPort.BaudRate = 9600;//!!!input parameter
            comPort.DataBits = 8;//!!!input parameter
            comPort.StopBits = StopBits.Two;//!!!input parameter
            comPort.Handshake = Handshake.None;//!!!input parameter
            comPort.Parity = Parity.None;//!!!input parameter
            comPort.Open();
        }

        public void CloseConnection()
        {
            comPort.Close();
        }


//        delegate void SetTextCallback(string text);
//        delegate void SetBatteryVoltageCallback(float x);
//        string InputData = String.Empty;

//        public Form1()
//        {
//            InitializeComponent();
//            SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
//            ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_1);
//        }
     
//        private void btnGetSerialPorts_Click(object sender, EventArgs e)
//        {
//            string[] ArrayComPortsNames = null;
//            int index = -1;
//            string ComPortName = null;
           
////Com Ports
//            ArrayComPortsNames = SerialPort.GetPortNames();
//            do
//            {
//                index += 1;
//                cboPorts.Items.Add(ArrayComPortsNames[index]);
               
              
//            } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
//            Array.Sort(ArrayComPortsNames);
           
//            if (index == ArrayComPortsNames.GetUpperBound(0))
//            {
//                ComPortName = ArrayComPortsNames[0];
//            }
//            //get first item print in text
//            cboPorts.Text = ArrayComPortsNames[0];
////Baud Rate
//            cboBaudRate.Items.Add(300);
//            cboBaudRate.Items.Add(600);
//            cboBaudRate.Items.Add(1200);
//            cboBaudRate.Items.Add(2400);
//            cboBaudRate.Items.Add(4800);
//            cboBaudRate.Items.Add(9600);
//            cboBaudRate.Items.Add(14400);
//            cboBaudRate.Items.Add(19200);
//            cboBaudRate.Items.Add(38400);
//            cboBaudRate.Items.Add(57600);
//            cboBaudRate.Items.Add(115200);
//            cboBaudRate.Items.ToString();
//            //get first item print in text
//            cboBaudRate.Text = cboBaudRate.Items[5].ToString(); 
////Data Bits
//            cboDataBits.Items.Add(7);
//            cboDataBits.Items.Add(8);
//            //get the first item print it in the text 
//            cboDataBits.Text = cboDataBits.Items[1].ToString();
           
////Stop Bits
//            cboStopBits.Items.Add("One");
//            cboStopBits.Items.Add("OnePointFive");
//            cboStopBits.Items.Add("Two");
//            //get the first item print in the text
//            cboStopBits.Text = cboStopBits.Items[2].ToString();
////Parity 
//            cboParity.Items.Add("None");
//            cboParity.Items.Add("Even");
//            cboParity.Items.Add("Mark");
//            cboParity.Items.Add("Odd");
//            cboParity.Items.Add("Space");
//            //get the first item print in the text
//            cboParity.Text = cboParity.Items[0].ToString();
////Handshake
//            cboHandShaking.Items.Add("None");
//            cboHandShaking.Items.Add("XOnXOff");
//            cboHandShaking.Items.Add("RequestToSend");
//            cboHandShaking.Items.Add("RequestToSendXOnXOff");
//            //get the first item print it in the text 
//            cboHandShaking.Text = cboHandShaking.Items[0].ToString();

//        }

     




//        private void SetText(string text)
//        {
//            this.rtbIncoming.Text += text;
//        }

//        void SetBatteryVoltage(float value)
//        {
//            textBoxBatteryVoltage.Text = value.ToString("0.00");
//        }

//        void SetHEX(byte[] bytes, int length)
//        {
//            if (length > 0)
//            {
//                if (false &&
//                    length == 6)
//                {
//                    uint integerPart = (uint)(bytes[0] * 256 + bytes[1]);
//                    uint fractionalPart = (uint)(bytes[2] * 256 + bytes[3]);

//                    double result = ((double)integerPart) + ((double)fractionalPart) / 65536.0;

//                    this.BeginInvoke(new SetTextCallback(SetText), new object[] { result.ToString("0.00") + "\r\n" });
//                    return;
//                }
//                if (length == 8)
//                {
//                    float x = BitConverter.ToSingle(bytes, 0);
//                    float y = BitConverter.ToSingle(bytes, 4);

//                    this.BeginInvoke(new SetTextCallback(SetText), new object[] { 
//                        string.Format("{0:0.00} {1:0.00} ", x, y) });
//                    return;
//                }
//                if (length == 6)
//                {
//                    float z = BitConverter.ToSingle(bytes, 0);

//                    this.BeginInvoke(new SetBatteryVoltageCallback(SetBatteryVoltage), new object[] { z });
//                    //this.BeginInvoke(new SetTextCallback(SetText), new object[] { 
//                    //    string.Format("{0:0.00}\r\n", z) });
//                    return;
//                }
                
//                string hexString = "";

//                for (int i = 0; i < length - 1; i++)
//                    hexString += bytes[i].ToString("X2") + " ";

//                hexString += bytes[length - 1].ToString("X2") + "\r\n";

//                this.BeginInvoke(new SetTextCallback(SetText), new object[] { hexString });
//            }
//        }

//        private void btnTest_Click(object sender, EventArgs e)
//        {
//            //SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
//            //ComPort.PinChanged += SerialPinChangedEventHandler1;
//            //ComPort.Open();

//            //ComPort.RtsEnable = true;
//            //ComPort.DtrEnable = true;
//            //btnTest.Enabled = false;

//        }


//        private void rtbOutgoing_KeyPress(object sender, KeyPressEventArgs e)
//        {
//            if (e.KeyChar == (char)13) // enter key  
//            {
//                ComPort.Write("\r\n");
//                rtbOutgoing.Text = "";
//            }
//            else if (e.KeyChar < 32 || e.KeyChar > 126)
//            {
//                e.Handled = true; // ignores anything else outside printable ASCII range  
//            }
//            else
//            {
//                ComPort.Write(e.KeyChar.ToString());
//            }
//        }
//        private void btnHello_Click(object sender, EventArgs e)
//        {
//            ComPort.Write("Hello World!");
//        }

//        private void btnHyperTerm_Click(object sender, EventArgs e)
//        {
//            string Command1 = txtCommand.Text;
//            string CommandSent;
//            int Length, j = 0;

//            Length = Command1.Length;

//            for (int i = 0; i < Length; i++)
//            {
//                CommandSent = Command1.Substring(j, 1);
//                ComPort.Write(CommandSent);
//                j++;
//            }

//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            ComPort.Write(new byte[1]{1}, 0, 1);
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            ComPort.Write(new byte[1] { 2 }, 0, 1);
//        }

//        void SendMessage(byte[] buffer)
//        {
//            //Crc16 crc = new Crc16();
//            ushort checkSum = Crc16.ModRTU_CRC(buffer, buffer.Length);

//            byte[] message = new byte[buffer.Length + 2];

//            buffer.CopyTo(message, 0);

//            message[buffer.Length] = (byte)(checkSum % 256);
//            message[buffer.Length + 1] = (byte)(checkSum / 256);

//            ComPort.Write(message, 0, message.Length);
//        }

//        private void buttonTestChannel_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 0 };
//            SendMessage(buffer);
//        }

//        void ChangeMotorState(byte side, byte state)
//        {
//            byte[] buffer = new byte[3] { 100, side, state };
//            SendMessage(buffer);
//        }

//        private void buttonLeftStop_Click(object sender, EventArgs e)
//        {
//            ChangeMotorState(0, 0);
//        }

//        private void buttonLeftForward_Click(object sender, EventArgs e)
//        {
//            ChangeMotorState(0, 1);
//        }

//        private void buttonLeftBackward_Click(object sender, EventArgs e)
//        {
//            ChangeMotorState(0, 2);
//        }

//        private void buttonRightStop_Click(object sender, EventArgs e)
//        {
//            ChangeMotorState(1, 0);
//        }

//        private void buttonRightForward_Click(object sender, EventArgs e)
//        {
//            ChangeMotorState(1, 1);
//        }

//        private void buttonRightBackward_Click(object sender, EventArgs e)
//        {
//            ChangeMotorState(1, 2);
//        }

//        private void buttonGetValueFromUltrasonicSensor_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[2] { 20, 0 };
//            SendMessage(buffer);
//        }

//        private void buttonMagnetometer_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 21 };
//            SendMessage(buffer);
//        }

//        private void buttonGetValuesFromAccelerometer_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 22 };
//            SendMessage(buffer);
//        }

//        private void buttonGetValuesFromGyroscope_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 23 };
//            SendMessage(buffer);
//        }

//        private void buttonGetBatteryVoltage_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 30 };
//            SendMessage(buffer);

//        }

//        private void buttonGetChargerVoltage_Click(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 31 };
//            SendMessage(buffer);
//        }

//        private void buttonStartMeasurer_Click(object sender, EventArgs e)
//        {
//            timerBatteryVoltageMeasurer.Enabled = true;
//        }

//        private void timerBatteryVoltageMeasurer_Tick(object sender, EventArgs e)
//        {
//            byte[] buffer = new byte[1] { 30 };
//            SendMessage(buffer);
//        }
    }
}
