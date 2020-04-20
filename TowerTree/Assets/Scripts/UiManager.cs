using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] 
    private Text _levelNumberText;
    [SerializeField] 
    private Text _bulletsCountText;
    [SerializeField] 
    private Text _gunText;
    [SerializeField] 
    private Image _gunImage;

    private MainCharacterGun _playerGun;
    private Gun _equippedGun;
    
    protected override void Awake()
    {
        base.Awake();

        _playerGun = GameObject.FindWithTag(Tags.Player).GetComponentInChildren<MainCharacterGun>();
    }

    private void Update()
    {
        _bulletsCountText.text = _playerGun.InfiniteBullets ? "-" : _playerGun.BulletsCount.ToString();
        if (_equippedGun == null || _playerGun.EquippedGun != _equippedGun)
        {
            _equippedGun = _playerGun.EquippedGun;
            _gunText.text = _equippedGun.gunName;
            _gunImage.sprite = _equippedGun.sprite;
        }
    }

    public void SetLevelNumberText(int levelNumber)
    {
        _levelNumberText.text = $"Floor {levelNumber}";
    }
}
