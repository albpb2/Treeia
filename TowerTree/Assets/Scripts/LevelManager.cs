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
        "Scenes/Level3",
        "Scenes/Level4",
        "Scenes/Level5",
        "Scenes/Level6",
    };

    private int[] _secondsPerPuddlePerLevel =
    {
        15,
        15,
        20,
        14,
        17,
        20
    };

    private int[] _puddlesCountPerLevel =
    {
        3,
        2,
        4,
        4,
        5,
        6
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
            const int pointForWinning = 50;
            GameManager.Instance.AddPoints(pointForWinning);
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
        UiManager.Instance.SetLevelNumberText(_currentLevel + 1);
        StartCoroutine(StartLevel());
        SoundManager.Instance.PlayMainTrack();
    }

    private IEnumerator StartLevel()
    {
        TimerManager.Instance.Reset();
        yield return new WaitForSeconds(.5f);
        var timerManager = FindObjectsOfType<TimerManager>();
        timerManager[0].SetLevelTimer(_puddlesCountPerLevel[_currentLevel]);
        timerManager[0].StartTimer(_secondsPerPuddlePerLevel[_currentLevel]);
    }
}
