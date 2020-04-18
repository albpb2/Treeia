﻿using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private LayerMask _interactiveLayerMask;

    private Player _player;
    private Collider2D _playerCollider;
    private bool _actionButtonDown;

    private void Start()
    {
        _player = transform.parent.GetComponent<Player>();
        _playerCollider = _player.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_actionButtonDown)
        {
            if (Input.GetAxisRaw(VirtualInputKeyNames.Action) <= 0)
            {
                _actionButtonDown = false;
            }
        }
        else if (Input.GetAxisRaw(VirtualInputKeyNames.Action) > 0)
        {
            _actionButtonDown = true;
            Interact();
        }
    }

    private void Interact()
    {
        var collider = Physics2D.OverlapCircle(_playerCollider.transform.position, 2, _interactiveLayerMask);
        if (collider != null)
        {
            if (collider.tag == Tags.Puddle)
            {
                var puddle = collider.gameObject;
                puddle.SetActive(false);
                _player.PickWater();
            }
        }
    }
}