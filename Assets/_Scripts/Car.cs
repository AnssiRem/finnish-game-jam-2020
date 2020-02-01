using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float wheelDriveForce = 1f;
    [SerializeField] private float startingSpeed = 0f;

    public float WheelDrivingForce { get => wheelDriveForce; set => wheelDriveForce = value; }

    void Start()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            // Right is forward because...
            rb.velocity = transform.right * startingSpeed;
        }
    }
}
