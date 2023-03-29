using Audio;
using Controllers;
using Save;
using UnityEngine;

namespace GameFlow.GameState
{
    public class GameStateInit : GameState
    {
        [SerializeField]
        private LevelSliderController _levelController;
        
        [SerializeField]
        private GameObject _menuUI;

        [SerializeField] 
        private AudioClip _menuLoopMusic;

        [SerializeField]
        private InitGameController _controller;
       
        [SerializeField]
        private LevelConfigProvider _levelProvider;
    
        public override void Construct()
        {
            GameManager.Instance.ChangeCamera(GameCamera.Init);

            _controller.Init(SaveManager.Instance.SaveState.Fish, SaveManager.Instance.SaveState.Highscore);

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
            var level = _levelController.Level;
            var config = _levelProvider.GetConfigById(level);
            
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
}