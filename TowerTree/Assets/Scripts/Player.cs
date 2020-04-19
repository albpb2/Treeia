using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] 
    private float _healthPoints = 100;

    private Tree _tree;
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

    public void WaterTree()
    {
        _tree = _tree ?? FindObjectOfType(typeof(Tree)) as Tree;
        if (_waterCount > 0)
        {
            _tree.Water();
            _waterCount--;
        }
    }
}