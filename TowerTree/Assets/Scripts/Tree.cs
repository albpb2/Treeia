using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private int _timesWatered;
    
    public void Water()
    {
        _timesWatered++;
        Debug.Log($"Watered {_timesWatered} times");
    }
}
