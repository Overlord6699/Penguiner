using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI fishcountText;
    [SerializeField] private AudioClip menuLoopMusic;

    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        hiscoreText.text = "Highscore: " + SaveManager.Instance.save.Highscore.ToString();
        fishcountText.text = "Fish: " + SaveManager.Instance.save.Fish.ToString();

        menuUI.SetActive(true);

        if (SaveManager.Instance.save.Fish >= 300)
            //Social.ReportProgress(GPGSIds.achievement_money_in_the_bank, 100.0f, null);

        AudioManager.Instance.PlayMusicWithXFade(menuLoopMusic, 0.5f);
    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
        GetComponent<GameStateDeath>().EnableRevive();
    }

    public void OnShopClick()
    {
        brain.ChangeState(GetComponent<GameStateShop>());
    }

    public void OnAchievementClick()
    {
        if (GameManager.Instance.IsConnectedToGooglePlayServices)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            GameManager.Instance.SignInToGooglePlayServices();
        }
    }

    public void OnLeaderboardClick()
    {
        if (GameManager.Instance.IsConnectedToGooglePlayServices)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            GameManager.Instance.SignInToGooglePlayServices();
        }
    }
}