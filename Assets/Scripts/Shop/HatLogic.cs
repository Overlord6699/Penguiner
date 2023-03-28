using System.Collections.Generic;
using UnityEngine;

public class HatLogic : MonoBehaviour
{
    private const string FOLDER_NAME = "Hat";

    [SerializeField] 
    private Transform _hatContainer;
    private List<GameObject> _hatModels = new List<GameObject>();
    private Hat[] _hats;

    private void Start()
    {
        _hats = Resources.LoadAll<Hat>(FOLDER_NAME);

        if (_hats != null)
        {
            SpawnHats();
            SelectHat(SaveManager.Instance.SaveState.CurrentHatIndex);
        }
        else
            Debug.Log("There are no hats");
       
    }

    private void SpawnHats()
    {
        for (int i = 0; i < _hats.Length; i++)
        {
            _hatModels.Add(Instantiate(_hats[i].Model, _hatContainer) as GameObject);
        }
    }

    public void DisableAllHats()
    {
        for (int i = 0; i < _hatModels.Count; i++)
        {
            _hatModels[i].SetActive(false);
        }
    }

    public void SelectHat(int index)
    {
        DisableAllHats();
        _hatModels[index].SetActive(true);
    }
}
