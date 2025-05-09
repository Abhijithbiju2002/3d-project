using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotation_speed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpCoolDown = 1f;

    bool canJump = true;
    bool isGrounded;
    bool hasControl = true;

    float ySpeed;
    Quaternion TargetRotation;

    CameraFollow1 cameraController;
    Animator animator;
    CharacterController characterController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraFollow1>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;
        var moveDirection = cameraController.PlanarRotation * moveInput;



        //for sprinting
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = moveAmount > 0f;
        bool isRunning = shiftHeld && isMoving;

        //walking
        moveSpeed = isMoving ? 2f : 0f;
        animator.SetBool("isWalking", isMoving);
        //sprint
        moveSpeed = isRunning ? 8f : 2f;
        animator.SetBool("isRunning", isRunning);

        if (!hasControl) return;

        GroundCheck();
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            ySpeed = -0.5f;
            if (Input.GetButtonDown("Jump") && isMoving && canJump)//for Jump
            {
                animator.SetTrigger("Jump");
                ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity); //physics formula
                StartCoroutine(JumpCoolDownRoutine());
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDirection * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)//checking if the player is moving
        {

            // transform.position += moveDirection * moveSpeed * Time.deltaTime;
            TargetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation,
            rotation_speed * Time.deltaTime);

        animator.SetFloat("MoveAmount", moveAmount, 0.2f, Time.deltaTime);
    }
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }
    public void SetControl(bool hasControl)
    {
        this.hasControl = hasControl;
        characterController.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("MoveAmount", 0f);
            TargetRotation = transform.rotation;
        }
    }
    private IEnumerator JumpCoolDownRoutine()
    {//jump cooldown
        canJump = false;
        yield return new WaitForSeconds(jumpCoolDown);
        canJump = true;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
