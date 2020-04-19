using UnityEngine;

public class PointsPickUp : MonoBehaviour
{
    [SerializeField] 
    private int _points;

    public int Points => _points;
}