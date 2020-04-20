using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public bool automatic;
    public float shootFrequencySeconds;
    public float damage;
    public int initialBulletsCount;
    public bool infiniteBullets;
    public Sprite sprite;
    public AudioClip soundClip;
}