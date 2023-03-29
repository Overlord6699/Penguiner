using System;
using UnityEngine;

[Serializable]
public class LevelConfiguration : ScriptableObject
{
    public PlayerConfiguration PlayerConfiguration => _playerConfig;

    [SerializeField]
    private PlayerConfiguration _playerConfig;
}
