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
        private TextMeshProUGUI _totalFishText;
        [SerializeField]
        private TextMeshProUGUI _currentHatNameText;
        [SerializeField]
        private HatLogic _hatLogic;

        private bool isInit = false;
        private int hatCount;
        private int unlockedHatCount;

        // Shop Item
        [SerializeField]
        private GameObject hatPrefab;
        [SerializeField]
        private Transform hatContainer;



        // Completion Circle
        [SerializeField]
        private Image completionCircle;
        [SerializeField]
        private TextMeshProUGUI completionText;

        private Hat[] hats;

        public override void Construct()
        {
            GameManager.Instance.ChangeCamera(GameCamera.Shop);
            hats = Resources.LoadAll<Hat>("Hat");
            _shopUI.SetActive(true);

            if (!isInit)
            { 
                _totalFishText.text = SaveManager.Instance.SaveState.Fish.ToString("000");
                _currentHatNameText.text = hats[SaveManager.Instance.SaveState.CurrentHatIndex].ItemName;
                PopulateShop();
                isInit = true;
            }

            ResetCompletionCircle();
        }

        public override void Destruct()
        {
            _shopUI.SetActive(false);
        }

        private void PopulateShop()
        {
            for (int i = 0; i < hats.Length; i++)
            {
                int index = i;
                GameObject go = Instantiate(hatPrefab, hatContainer);
                // Button
                go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index));
                // Thumbnail
                go.transform.GetChild(0).GetComponent<Image>().sprite = hats[index].Thumbnail;
                // ItemName
                go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
                // Price
                if (SaveManager.Instance.SaveState.UnlockedHatFlag[i] == 0)
                    go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();
                else
                { 
                    go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                    unlockedHatCount++;
                }
            }
        }

        private void OnHatClick(int i)
        {
            if (SaveManager.Instance.SaveState.UnlockedHatFlag[i] == 1)
            {
                SaveManager.Instance.SaveState.CurrentHatIndex = i;
                _currentHatNameText.text = hats[i].ItemName;
                _hatLogic.SelectHat(i);
                SaveManager.Instance.Save();
            }
            // If we don't have it, can we buy it?
            else if (hats[i].ItemPrice <= SaveManager.Instance.SaveState.Fish)
            {
                SaveManager.Instance.SaveState.Fish -= hats[i].ItemPrice;
                SaveManager.Instance.SaveState.UnlockedHatFlag[i] = 1;
                SaveManager.Instance.SaveState.CurrentHatIndex = i;
                _currentHatNameText.text = hats[i].ItemName;
                _hatLogic.SelectHat(i);
                _totalFishText.text = SaveManager.Instance.SaveState.Fish.ToString("000");
                SaveManager.Instance.Save();
                hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                unlockedHatCount++;
                ResetCompletionCircle();

                //Social.ReportProgress(GPGSIds.achievement_classy, 100.0f, null);
            }
            // Don't have it, can't buy it
            else
            {
                Debug.Log("Not enough fish");
            }
        }

        private void ResetCompletionCircle()
        {
            int hatCount = hats.Length - 1;
            int currentlyUnlockedCount = unlockedHatCount - 1;

            completionCircle.fillAmount = (float)currentlyUnlockedCount / (float)hatCount;
            completionText.text = currentlyUnlockedCount + " / " + hatCount;

            //if(hatCount == currentlyUnlockedCount)
            //Social.ReportProgress(GPGSIds.achievement_buy_all_the_hats, 100.0f, null);
        }

        public void OnHomeClick()
        {
            _brain.ChangeState(GetComponent<GameStateInit>());
        }
    }
}
