﻿using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _healthPoints = 100;

    private int _waterCount;

    public void Hurt(float damage)
    {
        _healthPoints -= damage;
        if (_healthPoints <= 0)
        {
            _healthPoints = 0;
        }
        
        Debug.Log($"Received {damage} damage points. Remaining HP = {_healthPoints}");
        if (_healthPoints == 0)
        {
            Debug.Log("You're dead");
        }
    }

    public void PickWater()
    {
        _waterCount++;
        Debug.Log($"Water obtained. Available: {_waterCount}");
    }
}