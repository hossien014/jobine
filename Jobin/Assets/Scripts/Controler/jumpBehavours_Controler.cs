using Abed.Controler;
using UnityEngine;

public class jumpBehavours_Controler : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    Jump_Controler JumpS;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        JumpS = GetComponent<Jump_Controler>();
    }
    void Start()
    {
        // B short for behaviuor
        JumpS.OnJump += OnJumpB;
        JumpS.OnGround += OnGroundB;
        JumpS.OnCoyoteJump += OnCoyoteJumpB;
        JumpS.Onlanding += OnlandingB;
        JumpS.OnCrouch += OnCrouchB;
    }
    void Update()
    {
        OnfalingB();
    }
    void OnCoyoteJumpB(float jumpVelocity)
    {
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
