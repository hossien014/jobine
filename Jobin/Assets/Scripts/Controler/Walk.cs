using UnityEngine;
using System;
using Abed.Utils;

namespace Abed.Controler
{
    public class Walk : MonoBehaviour
    {
        Controls controls;
        Rigidbody2D rb;
        Animator animator;
        Utilis util;
        SwipeDetection touch;
        shosColider sColid;

       public enum MoveState {Stoping,Walking};
       public MoveState movestate=MoveState.Stoping;
 
        [SerializeField] bool touchControl;
        [SerializeField] bool lockVelocity;

        [SerializeField] float maxSpeed = 25;
        [SerializeField] float acceleration = 1;
        [SerializeField] int speed = 500;
        [SerializeField] int jumpVelocity = 15;
       // [SerializeField] int fps;
        float dir;
        float inputValue;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            controls = new Controls();
            animator = GetComponent <Animator>();
            util = FindObjectOfType<Utilis>();
            sColid = GetComponentInChildren<shosColider>();
            touch = FindObjectOfType<SwipeDetection>();
            controls.movement.Enable();
        }
        private void FixedUpdate()
        {
            //Application.targetFrameRate = fps;
            walk();
            LockSpeed();
            Utilis.reloadScene();
        }
        public void walk()
        {
            SetMoveInputs();
            FilpSprit();
            preformWalk();
        }
        private void SetMoveInputs()
        {
            if (!touchControl)
            {
                inputValue = controls.movement.walk.ReadValue<float>();
                dir = util.TimeAcceleration(inputValue, acceleration);
                FindObjectOfType<ScreenLog>().Log(3, dir);
            }
            controls.movement.walk.canceled += ctx => { movestate = MoveState.Stoping; };
            if (Mathf.Abs(dir) > 0) movestate = MoveState.Walking;
            
            // touch 
            if (touch.SwipeRight) dir = util.TimeAcceleration(1, acceleration);
            if(touch.SwipeLeft) dir = util.TimeAcceleration(-1, acceleration);
            if (touch.tap||touch.SwipeDown) dir = 0f;
        }
        private void FilpSprit()
        {
            if (dir > 0) { this.transform.localScale =  new Vector2(1, 1); }
            else if (dir < 0) { this.transform.localScale = new Vector2(-1, 1); }
        }
        private void preformWalk()
        {
            rb.velocity = new Vector2( dir*(speed*Time.fixedDeltaTime), rb.velocity.y);
            if (movestate == MoveState.Stoping) rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("speed_A", Mathf.Abs(dir));

            if (!sColid.getGround())
            {
                animator.SetFloat("speed_A",0);
            }
        }
        public void LockSpeed()
        {
            if (lockVelocity)
            {
                rb.velocity = new Vector2
                (Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
            }
        }
 
    }   
}

