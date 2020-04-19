using UnityEngine;

public class StairsDown : MonoBehaviour
{
    [SerializeField] 
    private GameObject _stairsDoor;

    public void CloseStairs()
    {
        _stairsDoor.SetActive(true);
    }
}
