using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 movementDirection;

    [SerializeField] private float gravity = 9.81f;


    [Header("Movement info")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float turnSpeed = 5f;
    private float speed;

    private float verticalVelocity;

    private PlayerControls controls;

    private CharacterController characterController;

    public Vector2 moveInput { get; private set; }

    private Animator animator;

    private bool isRunning;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = moveSpeed;

        AssignInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Character.Run.performed += context =>
        {
            isRunning = true;
            speed = runSpeed;
        };
        controls.Character.Run.canceled += context =>
        {
            isRunning = false;
            speed = moveSpeed;
        };
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, 0.1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, 0.1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }

    private void ApplyRotation()
    {
        Vector3 targetPosition = player.aim.GetMousePosition().point;
        targetPosition.y = transform.position.y;
        
        // Vypočítej směr k cíli
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        
        // Vytvořit cílovou rotaci
        Quaternion desiredRotation = Quaternion.LookRotation(directionToTarget);
        
        // Postupně interpolovat mezi současnou a cílovou rotací
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * speed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        movementDirection.y = verticalVelocity;
    }

    private void OnEnable()
    {
        if (controls != null)
        {
            controls.Enable();
        }
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Disable();
        }
    }
}
