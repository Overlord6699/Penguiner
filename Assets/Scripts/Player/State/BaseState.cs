using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected const int RIGHT = 1, LEFT = -1;
    protected PlayerMotor motor;
    protected AnimationController animController;

    public virtual void Construct(AnimationController cont) 
    {
        animController = cont;
    }
    public virtual void Destruct(){ }
    public virtual void Transition(){ }

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    public virtual Vector3 ProcessMotion()
    {
        Debug.Log("Process motion is not implemented in " + this.ToString());
        return Vector3.zero;
    }
}
