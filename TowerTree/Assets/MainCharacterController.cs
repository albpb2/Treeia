using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] 
    private float _movementSpeed;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _movement;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        _rigidbody.MovePosition(_rigidbody.position + _movement * (_movementSpeed * Time.fixedDeltaTime));
    }
}
