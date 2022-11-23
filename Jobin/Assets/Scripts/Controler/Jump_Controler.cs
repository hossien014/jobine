using Abed.Utils;
using System;
using UnityEngine;

namespace Abed.Controler
{
    [RequireComponent(typeof(jumpBehavours_Controler))]
    public class Jump_Controler : MonoBehaviour
    {
        #region filds
        public enum JumpStat { Grounded, PrepareToJump, Jumping, InFlight, Falling, Lande };
        public JumpStat jumpstate = JumpStat.Grounded;

        Controls controls;
        Rigidbody2D rb;
        //ShosColider_Controler ShosColider;
        SwipeDetection_Controler touch;
        ScreenLog_Utils Slog;

        [SerializeField] Transform rayPos;
        [SerializeField] LayerMask groundLayer;
        public bool grounded, JumpPresed, jumping, canUseCcoyote, crouch, crouchPresed, onlyOne;
        bool israyHit;
        bool coyot = false;
        public bool falling = false;

        bool toucContol = true;
        [SerializeField] bool itsPlayer = false;
        int grounded_C, perper_C, jump_C, inFlight_C, landed_C;
        private float lastTimeJump, lastTimeGrounded;

        float coyoteTimeTereshold = 1;
        float GravityMultiply = 2.5f;
        float LowJumpMultyPly = 2f;
        float landingDistance = 0.3f;
        float jumpAcclreation = 0.5f;
        int jumpVelocity = 20;
        float downRayLeant = 5;
        #endregion
        #region event
        public event Action<float> OnJump;
        public event Action OnGround;
        public event Action<float> OnCoyoteJump;
        public event Action<RaycastHit2D> Onlanding;
        public event Action<bool> OnCrouch;

        #endregion
        private void Awake()
        {

            rb = GetComponent<Rigidbody2D>();
            Slog = FindObjectOfType<ScreenLog_Utils>();
           // ShosColider = GetComponentInChildren<ShosColider_Controler>();
            touch = FindObjectOfType<SwipeDetection_Controler>();
            controls = new Controls();
            controls.movement.Enable();
            rayPos = transform.Find("rayPos").transform;
        }
        private void Update()
        {
            jumpProtocol();
            PerformCrouch();
        }


        public void jumpProtocol()
        {
            if (itsPlayer) setKey();
            setGround();
            performJump();
            if (itsPlayer) coyoteJump();
            SetGravity();
            //Ray();
            debugJUmp();
        }


        private void setKey()
        {
            if (toucContol)
            {
                JumpPresed = touch.SwipeUp;
                crouch = touch.SwipeDown;
            }
            else
            {
                controls.movement.jump.performed += ctx => { JumpPresed = true; };
                controls.movement.jump.canceled += ctx => { JumpPresed = false; };
                controls.movement.crouch.performed += ctx => { crouch = true; };
            }
        }
        private void setGround()
        {
            //grounded = ShosColider.getGround();
           // lastTimeJump = ShosColider.lastTimeJump;
           // lastTimeGrounded = ShosColider.lastTimeGrounded;
            boxRayGroundCheck();
            if (grounded == false)
            {
                jumping = false;
                jumpstate = JumpStat.InFlight;
            }
            if (grounded)
            {
                lastTimeGrounded=Time.time; //fix this
                jumpstate = JumpStat.Grounded; //to check for dellet 
                falling = false;
                canUseCcoyote = true;
                OnGround?.Invoke();
            }

        }

        private void boxRayGroundCheck()
        {
          
            var boxcolier = GetComponent<BoxCollider2D>();
            Color colColor = Color.green;
            float extraHeghit = 2f;
            RaycastHit2D ratHit = Physics2D.BoxCast(boxcolier.bounds.center, boxcolier.bounds.size, 0f, Vector2.down, extraHeghit, groundLayer);

            if (ratHit.collider == null) { 
             grounded = false;
             colColor = Color.red; }
            else { 
            grounded = true;
            colColor = Color.green;
                if (falling && ratHit.distance <= landingDistance)
                {
                    jumpstate = JumpStat.Lande;
                    if (onlyOne)
                    {
                        landed_C += 1;
                        onlyOne = false;
                        Onlanding?.Invoke(ratHit);
                    }
                }
            }
           
            ShowBoxRay(boxcolier, colColor, extraHeghit);
        }

        private static void ShowBoxRay(BoxCollider2D boxcolier, Color colColor, float extraHeghit)
        {
            Debug.DrawRay(boxcolier.bounds.center + new Vector3(boxcolier.bounds.extents.x, 0), Vector3.down * (boxcolier.bounds.extents.y + extraHeghit), colColor);
            Debug.DrawRay(boxcolier.bounds.center - new Vector3(boxcolier.bounds.extents.x, 0), Vector3.down * (boxcolier.bounds.extents.y + extraHeghit), colColor);
            Debug.DrawRay(boxcolier.bounds.center - new Vector3(boxcolier.bounds.extents.x, boxcolier.bounds.extents.y + extraHeghit), Vector3.right * (boxcolier.bounds.extents.x + extraHeghit), colColor);
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
                OnJump?.Invoke(jumpVelocity);
            }
        }
        private void coyoteJump()
        {
            if (!grounded && lastTimeGrounded + coyoteTimeTereshold < Time.time && canUseCcoyote)
            {
                if (JumpPresed)
                {
                    OnCoyoteJump(jumpVelocity);
                    canUseCcoyote = false;
                }
            }
            else coyot = false;
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
                rb.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultyPly - 1) * Time.deltaTime;
            }
        }
       /* void Ray()
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(rayPos.position.x
            , rayPos.position.y - 0.09f), Vector2.down, downRayLeant, 1);

            Debug.DrawRay(rayPos.transform.position, Vector2.down, Color.black, downRayLeant);
            if (hit.collider != null)
            {
                israyHit = true;
                // Slog.Log(3, ("is ray hit = " + israyHit +
                //"||rayhit tag =" + hit.collider.tag + "||distance" + hit.distance).ToString());

                if (falling && hit.distance <= landingDistance)
                {
                    jumpstate = JumpStat.Lande;
                    if (onlyOne)
                    {
                        landed_C += 1;
                        onlyOne = false;
                        Onlanding?.Invoke(hit);
                    }
                }
            }
            else israyHit = false;
        }*/
        void PerformCrouch()
        {
            OnCrouch?.Invoke(crouch);
            float dir = Mathf.Abs(controls.movement.walk.ReadValue<float>());
            if (dir > 0)
            {
                crouch = false;
            }
        }

        public void Setting(bool toucContol, float coyoteTimeTereshold, float GravityMultiply, float LowJumpMultyPly, float landingDistance, int jumpVelocity, float downRayLeant)
        {
            this.toucContol = toucContol;
            this.coyoteTimeTereshold = coyoteTimeTereshold;
            this.GravityMultiply = GravityMultiply;
            this.LowJumpMultyPly = LowJumpMultyPly;
            this.landingDistance = landingDistance;
            this.jumpVelocity = jumpVelocity;
            this.downRayLeant = downRayLeant;
        }
        void debugJUmp() //delete for final build
        {
            //Slog.Log(0, "is coyoty = " + coyot);
            //Slog.Log(1, "jumpStat:" + jumpstate.ToString() + "||faling=" + falling);
            //Slog.Log(2, "grounded =" + ShosColider.grounded_C + "||" + "perper=:" + perper_C / 2 + "||" + "jump=" + jump_C / 2 + "||" + "flight=" + ShosColider.inFlight_C + "||" + "landed" + landed_C);
            //Slog.Log(4, "last ground = " + lastTimeGrounded + "||" + " last jump=" + "||" + lastTimeJump);
            //Slog.Log(6, Time.time.ToString());
        }
    }
}