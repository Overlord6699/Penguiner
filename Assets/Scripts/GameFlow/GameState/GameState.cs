using UnityEngine;

namespace GameFlow.GameState
{
    public abstract class GameState : MonoBehaviour, IGameState
    {
        protected GameManager _brain;

        protected virtual void Awake()
        {
            _brain = GetComponent<GameManager>();
        }

        public virtual void Construct()
        {
            Debug.Log("Constructing : " + this.ToString());
        }

        public virtual void Destruct()
        {

        }

        public virtual void UpdateState()
        {

        }
    }
}
