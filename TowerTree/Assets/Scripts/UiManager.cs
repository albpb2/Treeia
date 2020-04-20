using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] 
    private Text _levelNumberText;
    [SerializeField] 
    private Text _bulletsCountText;

    private MainCharacterGun _playerGun;
    
    protected override void Awake()
    {
        base.Awake();

        _playerGun = GameObject.FindWithTag(Tags.Player).GetComponentInChildren<MainCharacterGun>();
    }

    private void Update()
    {
        _bulletsCountText.text = _playerGun.InfiniteBullets ? "-" : _playerGun.BulletsCount.ToString();
    }

    public void SetLevelNumberText(int levelNumber)
    {
        _levelNumberText.text = $"Floor {levelNumber}";
    }
}
