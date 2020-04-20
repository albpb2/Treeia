using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] 
    private int _pointsPerRemainingSecond = 10;
    
    private int _currentLevel = -1;

    private string[] _levelSceneNames =
    {
        "Scenes/Level1",
        "Scenes/Level2",
    };

    private int[] _secondsPerPuddlePerLevel =
    {
        15,
        20
    };

    private int[] _puddlesCountPerLevel =
    {
        3,
        2
    };

    public int CurrentLevel => _currentLevel;

    public void SetUpLevelCompletion()
    {
        var stairs = GameObject.FindGameObjectWithTag(Tags.StairsUp).GetComponent<StairsUp>();
        stairs.OpenStairs();
    }
    
    public void CompleteLevel()
    {
        var timerManager = GameObject.FindGameObjectWithTag(Tags.TimerManager).GetComponent<TimerManager>();
        GameManager.Instance.AddPoints((int) (timerManager.RemainingTime * _pointsPerRemainingSecond));
        
        Debug.Log("Level completed");
        if (_currentLevel < _levelSceneNames.Length - 1)
        {
            StartNextLevel();
        }
        else
        {
            GameManager.Instance.Win();
        }
    }

    public void FailLevel()
    {
        GameManager.Instance.Lose();
    }

    public void StartNextLevel()
    {
        _currentLevel++;
        SceneManager.LoadScene(_levelSceneNames[_currentLevel]);
        StartCoroutine(StartLevel());
        SoundManager.Instance.PlayMainTrack();
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2);
        var timerManager = FindObjectsOfType<TimerManager>();
        timerManager[0].SetLevelTimer(_puddlesCountPerLevel[_currentLevel]);
        timerManager[0].StartTimer(_secondsPerPuddlePerLevel[_currentLevel]);
    }
}
