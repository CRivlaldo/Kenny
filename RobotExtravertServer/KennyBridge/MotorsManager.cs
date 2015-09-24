using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KennyBridge
{
    public class MotorsManager
    {
        public void MoveForward()
        {
            Console.WriteLine("Forward");

            ChangeMotorState(0, 1);
            ChangeMotorState(1, 1);
        }

        public void MoveBackward()
        {
            Console.WriteLine("Backward");

            ChangeMotorState(0, 2);
            ChangeMotorState(1, 2);
        }

        public void RotateLeft()
        {
            Console.WriteLine("Left");

            ChangeMotorState(0, 1);
            ChangeMotorState(1, 2);
        }

        public void RotateRight()
        {
            Console.WriteLine("Right");

            ChangeMotorState(0, 2);
            ChangeMotorState(1, 1);
        }

        public void Stop()
        {
            Console.WriteLine("Stop");

            ChangeMotorState(0, 0);
            ChangeMotorState(1, 0);
        }

        void ChangeMotorState(byte side, byte state)
        {
            byte[] buffer = new byte[3] { 100, side, state };
            KennyBridge.Instance.ComPort.SendMessage(buffer);
        }
    }
}
