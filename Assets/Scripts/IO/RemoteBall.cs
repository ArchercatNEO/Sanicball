using UnityEngine;
using Sanicball.Logic;
using Sanicball.UI;
using Sanicball.Data;
using Unity.VisualScripting;
using Sanicball.Gameplay;

namespace Sanicball.Ball
{
    //TODO get lobby camera to look at this
    public class RemoteBall : AbstractBall
    {
        public Vector3 DirectionVector;

        //TODO make this destroy the player's ball when called
        public static RemoteBall LobbySpawn(Rigidbody ball)
        {


/*             CtrlType = ctrlType;
            CharacterId = charid;
            Nickname = name;

            LobbyCamera.AddBall(ball); */


            return ball.AddComponent<RemoteBall>();
        }



        public void DeusExMachina(RemoteMovement movement)
        {
            Rigidbody ballRb = Body.GetComponent<Rigidbody>();

            Body.transform.position = movement.Position;
            Body.transform.rotation = movement.Rotation;
            ballRb.velocity = movement.Velocity;
            ballRb.angularVelocity = movement.AngularVelocity;
            DirectionVector = movement.DirectionVector;
        }

        protected override Vector3 GetAcceleration()
        {
            return DirectionVector;
        }

        protected override void OnFinishLine() {}
    }
}