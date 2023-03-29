using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ShopStateView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _currentHatName;
        
        [SerializeField]
        private TextMeshProUGUI _totalFish;

        [SerializeField]
        private Image _completionCircle;
        
        [SerializeField]
        private TextMeshProUGUI _completionText;

        public void DisplayTotalFish(string text)
        {
            _totalFish.text = text;
        }

        public void DisplayCompletionCircle(float value)
        {
            _completionCircle.fillAmount = value;
        }

        public void DisplayCompletionText(string text)
        {
            _completionText.text = text;
        }
        
        public void DisplayCurrentHatName(string text)
        {
            _currentHatName.text = text;
        }
        
        
    }
}