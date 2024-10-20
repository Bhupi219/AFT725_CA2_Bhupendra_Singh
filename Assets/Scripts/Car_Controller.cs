using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
