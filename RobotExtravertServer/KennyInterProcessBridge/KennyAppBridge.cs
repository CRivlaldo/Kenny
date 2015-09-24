using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Drawing;

namespace KennyInterProcessBridge
{
    [ServiceContract]
    public interface IKennyAppBridge
    {
        [OperationContract]
        int Get42();

        [OperationContract]
        bool IsManualModeEnabled();

        [OperationContract]
        void EnableManualMode();

        [OperationContract]
        void DisableManualMode();

        [OperationContract]
        void MoveForward();

        [OperationContract]
        void MoveBackward();

        [OperationContract]
        void RotateLeft();

        [OperationContract]
        void RotateRight();

        [OperationContract]
        void Stop();

        [OperationContract]
        Bitmap GetWebCameraFrame();

        [OperationContract]
        Telemetry GetTelemetry();
    }
}
