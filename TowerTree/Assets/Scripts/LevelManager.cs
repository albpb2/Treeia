using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    private int _currentLevel = -1;

    private string[] _levelSceneNames = new[]
    {
        "Scenes/Level1",
        "Scenes/Level2",
    };
    
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
        if (_currentLevel < _levelSceneNames.Length - 1)
        {
            StartNextLevel();
        }
        else
        {
            Debug.Log("Game finished!");
        }
    }

    public void FailLevel()
    {
        
    }

    public void StartNextLevel()
    {
        _currentLevel++;
        SceneManager.LoadScene(_levelSceneNames[_currentLevel]);
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2);
        var timerManager = FindObjectsOfType<TimerManager>();
        timerManager[0].SetLevelTimer();
        timerManager[0].StartTimer();
    }
}
