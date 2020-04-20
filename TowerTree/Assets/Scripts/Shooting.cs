using UnityEngine;

public static class Shooting
{
    public static void Shoot(Vector2 sourcePosition, Vector2 direction, GameObject bullet, float damage)
    {
        var hits = Physics2D.RaycastAll(sourcePosition, direction);
        if (hits != null && hits.Length > 0)
        {
            for (var i = 0; i < hits.Length; i++)
            {
                if (!hits[i].collider.isTrigger)
                {
                    bullet.transform.position = hits[i].point;
                    bullet.SetActive(true);

                    if (hits[i].transform.tag == Tags.Enemy)
                    {
                        var enemy = hits[i].transform.GetComponent<Enemy>();
                        enemy.Hurt(damage);
                    }
                    else if (hits[i].transform.tag == Tags.Player)
                    {
                        var player = hits[i].transform.GetComponent<Player>();
                        player.Hurt(damage);
                    }

                    break;
                }
            }
        }
    }
}