using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(HingeJoint))]
public class CarWheel : MonoBehaviour
{
    private float driveForce;
    private float radius;
    private Rigidbody rb;
    private Transform car;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        car = transform.parent;

        driveForce = car.GetComponent<Car>().WheelDrivingForce;

        Vector3 extents = GetComponent<Collider>().bounds.extents;
        radius = Mathf.Max(extents.x, extents.y, extents.z);
    }

    private void FixedUpdate()
    {
        DriveWheel(Time.fixedDeltaTime * driveForce);
    }

    private void DriveWheel(float amount)
    {
        Vector3 force = car.right * amount;
        Vector3 forcePos = transform.position + car.up * radius;

        rb.AddForceAtPosition(force, forcePos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + car.up * radius, transform.position + Vector3.up * radius + car.right * driveForce);
    }
}
