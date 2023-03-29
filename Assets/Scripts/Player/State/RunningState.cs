using Animation;
using UnityEngine;

namespace Player.State
{
    public class RunningState : BaseState
    {
        //private Vector3 _prevAcc= Input.acceleration;

        public override void Construct(AnimationController cont)
        {
            base.Construct(cont);
            motor.VerticalVelocity = 0;
        }

        public override Vector3 ProcessMotion()
        {
            Vector3 m = Vector3.zero;

            m.x = motor.SnapToLane();
            m.y = -1.0f;
            m.z = motor.baseRunSpeed;

            return m;
        }

        public override void Transition()
        {
            /*var acc = Input.acceleration;
    
            if(acc.y-_prevAcc.y>0)
                motor.ChangeState(GetComponent<JumpingState>());
    
            if (acc.y - _prevAcc.y < 0)
                motor.ChangeState(GetComponent<SlidingState>());
    
            if (acc.x - _prevAcc.x > 0)
                motor.ChangeLane(RIGHT);
    
    
            if (acc.x - _prevAcc.x < 0)
                motor.ChangeLane(LEFT);
    
    
            if (!motor.isGrounded)
                motor.ChangeState(GetComponent<FallingState>());*/

            if (InputManager.Instance.SwipeUp && motor.IsGrounded)
                motor.ChangeState(GetComponent<JumpingState>());

            if (InputManager.Instance.SwipeDown)
                motor.ChangeState(GetComponent<SlidingState>());

            if (InputManager.Instance.SwipeLeft)
            {
                motor.ChangeLane(LEFT);
                Debug.Log("Swipe LEFT");
            }

            if (InputManager.Instance.SwipeRight)
            {
                motor.ChangeLane(RIGHT);
                Debug.Log("Swipe RIGHT");
            }

            if (!motor.IsGrounded)
                motor.ChangeState(GetComponent<FallingState>());



            //_prevAcc = acc;
        }
    }
}
