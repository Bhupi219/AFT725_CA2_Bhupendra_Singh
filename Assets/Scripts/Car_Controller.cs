using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car_Controller : MonoBehaviour
{
    private float accelerationInput;
    private float currentTurnInput;
    private float targetTurnInput;
    public float maxTurnAngle = 25f;
    public float handBrake = 5000f;
    private Vector3 currentVelocity;
    private bool isHandBrakePressed = false;

    public Rigidbody carBody;
    public float carHorsePower = 400f;

    [Header("Wheel Colliders")]
    public WheelCollider wc_FrontLeft;
    public WheelCollider wc_FrontRight;
    public WheelCollider wc_BackLeft;
    public WheelCollider wc_BackRight;

    void Start()
    {

    }

    void Update()
    {
        currentVelocity = carBody.velocity;

        accelerationInput = Input.GetAxis("Vertical");

        targetTurnInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isHandBrakePressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isHandBrakePressed = false;
        }

    }

    private void FixedUpdate()
    {
        Vector3 combinedInput = transform.forward * accelerationInput;
        float Dotproduct = Vector3.Dot(currentVelocity.normalized, combinedInput);

        if (Dotproduct < 0)
        {
            wc_BackLeft.motorTorque = 0;
            wc_BackRight.motorTorque = 0;
            wc_FrontLeft.motorTorque = 0;
            wc_FrontRight.motorTorque = 0;


            wc_FrontLeft.brakeTorque = 1000f;
            wc_FrontRight.brakeTorque = 1000f;
            wc_BackLeft.brakeTorque = 0;
            wc_BackRight.brakeTorque = 0;
        }
        else if (Dotproduct > 0)
        {
            wc_FrontRight.brakeTorque = 0;
            wc_FrontLeft.brakeTorque = 0;
            wc_BackLeft.brakeTorque = 0;
            wc_BackRight.brakeTorque = 0;

            wc_BackRight.motorTorque = accelerationInput * carHorsePower;
            wc_BackLeft.motorTorque = accelerationInput * carHorsePower;
        }
        if (isHandBrakePressed)
        {
            wc_BackLeft.brakeTorque = handBrake;
            wc_BackRight.brakeTorque = handBrake;

            // Disable motor torque while handbrake is applied
            wc_BackLeft.motorTorque = 0;
            wc_BackRight.motorTorque = 0;
        }
        else if (!isHandBrakePressed && Dotproduct > 0) // Reset brake torque when handbrake is released
        {
            wc_BackLeft.brakeTorque = 0;
            wc_BackRight.brakeTorque = 0;
        }



        // Pure Debugging
        string KeyPressed;
        if (accelerationInput > 0)
        {
            KeyPressed = "W";
        }
        else if (accelerationInput < 0)
        {
            KeyPressed = "S";
        }
        else
        {
            KeyPressed = "No Key Pressed";
        }
        Debug.Log("Input = " + KeyPressed + " ||| Velocity = " + currentVelocity.normalized + "||| Dot Product = " + Dotproduct);
        // Pure Debugging

        currentTurnInput = ApproachTargetValueWithIncrement(currentTurnInput, targetTurnInput, 0.07f);
        wc_FrontLeft.steerAngle = currentTurnInput * maxTurnAngle;
        wc_FrontRight.steerAngle = currentTurnInput * maxTurnAngle;
    }

    private float ApproachTargetValueWithIncrement(float currentValue, float targetValue, float increment)
    {
        if (currentValue == targetValue)
        {
            return currentValue;
        }

        else
        {
            if (currentValue < targetValue)
            {
                currentValue = currentValue + increment;
            }

            else
            {
                currentValue = currentValue - increment;
            }

        }
        return currentValue;

    }
}
