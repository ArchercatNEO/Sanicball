using System;
using System.Collections.Generic;
using UnityEngine;
using Lidgren.Network;
using SanicballCore;

namespace Sanicball.Data
{
    public record RemoteMovement : Packet
    {
        private const int updateRate = 40;

        private Timer Timer => timers[CtrlType];
        private static readonly Dictionary<ControlType, Timer> timers = new() {
            {ControlType.Keyboard, new()},
            {ControlType.Joystick1, new()},
            {ControlType.Joystick2, new()},
            {ControlType.Joystick3, new()},
            {ControlType.Joystick4, new()},
        };

        private static readonly Dictionary<ControlType, float> latestMovements = new() {
            {ControlType.Joystick1, int.MinValue},
            {ControlType.Joystick2, int.MinValue},
            {ControlType.Joystick3, int.MinValue},
            {ControlType.Joystick4, int.MinValue},
            {ControlType.Keyboard, int.MinValue},
        };
        private float LatestMovement
        {
            get => latestMovements[CtrlType];
            set => latestMovements[CtrlType] = value;
        }


        private readonly float Time;

        public readonly Guid ClientGuid;
        public readonly ControlType CtrlType;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Velocity;
        public readonly Vector3 AngularVelocity;
        public readonly Vector3 DirectionVector;

        public RemoteMovement(NetBuffer buffer)
        {
            ClientGuid = buffer.ReadGuid();
            CtrlType = (ControlType)buffer.ReadByte();
            Position = buffer.ReadVector3();
            Rotation = buffer.ReadQuaternion();
            Velocity = buffer.ReadVector3();
            AngularVelocity = buffer.ReadVector3();
            DirectionVector = buffer.ReadVector3();
            Timer.Start();
        }

        public RemoteMovement(Rigidbody ball, Vector3 direction, ControlType ctrl)
        {
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
            ClientGuid = Constants.guid;
            CtrlType = ctrl;
            Position = ball.transform.position;
            Rotation = ball.transform.rotation;
            Velocity = rigidbody.velocity;
            AngularVelocity = rigidbody.angularVelocity;
            DirectionVector = direction;
            Timer.Start();
        }

        public override bool RejectSend()
        {
            if (!ServerRelay.OnlineMode) return true;
            if (!Timer.Finished(1 / updateRate)) return true;

            Timer.Reset();
            Timer.Start();
            return false;
        }

        public override void WriteInto(NetBuffer buffer)
        {
            buffer.Write(ClientGuid);
            buffer.Write((byte)CtrlType);
            buffer.Write(Position);
            buffer.Write(Rotation);
            buffer.Write(Velocity);
            buffer.Write(AngularVelocity);
            buffer.Write(DirectionVector);
        }

        public override void Consume()
        {
            if (Time < LatestMovement) return;
            LatestMovement = Time;
            //Player.Find().DeusExMachina(this); //! Make this work with race manager's race players
        }
    }
}