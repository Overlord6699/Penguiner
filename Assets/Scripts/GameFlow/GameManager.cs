using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum GameCamera
{
    Init = 0,
    Game = 1,
    Shop = 2,
    Respawn = 3
}

public class GameManager : MonoBehaviour
{
    public delegate void ProcessGamePause();
    public event ProcessGamePause OnGamePaused, OnGameResumed;

    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public Transform MotorTransform { get { return motor.transform; } }
    [SerializeField]
    private PlayerMotor motor;
    [SerializeField]
    private WorldGeneration worldGeneration;


    [SerializeField]
    private SceneChunkGeneration sceneChunkGeneration;
    [SerializeField]
    private GameObject[] cameras;
    [SerializeField]
    private bool isConnectedToGooglePlayServices;
    public bool IsConnectedToGooglePlayServices { get { return isConnectedToGooglePlayServices; } }

    private GameState state;

    private void Awake()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
        Debug.Log(Application.persistentDataPath);

        instance = this;
    }

    private void Start()
    {       
        state = GetComponent<GameStateInit>();
        state.Construct();

        SignInToGooglePlayServices();
    }

    public void SignInToGooglePlayServices()
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
    }

    private void Update()
    {
        state.UpdateState();
    }

    public void ChangeState(GameState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }

    public void ChangeCamera(GameCamera c)
    {
        foreach (GameObject go in cameras)
            go.SetActive(false);

        cameras[(int)c].SetActive(true);
    }

    public void Reset()
    {
        motor.ResetPlayer();
        worldGeneration.ResetWorld();
        sceneChunkGeneration.ResetWorld();
    }

    public void ScanPosition()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }

    public void RespawnPlayer()
    {
        motor.RespawnPlayer();
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
