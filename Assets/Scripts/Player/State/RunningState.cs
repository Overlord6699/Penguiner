using UnityEngine;

public class RunningState : BaseState
{
    public override void Construct(AnimationController cont)
    {
        base.Construct(cont);
        motor.verticalVelocity = 0;
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
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(LEFT);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(RIGHT);

        if (InputManager.Instance.SwipeUp && motor.isGrounded)
            motor.ChangeState(GetComponent<JumpingState>());

        if(!motor.isGrounded)
            motor.ChangeState(GetComponent<FallingState>());

        if (InputManager.Instance.SwipeDown)
            motor.ChangeState(GetComponent<SlidingState>());
    }
}
