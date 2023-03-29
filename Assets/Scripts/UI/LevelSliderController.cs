using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSliderController : MonoBehaviour
{
    [SerializeField]
    private int _numOfLevels = 5;
    [SerializeField]
    private Slider _slider;
    private float[] _levelValues;

    public int Level => _level;
    private int _level = 0;
    
    private void Awake()
    {
        _slider.onValueChanged.AddListener(delegate { CalculateLevel(); });

        var part = (float) 1f/(_numOfLevels-1);
        _levelValues = new float[_numOfLevels];
        for (int i = 0; i < _numOfLevels; i++)
        {
            _levelValues[i] = part * i;
        }
    }

    public void CalculateLevel()
    {
        var value = _slider.value;
        _level = 1;
        
        for (int i = 1; i < _numOfLevels; i += 1)
        {
            if (value >= _levelValues[i])
                _level++;
            else
                break;
        }
    }

}
