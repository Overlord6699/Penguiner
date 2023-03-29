using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig")][Serializable]
public class LevelConfiguration : ScriptableObject
{
    public float RunSpeed => _runSpeed;
    public float Gravity => _gravity;
    public float SideAwaySpeed =>_sideAwaySpeed;
    public float JumpForce => _jumpForce;
    public float SlidingDuration => _slidingDuration;

    public PlayerConfiguration PlayerConfiguration => _playerConfig;
    
    [SerializeField]
    private float _runSpeed;
    [SerializeField]
    private float _gravity;
    [SerializeField]
    private float _sideAwaySpeed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _slidingDuration;

    [SerializeField]
    private PlayerConfiguration _playerConfig;
}
