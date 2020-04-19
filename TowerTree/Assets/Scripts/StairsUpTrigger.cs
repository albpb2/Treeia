using UnityEngine;

public class StairsUpTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.Player)
        {
            LevelManager.Instance.CompleteLevel();
        }
    }
}
