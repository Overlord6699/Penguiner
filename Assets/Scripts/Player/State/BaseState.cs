using Animation;
using UnityEngine;

namespace Player.State
{
    public abstract class BaseState : MonoBehaviour
    {
        protected const int RIGHT = 1, LEFT = -1;
        
        protected PlayerMotor motor;
        protected AnimationController animController;

        public virtual void Construct(AnimationController controller)
        {
            animController = controller;
        }

        public virtual void Destruct()
        {
        }

        public virtual void Transition()
        {
        }

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
}