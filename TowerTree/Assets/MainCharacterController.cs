using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] 
    private float _movementSpeed;
    
    private Player _player;
    private Rigidbody2D _rigidbody;
    private Vector2 _movement;
    private bool _sprinting;
    private bool _sprintingOnThisFrame;
    
    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _player = GetComponentInParent<Player>();
    }

    void Update()
    {
        var sprintInput = Input.GetAxisRaw("Sprint");
        if (_sprinting && sprintInput == 0)
        {
            _sprinting = false;
        }
        else if (!_sprinting && sprintInput > 0 && _player.SpendSprintStamina())
        {
            Debug.Log("Sprinting");
            _sprinting = true;
            _sprintingOnThisFrame = true;
        }
        
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
        
        if (verticalInput > 0)
        {
            _movement.y = 1;
        }
        else if (verticalInput < 0)
        {
            _movement.y = -1;
        }
        else
        {
            _movement.y = 0;
        }

        if (horizontalInput > 0)
        {
            _movement.x = 1;
        }
        else if (horizontalInput < 0)
        {
            _movement.x = -1;
        }
        else
        {
            _movement.x = 0;
        }
    }
    
    private void FixedUpdate()
    {
        var speed = _sprintingOnThisFrame ? _movementSpeed * 10 : _movementSpeed;
        _rigidbody.MovePosition(_rigidbody.position + _movement * (speed * Time.fixedDeltaTime));
        _sprintingOnThisFrame = false;
    }
}
