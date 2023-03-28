using TMPro;
using UnityEngine;

public class GameStateInit : GameState
{
    [SerializeField]
    private GameObject _menuUI;
    [SerializeField] 
    private TextMeshProUGUI _hiscoreText;
    [SerializeField] 
    private TextMeshProUGUI _fishcountText;
    [SerializeField] 
    private AudioClip _menuLoopMusic;

    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        _hiscoreText.text = "Highscore: " + SaveManager.Instance.SaveState.Highscore.ToString();
        _fishcountText.text = "Fish: " + SaveManager.Instance.SaveState.Fish.ToString();

        _menuUI.SetActive(true);

        if (SaveManager.Instance.SaveState.Fish >= 300)
            //Social.ReportProgress(GPGSIds.achievement_money_in_the_bank, 100.0f, null);

        AudioManager.Instance.PlayMusicWithXFade(_menuLoopMusic, 0.5f);
    }

    public override void Destruct()
    {
        _menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        _brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
        GetComponent<GameStateDeath>().EnableRevive();
    }

    public void OnShopClick()
    {
        _brain.ChangeState(GetComponent<GameStateShop>());
    }

    public void OnAchievementClick()
    {
        if (GameManager.Instance.IsConnectedToGooglePlayServices)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            //GameManager.Instance.SignInToGooglePlayServices();
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
            //GameManager.Instance.SignInToGooglePlayServices();
        }
    }
}