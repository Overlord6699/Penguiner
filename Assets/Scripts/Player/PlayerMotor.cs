using Animation;
using Player.State;
using UnityEngine;
using Audio;


public class PlayerMotor : MonoBehaviour
{

    [HideInInspector]
    public float VerticalVelocity;
    [HideInInspector] 
    public bool IsGrounded;
    [HideInInspector] 
    public int CurrentLane;


    [SerializeField]
    private float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

     public CharacterController Controller;
    [SerializeField]
    private AnimationController _animController;

    private BaseState state;
    
    private Vector3 _moveVector;
    private bool isPaused;

    private void Start()
    {
        //Controller = GetComponent<CharacterController>();
        //_animController = GetComponent<AnimationController>();
        state = GetComponent<RunningState>();
        
        state.Construct(_animController);

        //TODO Remove pause logic
        isPaused = true;
    }

    private void Update()
    {
        if(!isPaused)
            UpdateMotor();
    }
    private void UpdateMotor()
    {
        // Check if we're grounded
        IsGrounded = Controller.isGrounded;

        // How should we be moving right now? based on state
        _moveVector = state.ProcessMotion();

        // Are we trying to change state?
        state.Transition();

        // Feed our animator some values
        _animController.SetGrounded(IsGrounded);
        _animController.SetSpeed(Mathf.Abs(_moveVector.z));

        // Move the player
        Controller.Move(_moveVector * Time.deltaTime);
    }
    public float SnapToLane()
    {
        float r = 0.0f;

        // If we're not directly on top of a lane
        if (transform.position.x != (CurrentLane * distanceInBetweenLanes))
        {
            float deltaToDesiredPosition = (CurrentLane * distanceInBetweenLanes) - transform.position.x;
            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed;

            float actualDistance = r * Time.deltaTime;
            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
                r = deltaToDesiredPosition * (1 / Time.deltaTime);
        }
        else
        {
            r = 0;
        }

        return r;
    }
    public void ChangeLane(int direction)
    {
        CurrentLane = Mathf.Clamp(CurrentLane + direction, -1, 1);
    }
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct(_animController);
    }
    public void ApplyGravity()
    {
        VerticalVelocity -= gravity * Time.deltaTime;
        if (VerticalVelocity < -terminalVelocity)
            VerticalVelocity = -terminalVelocity;
    }

    public void PausePlayer()
    {
        isPaused = true;
    }
    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public void ResetPlayer()
    {
        CurrentLane = 0;
        transform.position = Vector3.zero;
        _animController.StartIdleAnimation();
        
        PausePlayer();
        ChangeState(GetComponent<RunningState>());
    }

    public void Respawn()
    {
        _animController.TriggerRespawn();
        
    }

    public void Die()
    {
        _animController.TriggerDeath();

    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if (hitLayerName == "Death")
        {
            AudioManager.Instance.PlayHitSFX();
            ChangeState(GetComponent<DeathState>());
        }
    }
}
