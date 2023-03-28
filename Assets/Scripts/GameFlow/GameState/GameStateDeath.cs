using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameStateDeath : GameState, IUnityAdsListener
{
    [SerializeField]
    private GameObject _deathUI;

    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private TextMeshProUGUI _currentScore;
    [SerializeField] private TextMeshProUGUI _fishTotal;
    [SerializeField] private TextMeshProUGUI _currentFish;

    // Completion circle fields
    [SerializeField] 
    private Image _completionCircle;
    [SerializeField]
    private float _decisionTime = 2.5f;


    private float _deathTime;

    private void OnEnable()
    {
        Advertisement.AddListener(this);
    }

    private void OnDisable()
    {
        Advertisement.RemoveListener(this);
    }

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.PauseGame();

        _deathTime = Time.time;
        _deathUI.SetActive(true);

        // Prior to saving, set the highscore if needed
        if (SaveManager.Instance.SaveState.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.SaveState.Highscore = (int)GameStats.Instance.score;
            _currentScore.color = Color.green;

            if (GameManager.Instance.IsConnectedToGooglePlayServices)
            {
                Debug.Log("Reporting score..");
                /*
                Social.ReportScore(SaveManager.Instance.save.Highscore, GPGSIds.leaderboard_top_score, (success) =>
                {
                    if (!success) Debug.LogError("Unable to post highscore");
                });
                */
                //Social.ReportProgress(GPGSIds.achievement_joining_the_ladder, 100.0f, null);
            }
            else
            {
                Debug.Log("Not signed in.. unable to report score");
            }
        }
        else
        {
            _currentScore.color = Color.white;
        }

        SaveManager.Instance.SaveState.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instance.Save();

        _highScore.text = "Highscore :  " + SaveManager.Instance.SaveState.Highscore;
        _currentScore.text = GameStats.Instance.ScoreToText();
        _fishTotal.text = "Total fish :" + SaveManager.Instance.SaveState.Fish;
        _currentFish.text = GameStats.Instance.FishToText();
    }

    public override void Destruct()
    {
        _deathUI.SetActive(false);
    }

    public override void UpdateState()
    {
        float ratio = (Time.time - _deathTime) / _decisionTime;
        _completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        _completionCircle.fillAmount = 1 - ratio;

        if (ratio > 1)
        {
            _completionCircle.gameObject.SetActive(false);
        }
    }

    public void TryResumeGame()
    {
        AdManager.Instance.ShowRewardedAd();
    }

    public void ResumeGame()
    {
        _brain.ChangeState(GetComponent<GameStateGame>());
        GameManager.Instance.RespawnPlayer();
    }

    public void ToMenu()
    { 
        _brain.ChangeState(GetComponent<GameStateInit>());

        Reset();
    }

    private void Reset()
    {
        GameManager.Instance.Reset();
    }

    public void EnableRevive()
    { 
        _completionCircle.gameObject.SetActive(true);
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        _completionCircle.gameObject.SetActive(false);
        switch (showResult)
        {
            case ShowResult.Failed:
                ToMenu();
                break;
            case ShowResult.Finished:
                ResumeGame();
                break;
            default:
                break;
        }
    }
}
