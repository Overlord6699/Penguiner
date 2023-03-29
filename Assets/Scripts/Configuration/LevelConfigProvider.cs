using UnityEngine;

public class LevelConfigProvider : MonoBehaviour
{
    [SerializeField]
    private LevelConfiguration[] _levels;
    
    public LevelConfiguration GetConfigById(int id)
    {
        //TODO search by name?
        //array order
        return _levels[id - 1];
    }
}
