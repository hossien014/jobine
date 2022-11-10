using Abed.Utils;
using UnityEngine;


namespace Abed.Controler
{
    [RequireComponent(typeof(Walk_Controler))]
    [RequireComponent(typeof(Jump_Controler))]
    public class MovementBhaviour_Controler : MonoBehaviour
    {
        //  Animator animator;
        //  Rigidbody2D rb;
        Controls controls;
        _UtilsM _utils;
        SwipeDetection_Controler touch;
        [SerializeField] bool touchControl = false;
        #region walk
        Walk_Controler walk;
        [Header("walke setting")]
        [SerializeField] float acceleration = 1;
        [SerializeField] bool lockVelocity;
        [SerializeField] float maxSpeed = 25;
        [SerializeField] float speed = 500;
        bool IswalkCanceld = false;
        float dir;
        float inputValue;
        #endregion
        #region jump
        Jump_Controler JumpS;

        [Header("jump setting")]
        [SerializeField] int jumpVelocity = 15;
        [SerializeField] float coyoteTimeTereshold = 1;
        [SerializeField] float GravityMultiply = 2.5f;
        [SerializeField] float LowJumpMultyPly = 2f;
        [SerializeField] float landingDistance = 0.3f;
        [SerializeField] float jumpAcclreation = 0.5f;
        [SerializeField] float downRayLeant = 5;
        #endregion
        void Awake()
        {
            JumpS = GetComponent<Jump_Controler>();
            // animator= GetComponent<Animator>();
            //rb = GetComponent<Rigidbody2D>();
            _utils = FindObjectOfType<_UtilsM>();
            touch = FindObjectOfType<SwipeDetection_Controler>();
            controls = new Controls();
            controls.movement.Enable();
            walk = GetComponent<Walk_Controler>();
        }
        private void FixedUpdate()
        {
            setting();
            IntractWhitWalke();
        }

        private void setting()
        {
            walk.Setting(lockVelocity, maxSpeed, speed);
            JumpS.Setting(touchControl, coyoteTimeTereshold, GravityMultiply, LowJumpMultyPly, landingDistance, jumpVelocity, downRayLeant);
        }

        private void IntractWhitWalke()
        {
            GetDir();
            IswalkCanceld = false;
            controls.movement.walk.canceled += ctx => { IswalkCanceld = true; };
            walk.Move(GetDir(), IswalkCanceld);
        }

        private float GetDir()
        {
            if (touchControl)
            {
                if (touch.SwipeRight) dir = _utils.TimeAcceleration(1, acceleration);
                if (touch.SwipeLeft) dir = _utils.TimeAcceleration(-1, acceleration);
                if (touch.tap || touch.SwipeDown) dir = 0f;
                return dir;
            }
            else
            {
                inputValue = controls.movement.walk.ReadValue<float>();
                dir = _utils.TimeAcceleration(inputValue, acceleration);
                return dir;
            }
        }

    }
}