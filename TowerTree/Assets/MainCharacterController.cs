using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField] 
    private float _movementSpeed;
    
    private Rigidbody2D _rigidbody;
    
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
        var currentPosition = transform.position;
        var yDelta = 0f;
        var xDelta = 0f;
        
        if (verticalInput > 0)
        {
            yDelta = _movementSpeed;
        }
        else if (verticalInput < 0)
        {
            yDelta = -_movementSpeed;
        }

        if (horizontalInput > 0)
        {
            xDelta = _movementSpeed;
        }
        else if (horizontalInput < 0)
        {
            xDelta = -_movementSpeed;
        }
        
        _rigidbody.MovePosition(new Vector2(currentPosition.x + xDelta, currentPosition.y + yDelta));
    }
}
