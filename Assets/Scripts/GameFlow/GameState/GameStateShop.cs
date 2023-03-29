using Controllers;
using Save;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFlow.GameState
{
    public class GameStateShop : GameState
    {
        [SerializeField]
        private GameObject _shopUI;
        [SerializeField]
        private ShopStateController _shopStateController;
        
        [SerializeField]
        private HatLogic _hatLogic;

        private bool isInit = false;
        

        private void Subscribe()
        {
            _shopStateController.OnHatSelected += _hatLogic.SelectHat;
        }

        private void Unsubscribe()
        {
            
            _shopStateController.OnHatSelected -= _hatLogic.SelectHat;
        }
        
        public override void Construct()
        {
            GameManager.Instance.ChangeCamera(GameCamera.Shop);
            _shopStateController.LoadHats();
            _shopUI.SetActive(true);

           
            Subscribe();
            
            if (!isInit)
            {
                _shopStateController.Init();

                PopulateShop();
                isInit = true;
            }

            _shopStateController.CountObtainedHats();
        }

        public override void Destruct()
        {
            Unsubscribe();
            _shopUI.SetActive(false);
        }

        private void PopulateShop()
        {
           _shopStateController.SpawnHats();
        }

        public void OnHomeClick()
        {
            _brain.ChangeState(GetComponent<GameStateInit>());
        }
    }
}
