using System;
using UnityEngine;

public class EnemyGraphics : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Player>();
    }

    void Update()
    {
        var targetDirection = _player.transform.position - transform.position;
        var absX = Math.Abs(targetDirection.x);
        var absY = Math.Abs(targetDirection.y);
        if (absX > absY)
        {
            if (targetDirection.x <= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // left
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 180); // left
            }
        }
        else
        {
            if (targetDirection.y <= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90); // down
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 270); // up
            }
        }
    }
}
