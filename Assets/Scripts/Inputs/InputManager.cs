using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // There should be only one InputManager in the scene
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    // Action schemes
    private RunnerInputAction actionScheme;

    // Configuration
    [SerializeField] private float sqrSwipeDeadzone = 50.0f;

    #region public properties
    public bool Tap { get { return _tap; } }
    public Vector2 TouchPosition { get { return _touchPosition; } }
    public bool SwipeLeft => _swipeLeft;
    public bool SwipeRight { get { return _swipeRight; } }
    public bool SwipeUp { get { return _swipeUp; } }
    public bool SwipeDown { get { return _swipeDown; } }
    #endregion

    #region privates
    private bool _tap;
    private Vector2 _touchPosition;
    private Vector2 _startDrag;
    private bool _swipeLeft;
    private bool _swipeRight;
    private bool _swipeUp;
    private bool _swipeDown;
    #endregion

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SetupControl();
    }
    
    public void OnEnable()
    {
        actionScheme.Enable();
    }
    public void OnDisable()
    {
        actionScheme.Disable();
    }
    
    private void LateUpdate()
    {
        ResetInputs();
    }
    private void ResetInputs()
    {
        _tap = _swipeLeft = _swipeRight = _swipeUp = _swipeDown = false;
    }

    private void SetupControl()
    {
        actionScheme = new RunnerInputAction();

        // Register different actions
        actionScheme.Gameplay.Tap.performed += ctx => OnTap(ctx);
        actionScheme.Gameplay.TouchPosition.performed += ctx => OnPosition(ctx);
        actionScheme.Gameplay.StartDrag.performed += ctx => OnStartDrag(ctx);
        actionScheme.Gameplay.EndDrag.performed += ctx => OnEndDrag(ctx);
    }

    private void OnEndDrag(InputAction.CallbackContext ctx)
    {
        Vector2 delta = _touchPosition - _startDrag;
        float sqrDistance = delta.sqrMagnitude;

        // Confirmed swipe
        if (sqrDistance > sqrSwipeDeadzone)
        {
            float x = Mathf.Abs(delta.x);
            float y = Mathf.Abs(delta.y);

            if (x > y) // Left or Right
            {
                if (delta.x > 0)
                    _swipeRight = true;
                else
                    _swipeLeft = true;
            }
            else // Up or Down
            {
                if (delta.y > 0)
                    _swipeUp = true;
                else
                    _swipeDown = true;
            }
        }

        _startDrag = Vector2.zero;
    }
    private void OnStartDrag(InputAction.CallbackContext ctx)
    {
        _startDrag = _touchPosition;
    }
    private void OnPosition(InputAction.CallbackContext ctx)
    {
        _touchPosition = ctx.ReadValue<Vector2>();
    }
    private void OnTap(InputAction.CallbackContext ctx)
    {
        _tap = true;
    }
}

/*
 *          DYNAMIC UPDATE
 *          InputManager that processes the inputs
 *          PlayerMotor uses these inputs to move
 *          
 *          LATE UPDATE
 *          InputManager resets these inputs
 */