using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 1f;
    public float lateralAcceleration = 5f;

    private Rigidbody2D rb;
    private Vector2 lateralVelocity;

    public GameObject propeller;
    public float rotationSpeed = 100.0f;
    private float currentRotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentRotationSpeed = 0f;
    }

    void Update()
    {
        HandleLateralMovement();
        HandleVerticalMovement();
        RotatePropeller();
    }

    private void HandleVerticalMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (rb.velocity.y < maxSpeed)
            {
                rb.velocity += Vector2.up * acceleration * Time.deltaTime;
            }
            currentRotationSpeed = rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity += Vector2.down * acceleration * Time.deltaTime;
            currentRotationSpeed = rotationSpeed;
        }
        else
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 5f * Time.deltaTime);
        }
    }

    private void HandleLateralMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lateralVelocity += Vector2.right * lateralAcceleration * Time.deltaTime;
            currentRotationSpeed = rotationSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lateralVelocity += Vector2.left * lateralAcceleration * Time.deltaTime;
            currentRotationSpeed = rotationSpeed;
            isMoving = true;
        }

        if (!isMoving)
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 5f * Time.deltaTime);
        }

        lateralVelocity = Vector2.ClampMagnitude(lateralVelocity, maxSpeed);
        rb.velocity = new Vector2(lateralVelocity.x, rb.velocity.y);
    }

    void RotatePropeller()
    {
        if (propeller != null && currentRotationSpeed > 0.01f)
        {
            propeller.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
        }
    }
}
