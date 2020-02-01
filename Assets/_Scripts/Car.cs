using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float prevVelocity;
    private Rigidbody rb = null;

    [SerializeField] private float leathalStop = 5f;
    [SerializeField] private float wheelDriveForce = 1f;
    [SerializeField] private float startingSpeed = 0f;

    public float WheelDrivingForce { get => wheelDriveForce; set => wheelDriveForce = value; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = transform.right * startingSpeed;
        }

        prevVelocity = rb.velocity.magnitude;
    }

    private void FixedUpdate()
    {
        if(prevVelocity - rb.velocity.magnitude > leathalStop)
        {
            // Die or something
        }
    }
}
