﻿using UnityEngine;
using UnityEngine.UI;

public class TimerManager : Singleton<TimerManager>
{
    [SerializeField] 
    private GameObject _timerBarPrefab;

    private GameObject _timerBackground;
    private GameObject _timerBorder;
    private GameObject[] _subTimerBackgrounds;
    private GameObject[] _timerBars;
    private Image _currentSubTimer;
    private Tree _tree;
    private TimerStates _currentSubTimerState;
    private int _currentSubTimerIndex;
    private int _targetWaterCount;
    private float _timerBackgroundScaleX;
    private float _timerBackgroundWidth;
    private float _leftTimerBackgroundX;
    private float _timerSeconds;
    private float _remainingTime;
    private bool _started;
    private int _completedMilestones;

    public float RemainingTime => _remainingTime;

    protected override void Awake()
    {
        base.Awake();
        
        _timerBackground = GameObject.Find("TimerBackground");
        _timerBorder = GameObject.Find("TimerBorder");
    }

    void Update()
    {
        if (_started)
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime < 0) _remainingTime = 0;
            _currentSubTimer.fillAmount = _remainingTime / _timerSeconds;

            if (_remainingTime > 0 && (!IsCurrentMilestoneAchieved() || _currentSubTimerIndex == 0))
            {
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
            else if (_remainingTime <= 0)
            {
                if (IsCurrentMilestoneAchieved() &&  _currentSubTimerIndex != 0)
                {
                    MoveToNextTimer();
                }
                else
                {
                    StopTimer();
                    LevelManager.Instance.FailLevel();
                }
            }
        }
    }

    public void SetLevelTimer(int puddlesCount)
    {
        _tree = FindObjectOfType<Tree>();
        _tree.TreeWatered += CompleteMilestone;
        
        _timerBackgroundScaleX = _timerBackground.transform.localScale.x;
        _timerBackgroundWidth = _timerBackground.GetComponent<RectTransform>().rect.width * _timerBackgroundScaleX;
        _leftTimerBackgroundX = _timerBackground.transform.position.x - (_timerBackgroundWidth / 2);
        
        _targetWaterCount = puddlesCount;
        _subTimerBackgrounds = new GameObject[_targetWaterCount];
        _timerBars = new GameObject[_targetWaterCount];
        
        var subtimerScaleX = _timerBackgroundScaleX / _targetWaterCount;
        var subtimerRectWidth = _timerBackgroundWidth / _targetWaterCount;
        for (var i = 0; i < _targetWaterCount; i++)
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
        
        _timerBorder.transform.SetSiblingIndex(_timerBorder.transform.GetSiblingIndex() + _targetWaterCount * 2);
        _timerBackground.SetActive(false);
    }

    public void StartTimer(float secondsPerPuddle)
    {
        _currentSubTimerIndex = _subTimerBackgrounds.Length - 1;
        _currentSubTimer = _subTimerBackgrounds[_currentSubTimerIndex].GetComponent<Image>();
        _timerSeconds = secondsPerPuddle;
        _remainingTime = _timerSeconds;
        _currentSubTimerState = TimerStates.Normal;
        SetSubTimersColor(Color.green);
        _completedMilestones = 0;
        _started = true;
    }

    public void Reset()
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
        
        _timerBackground.SetActive(true);
    }

    public void CompleteMilestone()
    {
        _completedMilestones++;
        SetSubTimersColor(Color.green);
        _currentSubTimerState = TimerStates.Normal;

        if (_completedMilestones == _targetWaterCount)
        {
            LevelManager.Instance.SetUpLevelCompletion();
        }
        else
        {
            _timerBars[_timerBars.Length - _completedMilestones].SetActive(false);
        }
    }

    public void StopTimer()
    {
        _started = false;
    }

    private void SetSubTimersColor(Color color)
    {
        for (var i = 0; i < _subTimerBackgrounds.Length; i++)
        {
            _subTimerBackgrounds[i].GetComponent<Image>().color = color;
        }
    }

    private bool IsCurrentMilestoneAchieved()
    {
        return _completedMilestones > ((_targetWaterCount - 1) - _currentSubTimerIndex);
    }

    private void MoveToNextTimer()
    {
        _currentSubTimerIndex--;
        if (_currentSubTimerIndex >= 0)
        {
            _currentSubTimer = _subTimerBackgrounds[_currentSubTimerIndex].GetComponent<Image>();
            _remainingTime = _timerSeconds;
            _currentSubTimerState = TimerStates.Normal;
        }
    }
}
