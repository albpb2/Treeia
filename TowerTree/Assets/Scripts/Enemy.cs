using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private float _lastAttackTime = 0;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player);
        _animator = GetComponent<Animator>();
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
}
