using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : Singleton<Player>
{
    private const int InitialStamina = 100;
    private const int MaxStamina = 100;
    private const float StaminaFillRatio = 5;
    
    [SerializeField] 
    private float _healthPoints = 100;
    [SerializeField] 
    private Image _staminaBar;

    private Tree _tree;
    private MainCharacterGun _gun;
    private MainCharacterController _mainCharacterController;
    private int _waterCount;
    private float _stamina = InitialStamina;

    protected override void Awake()
    {
        base.Awake();

        _gun = GetComponentInChildren<MainCharacterGun>();
        _mainCharacterController = GetComponentInChildren<MainCharacterController>();
    }

    private void Update()
    {
        if (_stamina < MaxStamina)
        {
            _stamina += StaminaFillRatio * Time.deltaTime;
            if (_stamina > MaxStamina)
            {
                _stamina = MaxStamina;
            }
        }

        _staminaBar.fillAmount = _stamina / MaxStamina;
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
        _tree = _tree ?? FindObjectOfType(typeof(Tree)) as Tree;
        if (_waterCount > 0)
        {
            _tree.Water();
            _waterCount--;
        }
    }

    public void SetActiveGun(Gun gun)
    {
        _gun.SetGun(gun);
    }

    public bool SpendSprintStamina()
    {
        const int staminaPerSprint = 30;
        if (_stamina > staminaPerSprint)
        {
            _stamina -= staminaPerSprint;
            return true;
        }

        return false;
    }
}