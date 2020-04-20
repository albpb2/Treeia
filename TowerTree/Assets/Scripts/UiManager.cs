using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] 
    private Text _levelNumberText;

    public void SetLevelNumberText(int levelNumber)
    {
        _levelNumberText.text = $"Floor {levelNumber}";
    }
}
