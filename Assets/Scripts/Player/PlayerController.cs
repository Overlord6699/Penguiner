using System.Collections;
using System.Collections.Generic;
using Player.State;
using UnityEngine;

namespace Player
{
    
    public class PlayerController : MonoBehaviour
    {
        public void Init(PlayerConfiguration config)
        {
 
            GetComponent<JumpingState>().jumpForce = config.JumpForce;
            GetComponent<SlidingState>().slideDuration = config.SlidingDuration;

            var motor = GetComponent<PlayerMotor>();
            motor.baseRunSpeed = config.RunSpeed;
            motor.gravity = config.Gravity;
            motor.baseSidewaySpeed = config.SideAwaySpeed;
        }
    }  
}


