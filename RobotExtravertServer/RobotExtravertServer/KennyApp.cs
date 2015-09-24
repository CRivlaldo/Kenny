using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using KennyInterProcessBridge;

namespace KennyServer
{
    public class KennyApp
    {
        const long maxMessageSize = 2097152;
        const int maxBufferSize = 2097152;

        static KennyApp instance = null;
        //IKennyAppBridge httpProxy = null;
        IKennyAppBridge pipeProxy = null;

        //NamedPipeClientStream pipeClient;

        ChannelFactory<IKennyAppBridge> pipeFactory;

        //

        public static KennyApp Instance
        {
            get { return instance; }
        }

        //public IKennyAppBridge HttpProxy
        //{
        //    get { return httpProxy; }
        //}

        public IKennyAppBridge PipeProxy
        {
            get { return pipeProxy; }
        }

        public static bool Init()
        {
            if (instance != null)
                return false;

            instance = new KennyApp();
            return instance.InitInternal();
        }

        bool InitInternal()
        {
            //ChannelFactory<IKennyAppBridge> httpFactory = new ChannelFactory<IKennyAppBridge>(new BasicHttpBinding(),
            //    new EndpointAddress("http://localhost:8000/KennyApp"));
            //httpProxy = httpFactory.CreateChannel();

            NetNamedPipeBinding binding = new NetNamedPipeBinding();
            binding.MaxReceivedMessageSize = maxMessageSize;
            binding.MaxBufferSize = maxBufferSize;
            binding.ReaderQuotas.MaxArrayLength = maxBufferSize;

            pipeFactory = new ChannelFactory<IKennyAppBridge>(binding,
                new EndpointAddress("net.pipe://localhost/PipeKennyApp"));
            pipeProxy = pipeFactory.CreateChannel();

                  //pipeClient = new NamedPipeClientStream("localhost", "PipeKennyStream", PipeDirection.InOut, PipeOptions.None);//, 
            //    //TokenImpersonationLevel.);
            //pipeClient.Connect();

            return true;
        }

        public static void Shutdown()
        {
            if (instance == null)
                return;
            
            instance.ShutdownInternal();
            instance = null;
        }

        void ShutdownInternal()
        {
            ((IClientChannel)pipeProxy).Close();
            pipeFactory.Close();

            //pipeClient.Close();
        }

        //Bitmap ReadImage(string fileName)
        //{
        //    //NamedPipeServerStream pipeServer = new NamedPipeServerStream("SamplePipe", PipeDirection.InOut);
        //    Bitmap bmpImage = null;

        //    using (BinaryReader binaryReader = new BinaryReader(pipeClient))
        //    {

        //        bmpImage = (Bitmap)Bitmap.FromStream(binaryReader.BaseStream);
        //    }

        //    return bmpImage;
        //}

        public Bitmap GetWebCameraFrame()
        {
            return (Bitmap) Image.FromFile("C:\\WebCameraImage.bmp");
            //return ReadImage("WebCameraImage.bmp");
        }

        //public int Get42()
        //{
        //    return httpProxy.Get42();

        //    //????
        //    //pipeProxy.Get42();
        //}
    }
}