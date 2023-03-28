using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum GameCamera
{
    Init,
    Game,
    Shop,
    Respawn
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public delegate void ProcessGamePause();
    public event ProcessGamePause OnGamePaused, OnGameResumed;



    public Transform MotorTransform { get { return _motor.transform; } }
    [SerializeField]
    private PlayerMotor _motor;


    [SerializeField]
    private WorldGeneration _worldGeneration;


    [SerializeField]
    private SceneChunkGeneration _sceneChunkGeneration;
    [SerializeField]
    private GameObject[] _cameras;

    [SerializeField]
    private bool _isConnectedToGooglePlayServices;
    public bool IsConnectedToGooglePlayServices { get { return _isConnectedToGooglePlayServices; } }


    private GameState _state;

    private void Awake()
    {
#if UNITY_ANDROID
        /*PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();*/
#endif
        Debug.Log(Application.persistentDataPath);

        instance = this;
    }

    private void Start()
    {       
        _state = GetComponent<GameStateInit>();
        _state.Construct();

        //SignInToGooglePlayServices();
    }

 /*   public void SignInToGooglePlayServices()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) => {
            switch (result)
            {
                case SignInStatus.Success:
                    isConnectedToGooglePlayServices = true;
                    break;
                default:
                    isConnectedToGooglePlayServices = false;
                    break;
            }
        });
#endif
    }*/

    private void Update()
    {
        _state.UpdateState();
    }

    public void ChangeState(GameState state)
    {
        _state.Destruct();
        _state = state;
        _state.Construct();
    }

    public void ChangeCamera(GameCamera cam)
    {
        foreach (GameObject go in _cameras)
            go.SetActive(false);

        _cameras[(int)cam].SetActive(true);
    }

    public void Reset()
    {
        _motor.ResetPlayer();
        _worldGeneration.ResetWorld();
        _sceneChunkGeneration.ResetWorld();
    }

    public void ScanPosition()
    {
        _worldGeneration.ScanPosition();
        _sceneChunkGeneration.ScanPosition();
    }

    public void RespawnPlayer()
    {
        _motor.RespawnPlayer();
    }

#region PAUSE
    public void ResumeGame()
    {
        OnGameResumed?.Invoke();
    }

    public void PauseGame()
    {
        OnGamePaused?.Invoke();
    }
#endregion
}
