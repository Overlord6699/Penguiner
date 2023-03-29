using Controllers;
using Save;
using UnityEngine;
using UnityEngine.Advertisements;

namespace GameFlow.GameState
{
    public class GameStateDeath : GameState, IUnityAdsListener
    {
        [SerializeField] private GameObject _deathUI;

        [SerializeField] private DeathStateController _deathStateController;
        [SerializeField] private TimerController _deathTimerController;

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

            _deathTimerController.Init(Time.time);
            _deathUI.SetActive(true);

            // Prior to saving, set the highscore if needed
            if (SaveManager.Instance.SaveState.Highscore < (int) GameStats.Instance.score)
            {
                SaveManager.Instance.SaveState.Highscore = (int) GameStats.Instance.score;
                _deathStateController.SetCurrentScoreColor(Color.green);

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
                _deathStateController.SetCurrentScoreColor(Color.white);
            }

            SaveManager.Instance.SaveState.Fish += GameStats.Instance.fishCollectedThisSession;
            SaveManager.Instance.Save();

            _deathStateController.DisplayHighscore(SaveManager.Instance.SaveState.Highscore.ToString());
            _deathStateController.DisplayCurrentFish(GameStats.Instance.FishToText());
            _deathStateController.DisplayFishTotal(SaveManager.Instance.SaveState.Fish.ToString());
            _deathStateController.DisplayCurrentScore(GameStats.Instance.ScoreToText());
        }

        public override void Destruct()
        {
            _deathUI.SetActive(false);
        }

        public override void UpdateState()
        {
            _deathTimerController.UpdateState();
            
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
            _deathTimerController.ShowLoading();
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
            _deathTimerController.HideLoading();
            
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
}
