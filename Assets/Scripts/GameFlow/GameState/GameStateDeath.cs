using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class GameStateDeath : GameState, IUnityAdsListener
{
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;

    // Completion circle fields
    [SerializeField] private Image completionCircle;
    public float timeToDecision = 2.5f;
    private float deathTime;

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

        deathTime = Time.time;
        deathUI.SetActive(true);

        // Prior to saving, set the highscore if needed
        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score;
            currentScore.color = Color.green;

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
            currentScore.color = Color.white;
        }

        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instance.Save();

        highscore.text = "Highscore :  " + SaveManager.Instance.save.Highscore;
        currentScore.text = GameStats.Instance.ScoreToText();
        fishTotal.text = "Total fish :" + SaveManager.Instance.save.Fish;
        currentFish.text = GameStats.Instance.FishToText();
    }

    public override void Destruct()
    {
        deathUI.SetActive(false);
    }

    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision;
        completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        completionCircle.fillAmount = 1 - ratio;

        if (ratio > 1)
        {
            completionCircle.gameObject.SetActive(false);
        }
    }

    public void TryResumeGame()
    {
        AdManager.Instance.ShowRewardedAd();
    }

    public void ResumeGame()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameManager.Instance.RespawnPlayer();
    }

    public void ToMenu()
    { 
        brain.ChangeState(GetComponent<GameStateInit>());

        Reset();
    }

    private void Reset()
    {
        GameManager.Instance.Reset();
    }

    public void EnableRevive()
    { 
        completionCircle.gameObject.SetActive(true);
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
        completionCircle.gameObject.SetActive(false);
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
