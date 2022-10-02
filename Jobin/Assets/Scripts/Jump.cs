using Abed.Utils;
using UnityEngine;

public class Jump : MonoBehaviour
{
    #region filds
    Controls controls;
    Collider2D colider;
    Rigidbody2D rb;
    Animator animator;
    shosColider ShosColider;
    Utilis utils;
    TochTest touch;
    ScreenLog Slog;
   // [SerializeField] Transform rayPos;
      Transform rayPos;
    [SerializeField] Collider2D normalColider, crouchColider;

    public enum JumpStat { Grounded, PrepareToJump, Jumping, InFlight, Falling, Lande };
    public JumpStat jumpstate = JumpStat.Grounded;

    bool israyHit;
    bool grounded, falling = false, JumpPresed,jumping, canUseCcoyote,crouch,crouchPresed, onlyOne;
    bool coyot = false;
    [SerializeField] bool toucContol = true;
    // bool ColidDown=false,ColidUp=false,ColidLeft = false,ColidRight = false; //

    int grounded_C, perper_C, jump_C, inFlight_C, landed_C;
    private float lastTimeJump, lastTimeGrounded;

    [Header("jump value")]
    [SerializeField] float coyoteTimeTereshold = 1;
    [SerializeField] float GravityMultiply = 2.5f;
    [SerializeField] float LowJumpMultyPly = 2f;
    [SerializeField] float landingDistance = 0.3f;
    [SerializeField] int jumpVelocity = 20;
    [SerializeField] float jumpAcclreation = 0.5f;

    [Header("ray")]
    [SerializeField] float downRayLeant = 5;
    #endregion

    private void Awake()
    {
        colider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        Slog = FindObjectOfType<ScreenLog>();
        ShosColider = GetComponentInChildren<shosColider>();
        utils = GetComponent<Utilis>();
        touch = GetComponent<TochTest>();
        controls = new Controls();
        controls.movement.Enable();

        rayPos = GameObject.Find("rayPos").transform;
        Slog.Log(4,israyHit);
    }
    private void Update()
    {
        jumpProtocol();
        performCrouch();
    }
    public void jumpProtocol()
    {
        setKey();
        setGround();
        performJump();
        coyoteJump();
        SetGravity();
        Ray();
        debugJUmp();
    }
    private void coyoteJump()
    {
        if (!grounded && lastTimeGrounded + coyoteTimeTereshold < Time.time && canUseCcoyote)
        {
            if (JumpPresed)
            {
                print("coyot");
                rb.velocity = new Vector2(0, jumpVelocity);
                animator.SetBool("jump_A", true);
                canUseCcoyote = false;
            }
        }
        else coyot = false;
    }
    private void setKey()
    {
        // touch contorl only 
        if (toucContol)
        {
            JumpPresed = touch.SwipeUp;
            crouch = touch.SwipeDown;
        }

        //normal control
        controls.movement.jump.performed += ctx => { JumpPresed = true; };
        controls.movement.jump.canceled += ctx => { JumpPresed = false; };
        controls.movement.crouch.performed += ctx => { crouch = true; };
    }

    private void setGround()
    {
        grounded = ShosColider.getGround();
        lastTimeJump = ShosColider.lastTimeJump;
        lastTimeGrounded = ShosColider.lastTimeGrounded;
        if (grounded == false)
        {
            jumping = false;
            jumpstate = JumpStat.InFlight;
        }
        if (grounded)
        {
            jumpstate = JumpStat.Grounded; //to check for dellet 
            falling = false;
            canUseCcoyote = true;
            animator.SetBool("jump_A", false);
        }
    }
    private void performJump()
    {
        if (JumpPresed && (grounded) ||
           (jumpstate == JumpStat.Lande && falling))
        {
            jumpstate = JumpStat.PrepareToJump;
            perper_C += 1;
            lastTimeJump = Time.time;
            canUseCcoyote = false;
        }
        if (jumpstate == JumpStat.PrepareToJump)
        {
            jumpstate = JumpStat.Jumping;
            jump_C += 1;
            jumping = true;
            onlyOne = true;
            crouch = false;
            animator.SetBool("jump_A", true);
            rb.velocity = new Vector2(0, jumpVelocity);
        }

        animator.SetBool("falling_a", falling);
       
        
        
    }
    private void SetGravity()
    {
        if (rb.velocity.y < -0.01f)
        {
            falling = true;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (GravityMultiply - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y >= 0)
        {
            falling = false;
        }
        if (!JumpPresed && rb.velocity.y > 0.01f)
        {
            //  if() rb.velocity = Vector2.zero;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultyPly - 1) * Time.deltaTime;
        }
    }
    void Ray()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(rayPos.position.x
        , rayPos.position.y - 0.09f), Vector2.down, downRayLeant, 1);

        Debug.DrawRay(rayPos.transform.position, Vector2.down, Color.black, downRayLeant);
        if (hit.collider != null)
        {
            israyHit = true;
            Slog.Log(3, ("is ray hit = " + israyHit +
           "||rayhit tag =" + hit.collider.tag + "||distance" + hit.distance).ToString());
            if (falling && hit.distance <= landingDistance)
            {
                jumpstate = JumpStat.Lande;
                if (onlyOne)
                {
                    landed_C += 1;
                    animator.SetBool("jump_A", false);
                    onlyOne = false;
                    
                }
            }
        }
        else israyHit = false;
    }
    void performCrouch()
    {
        if (crouch)
        {
            normalColider.enabled = false; // delete
            crouchColider.enabled = true;
            animator.SetBool("crouch_A", true);
        }
        else
        {
            normalColider.enabled = true;
            crouchColider.enabled = false;
            animator.SetBool("crouch_A", false) ;

        }
        float dir =Mathf.Abs( controls.movement.walk.ReadValue<float>());
        if (dir > 0)
        {
            crouch = false;
        }
    }
    void debugJUmp()
    {
        Slog.Log(0, "is coyoty = " + coyot);
        Slog.Log(1, "jumpStat:" + jumpstate.ToString() + "||faling=" + falling);
        Slog.Log(2, "grounded =" + ShosColider.grounded_C + "||" + "perper=:" + perper_C / 2 + "||" + "jump=" + jump_C / 2 + "||" + "flight=" + ShosColider.inFlight_C + "||" + "landed" + landed_C);
        Slog.Log(4, "last ground = " + lastTimeGrounded + "||" + " last jump=" + "||" + lastTimeJump);
        Slog.Log(6, Time.time.ToString()); 
    }
    public void testJump()
    {
        if (GetComponent<TochTest>().SwipeUp)
        {
            FindObjectOfType<ScreenLog>().Log(5, "up");

        }

    }
}