using UnityEngine;

public class GamePauser : MonoBehaviour
{
    [SerializeField]
    private PlayerMotor _player;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += PauseGame;
        GameManager.Instance.OnGameResumed += ResumeGame;
    }

    public void ResumeGame()
    {
        _player.ResumePlayer();
    }

    public void PauseGame()
    {
        _player.PausePlayer();
    }
}
