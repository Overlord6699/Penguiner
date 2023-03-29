﻿using Animation;
using UnityEngine;

namespace Player.State
{
    public class RespawnState : BaseState
    {
        [SerializeField] private float verticalDistance = 25.0f;
        [SerializeField] private float immunityTime = 1f;

        private float startTime;

        public override void Construct(AnimationController cont)
        {
            base.Construct(cont);

            startTime = Time.time;

            motor.Controller.enabled = false;
            motor.transform.position = new Vector3(0, verticalDistance, motor.transform.position.z);
            motor.Controller.enabled = true;

            motor.VerticalVelocity = 0.0f;
            motor.CurrentLane = 0;
            motor.Respawn();
        }

        public override void Destruct()
        {
            GameManager.Instance.ChangeCamera(GameCamera.Game);
        }

        public override Vector3 ProcessMotion()
        {
            // Apply gravity
            motor.ApplyGravity();

            // Create our return vector
            Vector3 m = Vector3.zero;

            m.x = motor.SnapToLane();
            m.y = motor.VerticalVelocity;
            m.z = motor.baseRunSpeed;

            return m;
        }

        public override void Transition()
        {
            if (motor.IsGrounded && (Time.time - startTime) > immunityTime)
                motor.ChangeState(GetComponent<RunningState>());

            if (InputManager.Instance.SwipeLeft)
                motor.ChangeLane(LEFT);

            if (InputManager.Instance.SwipeRight)
                motor.ChangeLane(RIGHT);
        }
    }
}