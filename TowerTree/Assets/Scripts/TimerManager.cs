using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject _timerBarPrefab;

    private LevelManager _levelManager;
    private GameObject _timerBackground;
    private GameObject _timerBorder;
    private GameObject[] _subTimerBackgrounds;
    private GameObject[] _timerBars;
    private Image _currentSubTimer;
    private TimerStates _currentSubTimerState;
    private int _currentSubTimerIndex;
    private int _targetWaterCount;
    private float _timerBackgroundScaleX;
    private float _timerBackgroundWidth;
    private float _leftTimerBackgroundX;
    private float _timerSeconds;
    private float _remainingTime;
    private bool _started;

    void Awake()
    {
        _timerBackground = GameObject.Find("TimerBackground");
        _timerBorder = GameObject.Find("TimerBorder");
        _levelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;
        _timerBackgroundScaleX = _timerBackground.transform.localScale.x;
        _timerBackgroundWidth = _timerBackground.GetComponent<RectTransform>().rect.width * _timerBackgroundScaleX;
        _leftTimerBackgroundX = _timerBackground.transform.position.x - (_timerBackgroundWidth / 2);
    }

    private void Update()
    {
        if (_started)
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime < 0) _remainingTime = 0;
            _currentSubTimer.fillAmount = _remainingTime / _timerSeconds;
            const float remainingSecondsWarning = 0.4f;
            const float remainingSecondsDanger = 0.2f;
            if (_currentSubTimerState == TimerStates.Normal && _currentSubTimer.fillAmount <= remainingSecondsWarning)
            {
                _currentSubTimerState = TimerStates.Warning;
                SetSubTimersColor(Color.yellow);
            }
            else if (_currentSubTimerState == TimerStates.Warning && _currentSubTimer.fillAmount <= remainingSecondsDanger)
            {
                _currentSubTimerState = TimerStates.Danger;
                SetSubTimersColor(Color.red);
            }
        }
    }

    public void SetLevelTimer()
    {
        CleanUp();
        _subTimerBackgrounds = new GameObject[_levelManager.TargetWaterCount];
        _timerBars = new GameObject[_levelManager.TargetWaterCount];
        
        var subtimerScaleX = _timerBackgroundScaleX / _levelManager.TargetWaterCount;
        var subtimerRectWidth = _timerBackgroundWidth / _levelManager.TargetWaterCount;
        for (var i = 0; i < _levelManager.TargetWaterCount; i++)
        {
            var subTimerBackground = Instantiate(_timerBackground, _timerBackground.transform.parent);
            subTimerBackground.transform.localScale = new Vector3(
                subtimerScaleX, 
                _timerBackground.transform.localScale.y, 
                1);
            subTimerBackground.transform.position = new Vector3(
                _leftTimerBackgroundX + (subtimerRectWidth * (i + 0.5f)),
                _timerBackground.transform.position.y,
                _timerBackground.transform.position.z);
            _subTimerBackgrounds[i] = subTimerBackground;

            var timerBar = Instantiate(_timerBarPrefab, _timerBackground.transform.parent);
            timerBar.transform.position = new Vector3(
                _leftTimerBackgroundX + (subtimerRectWidth * i),
                _timerBackground.transform.position.y,
                _timerBackground.transform.position.z);
            _timerBars[i] = timerBar;
        }
        
        _timerBorder.transform.SetSiblingIndex(_timerBorder.transform.GetSiblingIndex() + _levelManager.TargetWaterCount * 2);
        _timerBackground.SetActive(false);
    }

    public void StartTimer()
    {
        _currentSubTimerIndex = _subTimerBackgrounds.Length - 1;
        _currentSubTimer = _subTimerBackgrounds[_currentSubTimerIndex].GetComponent<Image>();
        _timerSeconds = _levelManager.SecondsPerWater;
        _remainingTime = _timerSeconds;
        _currentSubTimerState = TimerStates.Normal;
        SetSubTimersColor(Color.white);
        _started = true;
    }

    private void CleanUp()
    {
        if (_subTimerBackgrounds != null)
        {
            for (var i = 0; i < _subTimerBackgrounds.Length; i++)
            {
                Destroy(_subTimerBackgrounds[i].gameObject);
            }
        }
        
        if (_timerBars != null)
        {
            for (var i = 0; i < _timerBars.Length; i++)
            {
                Destroy(_timerBars[i].gameObject);
            }
        }
    }

    private void SetSubTimersColor(Color color)
    {
        for (var i = 0; i < _subTimerBackgrounds.Length; i++)
        {
            _subTimerBackgrounds[i].GetComponent<Image>().color = color;
        }
    }
}
