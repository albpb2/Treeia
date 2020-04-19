using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TimerManager _timerManager;
    private LevelManager _levelManager;

    private void Awake()
    {
        _timerManager = FindObjectOfType(typeof(TimerManager)) as TimerManager;
        _levelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelManager.TargetWaterCount = 3;
        _levelManager.SecondsPerWater = 20;
        _timerManager.SetLevelTimer();
        _timerManager.StartTimer();
    }
}
