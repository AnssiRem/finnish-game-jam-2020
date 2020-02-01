using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private bool isFiring;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private KeyCode fireKey = KeyCode.LeftControl;
    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField] private float jumpAcceleration = 350f;
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

        if (Input.GetKeyDown(jumpKey))
        {
            SetFiring(false);
            Jump();
        }
        else if (Input.GetKeyDown(fireKey))
        {
            SetFiring(true);
        }
        else if (Input.GetKeyUp(fireKey))
        {
            SetFiring(false);
        }

        Fire(isFiring);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void SetFiring(bool state)
    {
        isFiring = state;
        gun.SetActive(state);
        animator.SetBool("HoldingGun", state);
    }

    private void Fire(bool isFiring)
    {
        if (isFiring)
        {
            Vector3 dir = transform.forward * 2f - transform.up * 1f;
            Ray ray = new Ray(transform.localPosition + transform.up * 2.5f, dir.normalized);
            RaycastHit[] hits = Physics.RaycastAll(ray, 10f);
            Debug.DrawLine(transform.localPosition + transform.up * 2.5f,
                transform.localPosition + dir.normalized * 10f, Color.blue, 0.1f);
            foreach (RaycastHit hit in hits)
            {
                Debug.DrawLine(hit.point + Vector3.up * 0.1f, hit.point - Vector3.up * 0.1f, Color.red, 0.1f);
                Debug.DrawLine(hit.point + Vector3.right * 0.1f, hit.point - Vector3.right * 0.1f, Color.red, 0.1f);
                Debug.DrawLine(hit.point + Vector3.forward * 0.1f, hit.point - Vector3.forward * 0.1f, Color.red, 0.1f);

                if (hit.collider.tag == "Hole")
                {
                    hit.transform.GetComponent<Hole>().Fill();
                }
            }
        }
    }

    private void Jump()
    {
        bool isGrounded = false;

        Ray ray = new Ray(transform.localPosition + transform.up * 1f, -transform.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 2f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.layer != 12)
            {
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            JumpProceed();
        }
    }

    private void JumpProceed()
    {
        animator.SetTrigger("Jump");
        rb.AddForce(transform.up * jumpAcceleration);
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
