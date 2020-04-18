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

    private Dictionary<string, GameObject> _spritesPerKey;
    private string[] _spriteKeys;
    private string _shootingDirection;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(_shootingDirection))
        {
            if (Math.Abs(Input.GetAxis(_shootingDirection)) < 0.1)
            {
                _spritesPerKey[_shootingDirection].gameObject.SetActive(false);
                _shootingDirection = null;
            }
        }

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
    }
}
