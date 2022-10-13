using Abed.Utils;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Abed.Controler { 
   public class MovementBhaviour : MonoBehaviour
{
    Jump JumpS;
    Animator animator;
    Rigidbody2D rb;

        Controls controls;
        Utilis util;
        SwipeDetection touch;
        [SerializeField] bool touchControl;
        [SerializeField] bool lockVelocity;

        [SerializeField] float maxSpeed = 25;
        [SerializeField] float acceleration = 1;
        [SerializeField] int speed = 500;
        [SerializeField] int jumpVelocity = 15;
        float dir;
        float inputValue;
        void Awake()
    {
        JumpS = FindObjectOfType<Jump>();  
        animator= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
         util = FindObjectOfType<Utilis>();
            touch = FindObjectOfType<SwipeDetection>();
        }
    private void Start()
    {
        // B short for behaviuor
        JumpS.OnJump += OnJumpB;
        JumpS.OnGround += OnGroundB;
        JumpS.OnCoyoteJump += OnCoyoteJumpB;
        JumpS.Onlanding += OnlandingB;
        JumpS.OnCrouch += OnCrouchB;
    }
        private void FixedUpdate()
        {
            FindObjectOfType<ScreenLog>().Log(4, controls.movement.walk.ReadValue<float>());
            print(controls.movement.walk.ReadValue<float>());
        }
        void Update()
    {
        OnfalingB();



        }
        private void OnCoyoteJumpB(float jumpVelocity)
    {
        print("coyot");
        rb.velocity = new Vector2(0, jumpVelocity);
        animator.SetBool("jump_A", true);
    }
    void OnJumpB(float jumpVelocity)
    {
        animator.SetBool("jump_A", true);
        rb.velocity = new Vector2(0, jumpVelocity);


    }
    void OnGroundB()
    {
        animator.SetBool("jump_A", false);
    }  
    void OnfalingB()
    {
        animator.SetBool("falling_a", JumpS.falling);

    }
    void OnlandingB(RaycastHit2D hit)
    {
        animator.SetBool("jump_A", false);
    }
    void OnCrouchB(bool crouch)
    {
        animator.SetBool("crouch_A", crouch);
    }
}
}