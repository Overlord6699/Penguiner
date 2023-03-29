using Audio;
using Controllers;
using UnityEngine;

namespace GameFlow.GameState
{
    public class GameStateGame : GameState
    {
        public GameObject gameUI;
        [SerializeField]
        private GameStateController _controller;
        
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
            _controller.DisplayFishCount();
        }
        private void OnScoreChange(float score)
        {
            _controller.DisplayScoreCount();
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
}
