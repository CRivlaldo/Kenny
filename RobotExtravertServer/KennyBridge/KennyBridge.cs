using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KennyBridge
{
    public class KennyBridge
    {
        static KennyBridge instance = null;

        MotorsManager motors = new MotorsManager();
        ComPortHelper comPortHelper = new ComPortHelper();

        //

        public static KennyBridge Instance
        {
            get { return KennyBridge.instance; }
        }

        public MotorsManager Motors
        {
            get { return motors; }
        }

        internal ComPortHelper ComPort
        {
            get { return comPortHelper; }
        }

        public static bool Init()
        {
            if (instance != null)
                return false;

            instance = new KennyBridge();

            return instance.InitInternal();
        }

        bool InitInternal()
        {
            comPortHelper.OpenConnection();

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
            comPortHelper.CloseConnection();
        }
    }
}
