using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private CharacterController controller;
    [SerializeField, Min(0)] private float speed = 5f;
    [SerializeField, Min(0)] private float rotationSpeed = 10f;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField, Min(0)] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField, Min(0)] private float jumpHeight = 2f;

    public Vector3 movement { get; private set; }
    private Vector3 gravitationalForce;
    public bool isGrounded { get; private set; }
    public bool jumpMomentumCheck { get; private set; }

    [SerializeField] private Animator animator;
    
    private MarioState currentState = MarioState.Idle;
    
    private float idleTimer = 0f;
    private float maxIdleTime = 10f; // Tiempo máximo en segundos antes de ejecutar la animación extra


    private enum MarioState
    {
        Idle,
        Walk,
        Run,
        Fall,
        Jump,
        Punch1,
        Punch2,
        Punch3,
        Hit,
        Die
    }

  //  [Header("Audio")]
  //  [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // check if mario is grounded
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
        isGrounded = false;
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (!hitColliders[i].isTrigger)
            {
                isGrounded = true;
                break;
            }
        }

        // calculate movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
        movement = right * horizontal + forward * vertical;

        // check if player is trying to move
        if (movement != Vector3.zero)
        {
            // look in the direction of the movement
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.deltaTime);

            if (isGrounded)
            {
                //audioSource.enabled = true;
                //audioSource.loop = true;
            }
            else
            {
                //audioSource.enabled = false;
                //audioSource.loop = false;
            }
        }
        else
        {
           // audioSource.enabled = false;
           // audioSource.loop = false;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                currentState = MarioState.Jump;
            }
            /*else if (!isGrounded && jumpMomentumCheck)
            {
                currentState = MarioState.DoubleJump;
            }
            else if (!isGrounded && !jumpMomentumCheck)
            {
                currentState = MarioState.TripleJump;
            }*/
        }
        /*else if (Input.GetButtonDown("Punch"))
        {
            // Change to punch state based on input
            if (currentState == MarioState.Idle || currentState == MarioState.Walk || currentState == MarioState.Run)
            {
                currentState = MarioState.Punch1;
            }
            else if (currentState == MarioState.Punch1)
            {
                currentState = MarioState.Punch2;
            }
            else if (currentState == MarioState.Punch2)
            {
                currentState = MarioState.Punch3;
            }
        }*/
        else if (movement.magnitude > 0)
        {
            // Change to walk or run state based on movement input
            currentState = Input.GetKey(KeyCode.LeftShift) ? MarioState.Run : MarioState.Walk;
        }
        else
        {
            // Change to idle state if no movement or jump input
            currentState = MarioState.Idle;
        }

        jumpMomentumCheck = jumpMomentumCheck && Input.GetButton("Jump") && !isGrounded;

        // simulate gravity
        if (isGrounded)
        {
            // mario is standing on ground
            gravitationalForce.y = gravity * Time.deltaTime;
            jumpMomentumCheck = true;
        }
        else
        {
            // mario is in the air
            if (!jumpMomentumCheck && gravitationalForce.y > 0)
                gravitationalForce.y = 0;
            else
                gravitationalForce.y += gravity * Time.deltaTime;
        }

        // jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            gravitationalForce.y = Mathf.Sqrt(-2 * jumpHeight * gravity);
            animator.Play("Jump", - 1, 0f);
        }
        // move mario
        controller.Move((movement * speed * Time.deltaTime) + (gravitationalForce * Time.deltaTime));
        
        
        switch (currentState)
        {
            case MarioState.Idle:
                HandleIdleState();
                break;

            case MarioState.Walk:
                HandleWalkState();
                break;

            case MarioState.Run:
                HandleRunState();
                break;

            case MarioState.Fall:
                HandleFallState();
                break;

            case MarioState.Jump:
                HandleJumpState();
                break;

            case MarioState.Punch1:
                HandlePunch1State();
                break;

            case MarioState.Punch2:
                HandlePunch2State();
                break;

            case MarioState.Punch3:
                HandlePunch3State();
                break;

            case MarioState.Hit:
                HandleHitState();
                break;

            case MarioState.Die:
                HandleDieState();
                break;
        }
        
        if (currentState == MarioState.Idle)
        {
            // Incrementar el temporizador de Idle
            idleTimer += Time.deltaTime;

            // Si el temporizador alcanza el tiempo máximo, ejecutar animación extra
            if (idleTimer >= maxIdleTime)
            {
                HandleExtraIdle();
            }
        }
        else
        {
            // Si no está en estado Idle, reiniciar el temporizador
            idleTimer = 0f;
        }
        
    }
    
    // Add methods to handle each state
    private void HandleIdleState()
    {
        
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
        // Implement logic for the Idle state
    }

    private void HandleWalkState()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
        // Implement logic for the Walk state
    }

    private void HandleRunState()
    {
        // Implement logic for the Run state
    }

    private void HandleFallState()
    {
        // Implement logic for the Fall state
    }

    private void HandleJumpState()
    {
        // Implement logic for the Jump state
    }

    private void HandleDoubleJumpState()
    {
        // Implement logic for the DoubleJump state
    }

    private void HandleTripleJumpState()
    {
        // Implement logic for the TripleJump state
    }

    private void HandleLongJumpState()
    {
        // Implement logic for the LongJump state
    }

    private void HandleWallJumpState()
    {
        // Implement logic for the WallJump state
    }

    private void HandlePunch1State()
    {
        // Implement logic for the Punch1 state
    }

    private void HandlePunch2State()
    {
        // Implement logic for the Punch2 state
    }

    private void HandlePunch3State()
    {
        // Implement logic for the Punch3 state
    }

    private void HandleHitState()
    {
        // Implement logic for the Hit state
    }

    private void HandleDieState()
    {
        // Implement logic for the Die state
    }
    
    private void HandleExtraIdle()
    {
        animator.SetTrigger("Extra Idle");
        idleTimer = 0f;
        // Implement logic for the Walk state
    }
}
