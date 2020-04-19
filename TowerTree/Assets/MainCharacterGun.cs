using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterGun : MonoBehaviour
{
    [SerializeField] 
    private SpriteRenderer _fireUpSprite;
    [SerializeField] 
    private SpriteRenderer _fireDownSprite;
    [SerializeField] 
    private SpriteRenderer _fireLeftSprite;
    [SerializeField] 
    private SpriteRenderer _fireRightSprite;
    [SerializeField] 
    private Gun _equippedGun;
    [SerializeField] 
    private int _gunShotsPoolSize;
    [SerializeField] 
    private GameObject _gunShotsRoot;
    [SerializeField] 
    private GameObject _gunShotPrefab;

    private Dictionary<string, GameObject> _spritesPerKey;
    private Dictionary<string, Vector2> _directionsPerKey;
    private string[] _spriteKeys;
    private string _shootingDirection;
    private bool _gunShot;
    private float _lastShotTime = 0;
    private Transform _characterTransform;
    private GameObject[] _gunShotsPool;
    private int _currentGunShotIndex;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _spritesPerKey = new Dictionary<string, GameObject>
        {
            [VirtualInputKeyNames.FireUp] = _fireUpSprite.gameObject,
            [VirtualInputKeyNames.FireDown] = _fireDownSprite.gameObject,
            [VirtualInputKeyNames.FireLeft] = _fireLeftSprite.gameObject,
            [VirtualInputKeyNames.FireRight] = _fireRightSprite.gameObject,
        };

        _spriteKeys = new[]
        {
            VirtualInputKeyNames.FireUp,
            VirtualInputKeyNames.FireDown,
            VirtualInputKeyNames.FireLeft,
            VirtualInputKeyNames.FireRight,
        };
        
        _directionsPerKey = new Dictionary<string, Vector2>
        {
            [VirtualInputKeyNames.FireUp] = Vector2.up,
            [VirtualInputKeyNames.FireDown] = Vector2.down,
            [VirtualInputKeyNames.FireLeft] = Vector2.left,
            [VirtualInputKeyNames.FireRight] = Vector2.right,
        };

        _characterTransform = gameObject.GetComponentInParent<Transform>();

        _gunShotsPool = new GameObject[_gunShotsPoolSize];
        for (var i = 0; i < _gunShotsPoolSize; i++)
        {
            _gunShotsPool[i] = Instantiate(_gunShotPrefab, _gunShotsRoot.transform);
            _gunShotsPool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(_shootingDirection))
        {
            if (Math.Abs(Input.GetAxis(_shootingDirection)) < 0.1)
            {
                _gunShot = false;
                _spritesPerKey[_shootingDirection].gameObject.SetActive(false);
                _shootingDirection = null;
                if (_equippedGun.automatic)
                {
                    _audioSource.Stop();
                }
            }
        }

        if (_equippedGun != null)
        {
            if (string.IsNullOrEmpty(_shootingDirection))
            {
                for (var i = 0; i < _spriteKeys.Length; i++)
                {
                    if (Input.GetAxis(_spriteKeys[i]) > 0)
                    {
                        _spritesPerKey[_spriteKeys[i]].gameObject.SetActive(true);
                        _shootingDirection = _spriteKeys[i];
                        break;
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(_shootingDirection) && (!_gunShot || (_equippedGun.automatic && IsAutomaticGunCooledDown())))
            {
                Debug.Log("Shooting");
                var hits = Physics2D.RaycastAll(_characterTransform.position, _directionsPerKey[_shootingDirection]);
                if (hits != null && hits.Length > 0)
                {
                    for (var i = 0; i < hits.Length; i++)
                    {
                        if (!hits[i].collider.isTrigger)
                        {
                            _gunShotsPool[_currentGunShotIndex].transform.position = hits[i].point;
                            _gunShotsPool[_currentGunShotIndex].SetActive(true);
                            StartCoroutine(DisableGunShot(_gunShotsPool[_currentGunShotIndex]));
                            _currentGunShotIndex = (_currentGunShotIndex + 1) % _gunShotsPoolSize;

                            if (hits[i].transform.tag == Tags.Enemy)
                            {
                                var enemy = hits[i].transform.GetComponent<Enemy>();
                                enemy.Hurt(_equippedGun.damage);
                            }

                            break;
                        }
                    }
                }

                if (_equippedGun.soundClip != null)
                {
                    if (!_equippedGun.automatic)
                    {
                        _audioSource.PlayOneShot(_equippedGun.soundClip);
                    }
                    else if (!_gunShot)
                    {
                        _audioSource.clip = _equippedGun.soundClip;
                        _audioSource.loop = true;
                        _audioSource.Play();
                    }
                }

                _gunShot = true;
                _lastShotTime = Time.time;
            }
        }
    }

    public void SetGun(Gun gun)
    {
        _equippedGun = gun;
    }
    
    private bool IsAutomaticGunCooledDown()
    {
        var a =  Time.time - _lastShotTime > _equippedGun.shootFrequencySeconds;
        if (!a)
        {
            Debug.Log("Not cooled down");
        }

        return a;
    }

    private IEnumerator DisableGunShot(GameObject gunShot)
    {
        yield return new WaitForSeconds(.5f);
        gunShot.SetActive(false);
    }
}
