using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float acceleration = 1f;
    public float lateralAcceleration = 5f;

    private Rigidbody rb;
    private Vector3 lateralVelocity;

    public GameObject propeller; // Объект пропеллера для вращения
    public float rotationSpeed = 100.0f; // Скорость вращения в градусах в секунду

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleLateralMovement();
        HandleVerticalMovement();
    }

    private void HandleVerticalMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (rb.velocity.y < maxSpeed)
            {
                rb.velocity += Vector3.up * acceleration * Time.deltaTime;
            }
            RotatePropeller();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity += Vector3.down * acceleration * Time.deltaTime;
            RotatePropeller();
        }
    }

    private void HandleLateralMovement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            lateralVelocity += Vector3.right * lateralAcceleration * Time.deltaTime;
            RotatePropeller();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            lateralVelocity += Vector3.left * lateralAcceleration * Time.deltaTime;
            RotatePropeller();
        }

        lateralVelocity = Vector3.ClampMagnitude(lateralVelocity, maxSpeed);
        rb.velocity = new Vector3(lateralVelocity.x, rb.velocity.y, 0);
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.x > 0.5)  // Столкновение с левой стороны
            {
                if (lateralVelocity.x < 0) lateralVelocity.x = 0;
            }
            else if (contact.normal.x < -0.5)  // Столкновение с правой стороны
            {
                if (lateralVelocity.x > 0) lateralVelocity.x = 0;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            lateralVelocity = Vector3.zero;
        }
    }

    void RotatePropeller()
    {
        if (propeller != null)
        {
            propeller.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // Вращение вокруг оси Z
        }
    }
}
