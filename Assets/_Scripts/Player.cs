using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private KeyCode fireKey = KeyCode.Space;

    [SerializeField] private float moveAcceleration = 1000f;
    [SerializeField] private float moveMaxSpeed = 10f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float runTreshold = 1f;

    [Header("References")]

    [SerializeField] GameObject gun = null;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDirection = GetMoveDirection();

        if (Input.GetKeyDown(fireKey))
        {
            Fire(true);
        }
        else if (Input.GetKeyUp(fireKey))
        {
            Fire(false);
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void Fire(bool state)
    {
        gun.SetActive(state);
        animator.SetBool("HoldingGun", state);

        // TODO: Apply further tursotin logic here
    }


    private void MoveCharacter()
    {
        float horizontalVel = Vector3.ProjectOnPlane(rb.velocity, Vector3.up).magnitude;

        // Animate character
        if (horizontalVel > runTreshold)
        {
            animator.SetBool("Running", true);
        }
        else if (moveDirection == Vector3.zero)
        {
            animator.SetBool("Running", false);
        }

        // Move character
        if (horizontalVel < moveMaxSpeed)
        {
            rb.AddForce(moveDirection * moveAcceleration * Time.fixedDeltaTime);
        }

        // Turn character
        if (moveDirection != Vector3.zero)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
                Quaternion.LookRotation(moveDirection), turnSpeed * Time.fixedDeltaTime);
        }
    }

    private Vector3 GetMoveDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = x * Camera.main.transform.right + y *
            Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

        direction.Normalize();

        return direction;
    }
}
