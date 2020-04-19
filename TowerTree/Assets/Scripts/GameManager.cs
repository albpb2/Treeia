﻿using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private LevelManager _levelManager;

    private int _currentPoints;
    private bool _gameStarted;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Game manager awaking");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game manager starting");
        _levelManager = LevelManager.Instance;

        if (!_gameStarted)
        {
            _levelManager.TargetWaterCount = 3;
            _levelManager.SecondsPerWater = 20;
            _levelManager.StartNextLevel();
        }
    }

    public void AddPoints(int points)
    {
        _currentPoints += points;
        Debug.Log($"{points} points earned. Current points = {_currentPoints}");
    }
}