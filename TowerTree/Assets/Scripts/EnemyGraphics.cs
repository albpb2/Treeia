using System;
using UnityEngine;

public class EnemyGraphics : MonoBehaviour
{
    private Player _player;
    private Transform _parentTransform;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Player>();
        _parentTransform = gameObject.transform.parent;
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
                _parentTransform.rotation = Quaternion.Euler(0, 0, 0); // left
            }
            else
            {
                _parentTransform.rotation = Quaternion.Euler(0, 0, 180); // left
            }
        }
        else
        {
            if (targetDirection.y <= 0)
            {
                _parentTransform.rotation = Quaternion.Euler(0, 0, 90); // down
            }
            else
            {
                _parentTransform.rotation = Quaternion.Euler(0, 0, 270); // up
            }
        }
    }
}
