using UnityEngine;

public class FallingState : BaseState
{
    public override void Construct(AnimationController cont)
    {
        base.Construct(cont);

        Fall();
    }

    private void Fall()
    {
        animController?.TriggerFall();
    }

    public override Vector3 ProcessMotion()
    {
        // Apply gravity
        motor.ApplyGravity();

        // Create our return vector
        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = motor.verticalVelocity;
        m.z = motor.baseRunSpeed;

        return m;
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
            motor.ChangeLane(LEFT);

        if (InputManager.Instance.SwipeRight)
            motor.ChangeLane(RIGHT);

        if (motor.isGrounded)
            motor.ChangeState(GetComponent<RunningState>());
    }
}