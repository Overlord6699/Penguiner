using UnityEngine;

public class JumpingState : BaseState
{
    public float jumpForce = 7.0f;

    public override void Construct(AnimationController cont)
    {
        base.Construct(cont);
 
        Jump();
    }

    private void Jump()
    {
        motor.verticalVelocity = jumpForce;
        animController?.TriggerJump();
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

        if (motor.verticalVelocity < 0)
            motor.ChangeState(GetComponent<FallingState>());
    }
}
