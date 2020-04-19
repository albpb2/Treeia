using System;
using UnityEngine;

public class StairsDownTrigger : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.Player)
        {
            var stairs = transform.parent.GetComponent<StairsDown>();
            stairs.CloseStairs();
        }
    }
}
