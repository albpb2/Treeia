using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private int _currentLevel = -1;
    
    public int TargetWaterCount { get; set; }

    public float SecondsPerWater { get; set; }

    public int CurrentLevel => _currentLevel;

    public void SetUpLevelCompletion()
    {
        var stairs = GameObject.FindGameObjectWithTag(Tags.StairsUp).GetComponent<StairsUp>();
        stairs.OpenStairs();
    }
    
    public void CompleteLevel()
    {
        Debug.Log("Level completed");
    }

    public void FailLevel()
    {
        
    }

    public void StartNextLevel()
    {
        _currentLevel++;
        SceneManager.LoadScene("Scenes/Level1");
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2);
        TimerManager.Instance.SetLevelTimer();
        TimerManager.Instance.StartTimer();
    }
}
