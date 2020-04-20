using System;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private int _timesWatered;

    public delegate void TreeWateredHandler();
    public event TreeWateredHandler TreeWatered;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Water()
    {
        _timesWatered++;
        Debug.Log($"Watered {_timesWatered} times");
        TreeWatered?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.Player)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.Player)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        }
    }
}
