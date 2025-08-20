using UnityEngine;
using static GameManager;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameConfig config;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip jumpClip;

    private CharacterController controller;
    private float verticalVelocity;
    private Vector3 startPosition;

    private AudioSource footstepSource;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        footstepSource = GetComponent<AudioSource>();
        startPosition = transform.position;
    }

    public Vector3 GetStartPosition() => startPosition;

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.State != GameManager.GameState.Playing) {
            footstepSource.Stop();
                return; }

        // Read input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        // Move relative to camera
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 move = (forward * v + right * h).normalized;

        footstepSource.pitch = sprint ? 1.2f : 1f;

        float speed = config.playerMoveSpeed * (sprint ? config.sprintMultiplier : 1f);
        Vector3 horizontal = move * speed;


        
        // Gravity & Jump
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            if (jump)
            {
                
                animator.SetTrigger("Jump");
                verticalVelocity = Mathf.Sqrt(-2f * config.gravity * config.jumpHeight);
                SoundManager.Instance.PlaySFX(jumpClip);
            }
        }
        else
        {
            footstepSource.Stop();
            verticalVelocity += config.gravity * Time.deltaTime;
        }

        Vector3 velocity = horizontal + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // ---- Character rotation ----
        if (move.sqrMagnitude > 0.01f) // only rotate when moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, config.rotationSpeed * Time.deltaTime);
            if (!footstepSource.isPlaying && GameManager.Instance.State == GameState.Playing)
            {
                footstepSource.clip = runClip;
                footstepSource.loop = true;
                footstepSource.Play();
            }
            //else
            //    footstepSource.Stop();
        }
        else
        {
            footstepSource.Stop();
            
        }

        // ---- Animation parameters ----
        float currentSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("IsSprinting", sprint);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }
}