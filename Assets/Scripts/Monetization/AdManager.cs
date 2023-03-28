using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get { return _instance; } }
    private static AdManager _instance;

    [SerializeField] private string _gameID;
    [SerializeField] private string _rewardedVideoPlacementId;
    [SerializeField] private bool _testMode;

    private void Awake()
    {
        _instance = this;
        Advertisement.Initialize(_gameID, _testMode);
    }
    public void ShowRewardedAd()
    {
        ShowOptions so = new ShowOptions();
        Advertisement.Show(_rewardedVideoPlacementId, so);
    }
}
