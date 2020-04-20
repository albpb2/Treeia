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
    [SerializeField] 
    private Image _hpBar;

    private Player _player;
    private MainCharacterGun _playerGun;
    private Gun _equippedGun;
    private float _previousHpFillAmount;
    private readonly Color _green = new Color32(56, 120, 45, 255);
    private readonly Color _orange = new Color32(255, 142, 4, 255);
    private readonly Color _red = new Color32(241, 60, 47, 255);
    
    protected override void Awake()
    {
        base.Awake();

        _player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
        _playerGun = _player.GetComponentInChildren<MainCharacterGun>();
        _previousHpFillAmount = 1;
        _hpBar.color = _green;
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

        _hpBar.fillAmount = _player.HealthPoints / Player.MaxHealthPoints;
        if (_previousHpFillAmount > .3 && _hpBar.fillAmount <= .3)
        {
            _hpBar.color = _red;
        }
        else if ((_previousHpFillAmount <= .3 || _previousHpFillAmount >.5) && (_hpBar.fillAmount > 0.3 && _hpBar.fillAmount <= .5))
        {
            _hpBar.color = _orange;
        }
        else if (_previousHpFillAmount <= -5)
        {
            _hpBar.color = _green;
        }

        _previousHpFillAmount = _hpBar.fillAmount;
    }

    public void SetLevelNumberText(int levelNumber)
    {
        _levelNumberText.text = $"Floor {levelNumber}";
    }
}
