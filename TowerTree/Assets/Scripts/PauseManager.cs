using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] 
    private GameObject _pausePanel;

    private float _timeScale;
    private bool _paused;
    private bool _pauseButtonDown;

    public bool Paused => _paused;

    void Update()
    {
        if (!_pauseButtonDown && Input.GetAxisRaw(VirtualInputKeyNames.Pause) > 0)
        {
            _pauseButtonDown = true;
            if (_paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
        else if (_pauseButtonDown && Input.GetAxisRaw(VirtualInputKeyNames.Pause) == 0)
        {
            _pauseButtonDown = false;
        }
    }

    public void Unpause()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = _timeScale;
        _paused = false;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = _timeScale;
        SceneManager.LoadScene("MainMenu");
    }

    private void Pause()
    {
        _paused = true;
        _pausePanel.SetActive(true);
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
    }
}
