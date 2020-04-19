using UnityEngine;

public class StairsUp : MonoBehaviour
{
    [SerializeField] 
    private GameObject _stairsDoor;

    public void OpenStairs()
    {
        _stairsDoor.SetActive(false);
    }
}
