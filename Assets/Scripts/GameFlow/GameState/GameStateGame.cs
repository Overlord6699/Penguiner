using TMPro;
using UnityEngine;

public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI fishCount;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private AudioClip gameLoopMusic;

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.ChangeCamera(GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFish;
        GameStats.Instance.OnScoreChange += OnScoreChange;

        gameUI.SetActive(true);

        AudioManager.Instance.PlayMusicWithXFade(gameLoopMusic, 0.5f);
    }

    private void OnCollectFish(int amnCollected)
    {
        fishCount.text = GameStats.Instance.FishToText();
    }
    private void OnScoreChange(float score)
    {
        scoreCount.text = GameStats.Instance.ScoreToText();
    }

    public override void Destruct()
    {
        gameUI.SetActive(false);

        GameStats.Instance.OnCollectFish -= OnCollectFish;
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }
    public override void UpdateState()
    {
        GameManager.Instance.ScanPosition();
    }
}
