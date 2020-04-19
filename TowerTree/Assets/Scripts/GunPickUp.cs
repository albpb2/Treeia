using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    [SerializeField] 
    private Gun _gun;

    public Gun Gun => _gun;
    
    void Awake()
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = _gun.sprite;
    }
}