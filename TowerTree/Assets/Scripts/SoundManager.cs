using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] 
    private AudioSource _audioSource;
    [SerializeField] 
    private AudioClip _mainTrack;

    public void PlayMainTrack()
    {
        _audioSource.clip = _mainTrack;
        _audioSource.Play();
    }
}