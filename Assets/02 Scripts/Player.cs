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
    public float rotationSpeed = 100.0f; // Начальная скорость вращения в градусах в секунду
    private float currentRotationSpeed; // Текущая скорость вращения

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentRotationSpeed = 0f; // Начальная скорость вращения равна 0
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
                rb.velocity += Vector3.up * acceleration * Time.deltaTime;
            }
            currentRotationSpeed = rotationSpeed; // Установить скорость вращения на максимум
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity += Vector3.down * acceleration * Time.deltaTime;
            currentRotationSpeed = rotationSpeed; // Установить скорость вращения на максимум
        }
        else
        {
            // Плавно уменьшаем скорость вращения до 0
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 5f * Time.deltaTime);
        }
    }

    private void HandleLateralMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            lateralVelocity += Vector3.right * lateralAcceleration * Time.deltaTime;
            currentRotationSpeed = rotationSpeed;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lateralVelocity += Vector3.left * lateralAcceleration * Time.deltaTime;
            currentRotationSpeed = rotationSpeed;
            isMoving = true;
        }

        if (!isMoving)
        {
            // Плавно уменьшаем скорость вращения до 0
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 5f * Time.deltaTime);
        }

        lateralVelocity = Vector3.ClampMagnitude(lateralVelocity, maxSpeed);
        rb.velocity = new Vector3(lateralVelocity.x, rb.velocity.y, 0);
    }

    void RotatePropeller()
    {
        if (propeller != null && currentRotationSpeed > 0.01f) // Проверка, что скорость не очень маленькая
        {
            propeller.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
        }
    }
}