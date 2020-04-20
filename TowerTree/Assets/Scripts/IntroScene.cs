using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] _panels;

    private int _currentPanelCount;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentPanelCount < _panels.Length - 1)
            {
                MoveToNextPanel();
            }
            else
            {
                StartGame();
            }
        }
    }

    public void MoveToNextPanel()
    {
        _panels[_currentPanelCount].SetActive(false);
        _currentPanelCount++;
        _panels[_currentPanelCount].SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PreLoadScene");
    }
}
