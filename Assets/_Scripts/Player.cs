using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerGunAudio))]
public class Player : MonoBehaviour
{
    private bool isFiring;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private KeyCode fireKey = KeyCode.LeftControl;
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode resetKey = KeyCode.R;

    [SerializeField] private float jumpAcceleration = 350f;
    [SerializeField] private float moveAcceleration = 1000f;
    [SerializeField] private float moveMaxSpeed = 10f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float runTreshold = 1f;

    [Header("References")]

    [SerializeField] GameObject gun = null;
    [SerializeField] ParticleSystem dust = null;

    //Audio
    private PlayerGunAudio playerAudio;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<PlayerGunAudio>();
    }

    private void Update()
    {
        moveDirection = GetMoveDirection();

        if (Input.GetKeyDown(jumpKey))
        {
            playerAudio.StopShootSFX();
            Jump();
        }
        else if (Input.GetKeyDown(fireKey))
        {
            SetFiring(true);
            playerAudio.StartShootSFX();
        }
        else if (Input.GetKeyUp(fireKey))
        {
            SetFiring(false);
            playerAudio.StopShootSFX();
        }
        else if (Input.GetKeyDown(resetKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Fire(isFiring);
        Land();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void SetFiring(bool state)
    {
        isFiring = state;
        gun.GetComponentInChildren<MeshRenderer>().enabled = state;

        if (state)
        {
            gun.GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            gun.GetComponentInChildren<ParticleSystem>().Stop();
        }

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
                transform.localPosition + transform.up * 2.5f + dir.normalized * 10f, Color.blue, 0.1f);
            foreach (RaycastHit hit in hits)
            {
                Color color = Color.red;

                if (hit.collider.tag == "Hole")
                {
                    hit.transform.GetComponent<Hole>().Fill();

                    color = Color.green;
                }

                Debug.DrawLine(hit.point + Vector3.up * 0.1f, hit.point - Vector3.up * 0.1f, color, 0.1f);
                Debug.DrawLine(hit.point + Vector3.right * 0.1f, hit.point - Vector3.right * 0.1f, color, 0.1f);
                Debug.DrawLine(hit.point + Vector3.forward * 0.1f, hit.point - Vector3.forward * 0.1f, color, 0.1f);
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

        if (isGrounded && !animator.GetBool("Jumping"))
        {
            JumpProceed();
        }
    }

    private void JumpProceed()
    {
        animator.SetTrigger("Jump");
        animator.SetBool("Jumping", true);
        Invoke("ApplyJumpForce", 0.35f);
    }

    private void ApplyJumpForce()
    {
        rb.AddForce(transform.up * jumpAcceleration);
    }

    private void Land()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall") && rb.velocity.y <= 0)
        {
            Ray ray = new Ray(transform.localPosition + transform.up * 1f, -transform.up);
            RaycastHit[] hits = Physics.RaycastAll(ray, 2f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.layer != 12)
                {
                    animator.SetBool("Jumping", false);
                }
            }
        }
    }

    private void MoveCharacter()
    {
        float horizontalVel = Vector3.ProjectOnPlane(rb.velocity, Vector3.up).magnitude;

        // Animate character
        if (horizontalVel > runTreshold)
        {
            animator.SetBool("Running", true);

            if (dust.isStopped)
            {
                dust.Play();
            }
        }
        else if (moveDirection == Vector3.zero)
        {
            animator.SetBool("Running", false);
            if (dust.isPlaying)
            {
                dust.Stop();
            }
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
