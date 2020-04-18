using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _healthPoints = 100;

    private Tree _tree;
    private int _waterCount;

    private void Start()
    {
        _tree = FindObjectOfType(typeof(Tree)) as Tree;
    }

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
        if (_waterCount > 0)
        {
            _tree.Water();
            _waterCount--;
        }
    }
}