using UnityEngine;

public class GamePauser : MonoBehaviour
{
    [SerializeField]
    private PlayerMotor player;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += PauseGame;
        GameManager.Instance.OnGameResumed += ResumeGame;
    }

    public void ResumeGame()
    {
        player.ResumePlayer();
    }

    public void PauseGame()
    {
        player.PausePlayer();
    }
}
