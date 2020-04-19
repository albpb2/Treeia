using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private float _healthPoints = 5;
    [SerializeField] 
    private float _hitPoints;
    [SerializeField] 
    private Collider2D _hitZoneCollider;
    [SerializeField] 
    private int valuePoints = 5;

    private Player _player;
    private float _lastAttackTime = 0;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastAttackTime > 3)
        {
            _animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
        }
    }

    public void Hurt(float damage)
    {
        _healthPoints -= damage;
        if (_healthPoints <= 0)
        {
            Die();
        }
    }

    public void HitPlayer()
    {
        if (_hitZoneCollider.IsTouching(_player.GetComponent<Collider2D>()))
        {
            _player.Hurt(_hitPoints);
        }
    }

    private void Die()
    {
        GameManager.Instance.AddPoints(valuePoints);
        Destroy(gameObject);
    }
}
