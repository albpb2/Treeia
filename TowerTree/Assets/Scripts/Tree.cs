using UnityEngine;

public class Tree : MonoBehaviour
{
    private int _timesWatered;

    public delegate void TreeWateredHandler();
    public event TreeWateredHandler TreeWatered;
    
    public void Water()
    {
        _timesWatered++;
        Debug.Log($"Watered {_timesWatered} times");
        TreeWatered?.Invoke();
    }
}
